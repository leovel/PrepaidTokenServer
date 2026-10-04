using MultiLsTokenServer.Domain.Validation;

namespace MultiLsTokenServer.Domain.Request;

public class RawCommandData(string command) : NotifyDataErrorInfo<RawCommandData>
{
    public string Command { get; } = command;
}
