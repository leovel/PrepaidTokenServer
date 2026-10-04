using System.Net.Sockets;
using System.Net;
using System.Text;
using MultiLsTokenServer.Domain.Interfaces.Comunication;
using MultiLsTokenServer.Domain.Exceptions;
using MultiLsTokenServer.Infrastructure.Tools;

namespace MultiLsTokenServer.Infrastructure.Comunication;

using static StandardTrasnferSpecificationTools;

/// <summary>
/// Create a TCP Asynchronous Client. This client is connect to the server and port with passed parameters.
/// </summary>
/// <param name="_ip">Server IP</param>
/// <param name="_port">Server Port</param>
public class AsynchronousHSMClient(string _ip, int _port) : IAsynchronousHSMClient
{
    // *** Event Handlers *** //
    public Action<string>? OnDataRecieved { get; set; }

    // *** Properties *** //

    // Connection Parameters
    public IPAddress IpAddress { get; init; } = IPAddress.Parse(_ip);
    public int Port { get; init; } = _port;

    // Socket Parameters
    private Socket? socket;
    private readonly byte[] readerBuffer = new byte[1024];


    readonly List<byte> responseBytes = [];

    // *** Methods *** //

    private void ClearResponse()
    {
        responseBytes.Clear();
    }

    public bool IsGLER => Encoding.ASCII.GetString([.. responseBytes], 0, responseBytes.Count).Contains(GLER_HEADER);

    public bool ResponseComplete => responseBytes.Count > 9 && (responseBytes.Count == responseBytes[1] + 3 || responseBytes.Count == responseBytes[1] + 2 || IsGLER);
    public bool ResponseCorrect => !IsGLER && ResponseComplete && responseBytes[0] == CommandFirstByte && responseBytes[responseBytes[1] + 1] == CommandLastByte;

    public void ResetConnection()
    {
        Connect();
    }

    /// <summary>
    /// Connect to the server
    /// </summary>
    private void Connect()
    {
        try
        {
            // Close the socket if open
            Disconnect();

            // Create the socket object
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Define the Server address and port
            IPEndPoint epServer = new(IpAddress, Port);

            // Connect to server non-Blocking method
            socket.Blocking = false;
            AsyncCallback onconnect = new(OnConnect);
            socket.BeginConnect(epServer, onconnect, socket);
        }
        catch (Exception ex)
        {
            var secondLine = ex.InnerException != null ? Environment.NewLine + ex.InnerException.Message : string.Empty;
            throw new ConnectionException($"Socket Connection Falied. Message : {ex.Message}.{secondLine}");
        }
    }

    /// <summary>
    /// Check connection status of the socket
    /// </summary>
    /// <returns>True or False based on status</returns>
    public bool IsConnected => socket?.Connected ?? false;

    // Setup Callbacks if Socket is Connected
    private void OnConnect(IAsyncResult ar)
    {
        Socket? _socket = (Socket?)ar.AsyncState;

        try
        {
            SetupRecieveCallback(_socket!);
        }
        catch (Exception ex)
        {
            var secondLine = ex.InnerException != null ? Environment.NewLine + ex.InnerException.Message : string.Empty;
            throw new ConnectionException($"Connection Erro. Message : {ex.Message}.{secondLine}");
        }
    }

    // Setup Recieve Callback for Async Listening
    private void SetupRecieveCallback(Socket _socket)
    {
        try
        {
            AsyncCallback recieveData = new(OnRecievedData);
            _socket.BeginReceive(readerBuffer, 0, readerBuffer.Length, SocketFlags.None, recieveData, _socket);
        }
        catch (Exception ex)
        {
            Dispose();
            var secondLine = ex.InnerException != null ? Environment.NewLine + ex.InnerException.Message : string.Empty;
            throw new ConnectionException($"Recieve Callback Setup Failed.{secondLine}");
        }
    }

    // Recieve data from TCP
    private void OnRecievedData(IAsyncResult ar)
    {
        Socket _socket = (Socket)ar.AsyncState!;

        if (IsConnected)
        {
            try
            {
                // Check data is available
                int nBytesRec = _socket.EndReceive(ar);
                if (nBytesRec > 0)
                {
                    responseBytes.AddRange(readerBuffer.Take(nBytesRec));

                    // Fire Data Recieved Event
                    if (ResponseComplete)
                    {
                        OnDataRecieved?.Invoke(ResponseCorrect ? Encoding.ASCII.GetString([.. responseBytes], 8, responseBytes[1] - 7)
                            : IsGLER ? Encoding.ASCII.GetString([.. responseBytes], 0, responseBytes.Count) : string.Empty);

                        ClearResponse();
                    }


                    // If the Connection is Still Usable Restablish the Callback
                    SetupRecieveCallback(_socket);
                }
                else
                {
                    Dispose();
                }
            }
            catch (Exception ex)
            {
                Dispose();
                var secondLine = ex.InnerException != null ? Environment.NewLine + ex.InnerException.Message : string.Empty;
                throw new ConnectionException($"Recieve Operation Failed. Message : {ex.Message}.{secondLine}");
            }
        }
    }


    /// <summary>
    /// Write Data to Socket
    /// </summary>
    /// <param name="_data">Data to be written</param>
    /// <param name="crc_c">Indicate if CRC_C checksum is used instead of CRC</param>
    /// <returns>Success status as Boolean Value</returns>
    public bool Write(string _data)
    {
        if (!IsConnected)
            ResetConnection();

        _data += CRCof(_data);

        byte lengthByte = Convert.ToByte($"{CommandHeaderBytes.Length + _data.Length + 1:X2}", 16);

        var commandData = new List<byte>
            {
                CommandFirstByte,
                lengthByte
            };
        commandData.AddRange(CommandHeaderBytes);
        commandData.AddRange(Encoding.ASCII.GetBytes(_data));
        commandData.Add(CommandLastByte);

        // Check Connection
        if (IsConnected)
        {
            try
            {
                byte[] commandAllBytes = [.. commandData];
                socket!.Send(commandAllBytes, commandAllBytes.Length, 0);
                return true;
            }
            catch (Exception ex)
            {
                Dispose();
                var secondLine = ex.InnerException != null ? Environment.NewLine + ex.InnerException.Message : string.Empty;
                throw new ConnectionException($"Data Writing Operation Failed.{secondLine}");
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Close the Socket Connection
    /// </summary>
    public void Dispose()
    {
        Disconnect();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disconnect the socket
    /// </summary>
    private void Disconnect()
    {
        ClearResponse();
        if (IsConnected)
        {
            socket?.Shutdown(SocketShutdown.Both);
            Thread.Sleep(10);
            socket?.Close();
        }
    }

    ~AsynchronousHSMClient()
    {
        Dispose();
    }
}

