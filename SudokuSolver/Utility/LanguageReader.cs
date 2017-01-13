using System;
using System.IO;
using Newtonsoft.Json;

using SudokuSolver.Settings;
namespace SudokuSolver.Utility
{
    public class LanguageReader
    {
        private static string jsonData;
        public LanguageReader(string data)
        {
            jsonData = data;
        }

        public static string GetText(string data, Language lang)
        {
            dynamic json = JsonConvert.DeserializeObject(jsonData);

            try
            {
                return json[data][lang.ToString()];
            }
            catch
            {
                return "Error GetText();";
            }
        }
    }
}