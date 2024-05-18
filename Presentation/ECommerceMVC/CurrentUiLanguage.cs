using System.Globalization;

namespace ECommerceMVC
{
    public static class CurrentUiLanguage
    {
        public static string LanguageCode => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "az-Latn-AZ" ? "az" : CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        public static string LanguageUrlPrefix => string.IsNullOrWhiteSpace(LanguageCode) ? "" : $"/{LanguageCode}";
    }
}
