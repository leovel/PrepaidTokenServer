using MultiLsTokenServer.Domain.Interfaces.Commands.Base.Utilities;

namespace MultiLsTokenServer.Infrastructure.Commands.Base.Utilities
{
    public class TidControlService : ITidControlService
    {
        private static readonly Dictionary<(string, uint), DateTime> tidControlDict = [];
        public DateTime GetTID(string decoderReferenceNumber, DateTime issueDateTime, uint keyRegister, bool isTidCommand)
        {
            var tidDateTime =
                new DateTime(issueDateTime.Year, issueDateTime.Month, issueDateTime.Day, issueDateTime.Hour, issueDateTime.Minute, 0, DateTimeKind.Utc);

            if (!isTidCommand)
                return tidDateTime;


            if (tidControlDict.TryGetValue((decoderReferenceNumber, keyRegister), out DateTime latsTidDateTime) && latsTidDateTime >= tidDateTime)
            {
                tidDateTime = latsTidDateTime.AddMinutes(1);
            }

            if (tidDateTime.Hour == 0 && tidDateTime.Minute == 1)
                tidDateTime = tidDateTime.AddMinutes(1);

            tidControlDict[(decoderReferenceNumber, keyRegister)] = tidDateTime;

            return tidDateTime;
        }
    }
}
