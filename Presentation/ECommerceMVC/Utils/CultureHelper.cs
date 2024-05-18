namespace ECommerceMVC.Utils
{
    public class CultureHelper
    {
        public static string TryGetLanguageCode(string? lang)
        {
            switch (lang)
            {
                case "en":
                    return "en";
                default:
                    return "az";
            }
        }
        public static string TryGetCulture(string languageCode)
        {
            if (!Cultures.TryGetValue(languageCode, out var culture))
                return "az-Latn-AZ";
            return culture;
        }
        private static readonly Dictionary<string, string> Cultures = new Dictionary<string, string>()
        {
            { "en", "en-US" },
            { "az", "az-Latn-AZ"}
        };
    }
}
