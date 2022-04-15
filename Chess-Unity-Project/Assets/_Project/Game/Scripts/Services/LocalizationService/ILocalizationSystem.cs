namespace SteampunkChess.LocalizationSystem
{
    public interface ILocalizationSystem : IService
    {
        void ChangeLanguage(string languageIdentifier);
        string GetLocalizedString(string tableName, string localizationKey);
    }
}