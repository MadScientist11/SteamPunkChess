using System;
using System.Collections.Generic;
using System.Linq;

namespace SteampunkChess
{
    public static class GameCommandLineArgs
    {
        private const string ArgPrefix = "--";

        private static IReadOnlyList<string> _gameArgs;

        private static IReadOnlyList<string> GameArgs
        {
            get
            {
                if(_gameArgs == null)
                {
                   _gameArgs = ParseCustomArguments(Environment.GetCommandLineArgs()).ToList();
                }

                return _gameArgs;
            }
        }

        public static bool Contains(string arg)
        {
            return GameArgs.Contains(arg);
        }

        private static IEnumerable<string> ParseCustomArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (!TryParseArgument(args[i], out string result))
                    continue;

                yield return result;

            }
        }

        private static bool TryParseArgument(string arg, out string result)
        {
            result = String.Empty;

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