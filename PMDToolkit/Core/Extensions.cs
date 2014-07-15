using System;
using System.Collections.Generic;
using System.Text;

namespace PMDToolkit.Core
{
    public static class Extensions
    {
        public static byte ToByte(this string str) {
            byte result = 0;
            if (!string.IsNullOrEmpty(str) && byte.TryParse(str, out result)) {
                return result;
            } else
                return 0;
        }

        public static byte ToByte(this string str, byte defaultVal) {
            byte result = 0;
            if (str != null && byte.TryParse(str, out result) == true) {
                return result;
            } else
                return defaultVal;
        }

        public static int ToInt(this string str) {
            int result = 0;
            if (!string.IsNullOrEmpty(str) && int.TryParse(str, out result)) {
                return result;
            } else
                return 0;
        }

        public static int ToInt(this string str, int defaultVal) {
            int result = 0;
            if (str != null && int.TryParse(str, out result) == true) {
                return result;
            } else
                return defaultVal;
        }

        public static double ToDbl(this string str) {
            double result = 0;
            if (str != null && double.TryParse(str, out result) == true) {
                return result;
            } else
                return 0;
        }

        public static double ToDbl(this string str, double defaultVal) {
            double result = 0;
            if (str != null && double.TryParse(str, out result) == true) {
                return result;
            } else
                return defaultVal;
        }

        public static string ToIntString(this bool boolval) {
            if (boolval == true)
                return "1";
            else
                return "0";
        }

        public static int ToInt(this bool boolval) {
            if (boolval == true)
                return 1;
            else
                return 0;
        }

        public static bool IsNumeric(this string str) {
            int result;
            return int.TryParse(str, out result);
        }

        public static ulong ToUlng(this string str) {
            ulong result = 0;
            if (ulong.TryParse(str, out result) == true) {
                return result;
            } else
                return 0;
        }

        public static bool ToBool(this string str) {
            if (string.IsNullOrEmpty(str)) {
                return false;
            }
            switch (str.ToLower()) {
                case "true":
                    return true;
                case "false":
                    return false;
                case "1":
                    return true;
                case "0":
                    return false;
                default:
                    return false;
            }
        }

        public static bool ToBool(this string str, bool defaultValue) {
            if (string.IsNullOrEmpty(str)) {
                return false;
            }
            switch (str.ToLower()) {
                case "true":
                    return true;
                case "false":
                    return false;
                case "1":
                    return true;
                case "0":
                    return false;
                default:
                    return defaultValue;
            }
        }

        public static bool IsEnum<T>(this string enumString)
        {
            return Enum.IsDefined(typeof(T), enumString);
        }

        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString, true);
        }

        public static DateTime? ToDate(this string date) {
            DateTime tmpDate;
            if (DateTime.TryParse(date, out tmpDate)) {
                return tmpDate;
            } else {
                return null;
            }
        }

        public static string ToHex(this System.Drawing.Color color) {
            return String.Format("#{0:x2}{1:x2}{2:x2}{3:x2}", color.A, color.R, color.G, color.B);
        }

        public static System.Drawing.Color ToColor(this string hexString) {
            return HexStringToColor(hexString);
        }

        private static System.Drawing.Color HexStringToColor(string hex) {
            hex = hex.Replace("#", "");

            if (hex.Length != 8)
                throw new Exception(hex +
                    " is not a valid 8-place hexadecimal color code.");

            string a, r, g, b;
            a = hex.Substring(0, 2);
            r = hex.Substring(2, 2);
            g = hex.Substring(4, 2);
            b = hex.Substring(6, 2);

            return System.Drawing.Color.FromArgb(HexStringToBase10Int(a),
                                                 HexStringToBase10Int(r),
                                                 HexStringToBase10Int(g),
                                                 HexStringToBase10Int(b));
        }

        private static int HexStringToBase10Int(string hex) {
            int base10value = 0;

            try { base10value = System.Convert.ToInt32(hex, 16); } catch { base10value = 0; }

            return base10value;
        }
    }
}
