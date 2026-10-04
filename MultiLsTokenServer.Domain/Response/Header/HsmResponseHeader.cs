namespace MultiLsTokenServer.Domain.Response.Header;

public class HsmResponseHeader
{
    public const string SuccessCode = "00";

    public required string Code { get; set; }
    public required string Identifier { get; set; }
    public required string Description { get; set; }

    public bool IsError => Code != SuccessCode;
}
