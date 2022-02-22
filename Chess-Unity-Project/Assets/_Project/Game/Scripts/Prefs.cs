using System;
using UnityEngine;

namespace SteampunkChess
{
    public static class Prefs
    {
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