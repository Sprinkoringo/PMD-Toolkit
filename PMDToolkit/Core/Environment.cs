using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PMDToolkit.Core
{
    public class Environment
    {
        static DateTime compileDate = DateTime.MinValue;

        public static string StartupPath {
            get {
                return System.Reflection.Assembly.GetEntryAssembly().Location;
            }
        }

        public static string StartupDirectory {
            get {
                return System.IO.Path.GetDirectoryName(StartupPath);
            }
        }

        public static DateTime CompileDate {
            get {
                if (compileDate == DateTime.MinValue) {
                    compileDate = RetrieveLinkerTimestamp();
                }
                return compileDate;
            }
        }

        public static bool OnMono {
            get {
                Type t = Type.GetType("Mono.Runtime");
                if (t != null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Retrieves the linker timestamp in UTC.
        /// </summary>
        /// <returns></returns>
        /// <remarks>http://www.codinghorror.com/blog/2005/04/determining-build-date-the-hard-way.html</remarks>
        private static System.DateTime RetrieveLinkerTimestamp() {
            string filePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;

            byte[] peHeader = new byte[2048];
            using (FileStream fileStream = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read)) {
                fileStream.Read(peHeader, 0, 2048);
            }
            DateTime dt = new System.DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(System.BitConverter.ToInt32(peHeader, System.BitConverter.ToInt32(peHeader, peHeaderOffset) + linkerTimestampOffset));
            return dt;
        }
    }
}
