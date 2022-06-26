using System;
using UnityEngine;

namespace SteampunkChess
{


    public static class Prefs
    {

        public static class Settings
        {
            private const string VSYNC = "Vsync";
            private const string POST_PROCESSING = "PostProcessing";
            private const string WINDOW_SIZE = "WindowSize";
            private const string TEXTURE_QUALITY = "TextureQuality";
            
            private const string MUSIC = "Music";
            private const string SOUNDS = "Sounds";

            private const string LANGUAGE = "Language";
            public static bool Vsync
            {
                get { return PlayerPrefs.GetInt(VSYNC, 0) == 1; }
                set { PlayerPrefs.SetInt(VSYNC, Convert.ToInt32(value)); }
            }
            
            public static bool PostProcessing
            {
                get { return PlayerPrefs.GetInt(POST_PROCESSING, 1) == 1; }
                set { PlayerPrefs.SetInt(POST_PROCESSING, Convert.ToInt32(value)); }
            }

            public static int WindowSize
            {
                get { return PlayerPrefs.GetInt(WINDOW_SIZE, (int) ScreenResolution.FullHD); }
                set { PlayerPrefs.SetInt(WINDOW_SIZE, value); }
            }
            
            public static int TextureQuality
            {
                get { return PlayerPrefs.GetInt(TEXTURE_QUALITY, 0);  }
                set { PlayerPrefs.SetInt(TEXTURE_QUALITY, Convert.ToInt32(value)); }
            }
            
            public static bool Music
            {
                get { return PlayerPrefs.GetInt(MUSIC, 1) == 1; }
                set { PlayerPrefs.SetInt(MUSIC, Convert.ToInt32(value)); }
            }
            
            public static bool Sounds
            {
                get { return PlayerPrefs.GetInt(SOUNDS, 1) == 1; }
                set { PlayerPrefs.SetInt(SOUNDS, Convert.ToInt32(value)); }
            }
            
            public static string Language
            {
                get { return PlayerPrefs.GetString(LANGUAGE, "en"); }
                set { PlayerPrefs.SetString(LANGUAGE, value); }
            }

            


        }

        private const string REMEMBER_ME = "FirstLog";

        private const string USERNAME = "Username";

        private const string PASSWORD = "Password";
        public static bool RememberMe
        {
            get { return PlayerPrefs.GetInt(REMEMBER_ME, 0) == 1; }
            set { PlayerPrefs.SetInt(REMEMBER_ME, Convert.ToInt32(value)); }
        }


        public static string Username
        {
            get { return PlayerPrefs.GetString(USERNAME, String.Empty); }
            set { PlayerPrefs.SetString(USERNAME, value); }
        }

        public static string Password
        {
            get { return PlayerPrefs.GetString(PASSWORD, String.Empty); }
            set { PlayerPrefs.SetString(PASSWORD, value); }
        }
    }
}