using MultiLsTokenServer.Domain.Response.Header;

namespace MultiLsTokenServer.Infrastructure.Commands.TestDisplay;

using MultiLsTokenServer.Domain.Interfaces.Commands.TestDisplay;
using MultiLsTokenServer.Domain.Request;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Header;
using MultiLsTokenServer.Domain.Response.Payloads;
using static HsmResponseHeaderFactory;

public class MeterTestDisplayService : IMeterTestDisplayService
{
    private static readonly string[] actions = [
        "Do test No. 2 to 5 plus, optionally, any other; inclusion of test No. 2 is mandatory if implemented",
        "Test supported load switch(es)",
        "Test supported display(s) and/or device(s)",
        "Display cumulative usage register totals",
        "Display the KRN and KT value",
        "Display the TI value",
        "Test the token input device",
        "Display maximum power limit",
        "Display tamper status",
        "Display active load power",
        "Display software version",
        "Display phase power unbalance limit",
        "Display water meter factor (reserved for future definition by the STS Association)",
        "Display tariff rate(reserved for future definition by the STS Association)",
        "Display the EA value",
        "Display number of key change tokens supported",
        "Display the SGC value Mandatory for 3 or 4 KCT meters",
        "Display the KEN value",
        "Display the DRN value",
        "Reserved for future assignment by the STS Association"];

    private static readonly string[] testTokensSubClass0 = [
        "5649 3153 7254 5031 3471",
        "0000 0000 0001 5099 7584",
        "0000 0000 0001 6777 4880",
        "0000 0000 0002 0132 8896",
        "1844 6744 0738 4377 2416",
        "3689 3488 1475 5332 2496",
        "0000 0000 0006 7109 3248",
        "0000 0000 0012 0797 4400",
        "0000 0000 0022 8172 8512",
        "0000 0000 0044 2920 8064",
        "0000 0000 0087 2419 5840",
        "0000 0000 0173 1410 5857",
        "0000 0000 0344 9399 1426",
        "0000 0000 0688 5369 7029",
        "0000 0000 1375 7317 3770",
        "0000 0000 2750 1212 7252",
        "0000 0000 5498 9003 4216",
        "0000 0001 0996 4584 8124",
        "0000 0002 1991 5747 5960",
        string.Empty];

    private static readonly string[] testTokensSubClass1 = [
        "0230 5843 0050 5295 1967",
        "0115 2921 5090 3605 4672",
        "0115 2921 5133 3104 2448",
        "0115 2921 5219 2095 2465",
        "0115 2921 5391 0083 8034",
        "0115 2921 5734 6054 3637",
        "0115 2921 6421 8002 0378",
        "0115 2921 7796 1897 3828",
        "0115 2922 0544 9688 0824",
        "0115 2922 6042 5269 4700",
        "0115 2923 7037 6432 2536",
        "0115 2925 9027 8757 7952",
        "0115 2930 3008 3408 9776",
        "0115 2939 0969 2711 2592",
        "0115 2956 6891 1315 4192",
        "0115 2991 8734 8524 9680",
        "0115 3062 2422 2942 8368",
        "0115 3202 9797 1778 8816",
        "0115 3484 4546 9451 4832",
        string.Empty];

    public HsmResponse<MeterTest> GetTestDisplayToken(MeterTestCommandData commandData)
    {
        HsmResponse<MeterTest> result = new();
        try
        {
            if (!commandData.HasErrors)
            {
                result.Header = CreateHeaderFromCode(HsmResponseHeader.SuccessCode);
                result.Payload = commandData.TestNumber >= actions.Length ? new(actions[^1], string.Empty)
                    : new(actions[commandData.TestNumber], commandData.DecoderReferenceNumber.Length == 11 ?
                                                                testTokensSubClass0[commandData.TestNumber] :
                                                                testTokensSubClass1[commandData.TestNumber]);
            }
            else
            {
                result.Header = ParameterValidationError(string.Join(Environment.NewLine, commandData.GetErrors()));
            }
        }
        catch (Exception ex)
        {
            result.Header = ResponseGenerationExceptionHeader(ex.Message);
        }

        return result;
    }
}
