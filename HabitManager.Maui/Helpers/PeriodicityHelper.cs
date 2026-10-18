namespace HabitManager.Maui.Helpers
{
    public static class PeriodicityHelper
    {
        public class SelectionItem  // elements attēlošanai checkboxos
        {
            public DayOfWeek Day { get; set; }
            public string Title => Day.ToString();
            public bool IsSelected { get; set; }
        }
        public static List<SelectionItem> ToSelectionItems(List<DayOfWeek>? selectedDays = null)  // no objekta īpašības iegūt attēlojamus checkboxus
        {
            var periodicity = selectedDays != null ? new List<DayOfWeek>(selectedDays) : new List<DayOfWeek>();

            return Enum.GetValues<DayOfWeek>()
                .Select(d => new SelectionItem
                {
                    Day = d,
                    IsSelected = periodicity.Contains(d)
                })
                .ToList();
        }

        public static List<DayOfWeek> ToEnums(List<SelectionItem> items) => items.Where(d => d.IsSelected).Select(d => d.Day).ToList();  // saglabāt objekta īpašību no attēlojamiem checkboxiem

        public static string ToNormalString(this List<DayOfWeek>? enums)  // pārveidot sarakstu uz tekstu
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
