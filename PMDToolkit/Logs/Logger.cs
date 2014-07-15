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
