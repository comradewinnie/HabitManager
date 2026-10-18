namespace HabitManager.Maui.Helpers
{
    public static class PeriodicityHelper
    {
        public static string ToNormalString(this List<DayOfWeek>? enums)
        {
            if (enums == null || !enums.Any())
                return "No schedule";

            var list = enums.ToList();
            if (list.Count == 7)
                return "Every day";

            return string.Join(", ", list);
        }
    }
}
