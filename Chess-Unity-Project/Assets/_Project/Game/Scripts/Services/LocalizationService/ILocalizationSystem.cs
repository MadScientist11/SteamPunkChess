﻿namespace SteampunkChess.LocalizationSystem
{
    public interface ILocalizationSystem : IService
    {
        void ChangeLanguage(string languageIdentifier);
    }
}