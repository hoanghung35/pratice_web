using System.Globalization;

namespace Content_App.Shared.DatePipe
{
    public class DateConverter
    {
        public string D_Convert(DateTime d)
        {
            return d.ToString("dd/MM/yyyy hh:mm tt", CultureInfor.InvariantCulture);
        }

        public DateTime D_TimeStampNow_Unspecified()
        {
            return DateTime.SpecifyKind(DateTime.UtcNow.Addhours(7), DateTimeKind.Unspecified);
        }
    }
}
