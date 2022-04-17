using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Zenject;

namespace SteampunkChess.LocalizationSystem
{
    [CreateAssetMenu(fileName = "LocalizationSystem", menuName = "Services/LocalizationSystem")]
    public class LocalizationSystemSO : ScriptableObject, ILocalizationSystem
    {
        public string InitializationMessage { get; } = "Initialize localization service...";

        [Inject]
        private void Construct(ServiceContainer serviceContainer)
        {
            serviceContainer.ServiceList.Add(this);
        }

        public async Task Initialize()
        {
            await LocalizationSettings.InitializationOperation.Task;
            ChangeLanguage(Prefs.Settings.Language);
        }

        public string GetLocalizedString(string tableName, string localizationKey)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(tableName, localizationKey);
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