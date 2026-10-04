namespace MultiLsTokenServer.Domain.Interfaces.Comunication;

public interface IAsynchronousHSMClient : IDisposable
{
    void ResetConnection();
    bool IsConnected { get; }

    bool ResponseComplete { get; }
    bool ResponseCorrect { get; }

    Action<string>? OnDataRecieved { set; }

    bool Write(string _data);
}