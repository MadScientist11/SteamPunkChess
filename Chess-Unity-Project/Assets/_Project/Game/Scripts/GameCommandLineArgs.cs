using System;
using System.Collections.Generic;
using System.Linq;

namespace SteampunkChess
{
    public static class GameCommandLineArgs
    {
        private const string ArgPrefix = "--";

        private static IReadOnlyList<string> _gameArgs;

        public static IReadOnlyList<string> GameArgs
        {
            get { return _gameArgs; }
        }

        static GameCommandLineArgs()
        {
            _gameArgs = ParseCustomArguments(Environment.GetCommandLineArgs()).ToList();
        }

        public static bool Contains(string arg)
        {
            return GameArgs.Contains(arg);
        }

        public static IEnumerable<string> ParseCustomArguments(string[] args)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < args.Length; i++)
            {
                if (!TryParseArgument(args[i], out string result))
                    continue;

                yield return result;
            }
        }

        private static bool TryParseArgument(string arg, out string result)
        {
            result = string.Empty;

            if (string.IsNullOrWhiteSpace(arg))
                return false;

            string semiParsedArg = arg.Trim().ToLower();

            if (!semiParsedArg.StartsWith(ArgPrefix))
                return false;

            result = semiParsedArg.Substring(2);

            return true;
        }
    }
}