// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CandyCoded.env
{

    public static class env
    {

        public const string filename = "env";

        public static readonly string editorFilePath = Path.Combine(Application.dataPath, "../", $".{filename}");

        public static readonly string resourcesDirPath = Path.Combine(Application.dataPath, "Resources");

        public static readonly string runtimeFilePath = Path.Combine(resourcesDirPath, $"{filename}.txt");

        private static Dictionary<string, string> _variables = new();

        public static Dictionary<string, string> variables
        {
            get
            {
                if (_variables.Count == 0)
                {
                    _variables = ParseEnvironmentFile();
                }

                return _variables;
            }
        }

        public static Dictionary<string, string> ParseEnvironmentFile(string contents)
        {

            return contents.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Where(l =>
                    !string.IsNullOrWhiteSpace(l) && !l.StartsWith("#") &&
                    l.IndexOf("=", StringComparison.Ordinal) != -1)
                .ToDictionary(l => l.Substring(0, l.IndexOf("=", StringComparison.Ordinal)).Trim(),
                    l => l.Substring(l.IndexOf("=", StringComparison.Ordinal) + 1).Trim().Trim('"', '\''));

        }

        public static string SerializeEnvironmentDictionary(Dictionary<string, string> dictionary)
        {

            if (dictionary.Any(item => string.IsNullOrEmpty(item.Key)))
            {

                throw new Exception("One or more keys are missing! Please fix and try again.");

            }

            return string.Join(Environment.NewLine,
                dictionary.Select(item => $"{item.Key}={item.Value}"));

        }

        public static Dictionary<string, string> ParseEnvironmentFile()
        {

#if UNITY_EDITOR
            if (File.Exists(editorFilePath))
            {
                return ParseEnvironmentFile(File.ReadAllText(editorFilePath, Encoding.UTF8));
            }
#else
            var contents = Resources.Load<TextAsset>(filename);

            if (contents)
            {
                return ParseEnvironmentFile(contents.text);
            }
#endif
            return new Dictionary<string, string>();

        }

        public static bool TryParseEnvironmentVariable(string key, out string value)
        {

            return variables.TryGetValue(key, out value);

        }

        public static bool TryParseEnvironmentVariable(string key, out bool value)
        {

            return bool.TryParse(variables[key], out value);

        }

        public static bool TryParseEnvironmentVariable(string key, out double value)
        {

            return double.TryParse(variables[key], out value);

        }

        public static bool TryParseEnvironmentVariable(string key, out float value)
        {

            return float.TryParse(variables[key], out value);

        }

        public static bool TryParseEnvironmentVariable(string key, out int value)
        {

            return int.TryParse(variables[key], out value);

        }

    }

}
