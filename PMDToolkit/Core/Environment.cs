/*The MIT License (MIT)

Copyright (c) 2014 Sprinkoringo

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
