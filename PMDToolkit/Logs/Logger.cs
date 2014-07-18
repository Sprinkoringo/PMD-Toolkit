/*The MIT License (MIT)

Copyright (c) 2014 PMU Staff

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace PMDToolkit.Logs {
    public static class Logger
    {
        static object lockObj = new object();

        private static List<string> battleLog;
        private static DateTime journeyStart;

        public static XmlWriterSettings XmlWriterSettings { get; private set; }

        public static void Init() {
            battleLog = new List<string>();

            XmlWriterSettings = new System.Xml.XmlWriterSettings();
            XmlWriterSettings.OmitXmlDeclaration = false;
            XmlWriterSettings.IndentChars = "  ";
            XmlWriterSettings.Indent = true;
            XmlWriterSettings.NewLineChars = Environment.NewLine;
            
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");
#if GAME_MODE
            if (!Directory.Exists("Logs/Journey"))
                Directory.CreateDirectory("Logs/Journey");
#endif
            if (!Directory.Exists("Logs/Error"))
                Directory.CreateDirectory("Logs/Error");
        }

        public static void LogBattle(string msg) {
            battleLog.Add(msg);
        }

        public static List<string> GetRecentBattleLog(int entries) {
            if (entries < 0 || entries > battleLog.Count) {
                entries = battleLog.Count;
            }
            List<string> returnLog = new List<string>();
            for (int i = 0; i < entries; i++) {
                returnLog.Insert(0, battleLog[battleLog.Count - i - 1]);
            }
            return returnLog;
        }

        public static void BeginJourney(int seed) {
#if GAME_MODE
            journeyStart = DateTime.Now;
            try {
                string date = DateTime.Now.ToShortTimeString();
                string filePath = "Logs/Journey/" + journeyStart.ToString().Replace("/", "-").Replace(":", "-") + ".txt";
                using (StreamWriter writer = new StreamWriter(filePath, true)) {
                    writer.WriteLine(seed);
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.Write(ex.ToString());
            }
#endif
        }

        public static void LogJourney(Logic.Gameplay.Command command) {
#if GAME_MODE
            try {
                string date = DateTime.Now.ToShortTimeString();
                string filePath = "Logs/Journey/" + journeyStart.ToString().Replace("/", "-").Replace(":", "-") + ".txt";
                using (StreamWriter writer = new StreamWriter(filePath, true)) {
                    writer.Write((int)command.Type);
                    for (int i = 0; i < command.ArgCount; i++) {
                        writer.Write(' ');
                        writer.Write(command[i]);
                    }
                    writer.WriteLine();
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.Write(ex.ToString());
            }
#endif
        }

        public static void LogDebug(string msg) {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(msg);
#endif
        }

        public static void LogError(Exception exception)
        {

            lock (lockObj)
            {
                LogBattle(exception.Message);

                try
                {
                    string date = DateTime.Now.ToShortTimeString();
                    string filePath = "Logs/Error/" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".txt";
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("--- " + DateTime.Now.ToLongTimeString() + " ---");
                        writer.WriteLine("Exception: " + exception.ToString());
                        writer.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.ToString());
                }
            }
        }

        public static void LogInfo(string diagInfo)
        {

            lock (lockObj)
            {

                try
                {
                    string date = DateTime.Now.ToShortTimeString();
                    string filePath = "Logs/Error/" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".txt";
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine(diagInfo);
                        writer.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.ToString());
                }
            }
        }
    }
}
