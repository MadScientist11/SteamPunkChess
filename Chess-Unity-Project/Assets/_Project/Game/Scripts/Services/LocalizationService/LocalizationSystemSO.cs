using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace SteampunkChess.LocalizationSystem
{
    [CreateAssetMenu(fileName = "LocalizationSystem", menuName = "Services/LocalizationSystem")]
    public class LocalizationSystemSO : ScriptableObject, ILocalizationSystem
    {
        public void GetLocalizedString(string tableName, string localizationKey)
        {
            LocalizationSettings.StringDatabase.GetLocalizedString(tableName, localizationKey);
           
        }
        public void GetLocalizedString(string tableName, string localizationKey, params object[] arguments)
        {
            LocalizationSettings.StringDatabase.GetLocalizedString(tableName, localizationKey, arguments);         
        }

        public void ChangeLanguage(string languageIdentifier)
        {
            LocalizationSettings settings = LocalizationSettings.Instance;
            LocaleIdentifier localeCode = new LocaleIdentifier(languageIdentifier);
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
            {
                Locale aLocale = LocalizationSettings.AvailableLocales.Locales[i];
                LocaleIdentifier anIdentifier = aLocale.Identifier;
                if (anIdentifier == localeCode)
                {
                    LocalizationSettings.SelectedLocale = aLocale;
                }
            }
        }

    }
}
