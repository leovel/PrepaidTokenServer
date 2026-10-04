namespace MultiLsTokenServer.Grpc.Api;

public partial class HsmDateTime
{
    public static implicit operator DateTime(HsmDateTime hsmDateTime)
    {
        try
        {
            return new DateTime(
                (int)hsmDateTime.Year,
                (int)hsmDateTime.Month,
                (int)hsmDateTime.Day,
                (int)hsmDateTime.Hour,
                (int)hsmDateTime.Minute,
                (int)hsmDateTime.Second, DateTimeKind.Utc);
        }
        catch (Exception)
        {
            return default;
        }
    }

    public static implicit operator HsmDateTime(DateTime dateTime)
    {
        return new HsmDateTime
        {
            Year = (uint)dateTime.Year,
            Month = (uint)dateTime.Month,
            Day = (uint)dateTime.Day,
            Hour = (uint)dateTime.Hour,
            Minute = (uint)dateTime.Minute,
            Second = (uint)dateTime.Second
        };
    }
}
