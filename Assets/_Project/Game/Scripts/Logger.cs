#if UNITY_EDITOR
#define ENABLE_LOGS
#endif
using System.Diagnostics;

namespace SteampunkChess
{

    public static class Logger
    {
        static Logger()
        {
#if !ENABLE_LOGS
            UnityEngine.Debug.unityLogger.logEnabled = false;
#endif
        }

        [Conditional("ENABLE_LOGS")]
        public static void Debug(string logMsg)
        {
            UnityEngine.Debug.Log(logMsg);
        }

        [Conditional("ENABLE_LOGS")]
        public static void DebugError(string logMsg)
        {
            UnityEngine.Debug.LogError(logMsg);
        }

    }
}