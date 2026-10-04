namespace MultiLsTokenServer.Domain.Response.Payloads;

public record VkAttributes(uint BDT, uint KEN)
{
    public VkAttributes() : this(0, 0) { }
}
