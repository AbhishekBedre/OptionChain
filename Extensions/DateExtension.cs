namespace OptionChain.Extensions
{
    public static class DateExtensions
    {
        private static readonly TimeZoneInfo IstZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");

        public static DateTime ToIst(this DateTime utcDate)
        {
            if (utcDate.Kind == DateTimeKind.Unspecified)
                utcDate = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, IstZone);
        }
    }

}
