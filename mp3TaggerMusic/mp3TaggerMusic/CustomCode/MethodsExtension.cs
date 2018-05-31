using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace mp3TaggerMusic.CustomCode
{
    public static class MethodsExtension
    {
        /// <summary>
        /// Devuelve si true si el string esta null o vacio o false si no lo esta
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool MyStringIsNullOrEmpty(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }

            return false;
        }
        
        public static string FormatDatetime(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static int ToInt(this object st)
        {
            int i = 0;

            if (int.TryParse(st.ToString(), out i))
            {
                return i;
            }
            else
            {
                return 0;
            }
        }

        public static int? ToIntNullable(this object st)
        {
            int i = 0;

            if (int.TryParse(st.ToString(), out i))
            {
                if (i > 0)
                {
                    return i;
                }
                else { return null; }
            }
            else
            {
                return null;
            }
        }

        //public static DateTime ToDatetime(this string st)
        //{
        //    DateTime i = DateTime.Now;
        //    CultureInfo ci = CultureInfo.CreateSpecificCulture("es-DO");

        //    if (DateTime.TryParse(st, ci, DateTimeStyles.None, out i))
        //    {
        //        return i;
        //    }
        //    else
        //    {
        //        return DateTime.Now;
        //    }
        //}
        
        public static DateTime? ToDatetimeNullable(this string st)
        {
            DateTime i = DateTime.Now;
            //CultureInfo ci = CultureInfo.CreateSpecificCulture("es-DO");

            if (DateTime.TryParse(st, out i))
            {
                return i;
            }
            else
            {
                return null;
            }
        }
        
        public static decimal ToDecimal(this string st)
        {
            decimal i = 0;

            if (decimal.TryParse(st, out i))
            {
                return i;
            }
            else
            {
                return 0;
            }
        }

        public static decimal? ToDecimalNullable(this string st)
        {
            decimal i = 0;

            if (decimal.TryParse(st, out i))
            {
                return i;
            }
            else
            {
                return null;
            }
        }

        public static string UppercaseFirstLetter(this string st)
        {
            if (string.IsNullOrEmpty(st))
            {
                return string.Empty;
            }
            char[] a = st.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        //public static string UppercaseFirstLetterOfWords(this string st)
        //{
        //    if (string.IsNullOrEmpty(st))
        //    {
        //        return string.Empty;
        //    }
        //    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        //    return textInfo.ToTitleCase(st.ToLower());
        //}
        
        public static string ToJson(this object obj)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetTreeViewJsonFormat(this string str) { return '[' + str + ']'; }

        public static string MergeJsonString(this string str, string strTwo) { if (String.IsNullOrEmpty(str)) return strTwo; else return str + "," + strTwo; }

        public static string GetInnerExceptionMessage(this Exception ex)
        {
            if (ex != null && ex.InnerException != null)
            {
                return ex.InnerException.ToString();
            }
            return "N/A";

        }

        public static decimal NormalizeDecimal(this decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }

        public static uint ToUInt(this object st)
        {
            uint i = 0;

            if (uint.TryParse(st.ToString(), out i))
            {
                return i;
            }
            else
            {
                return 0;
            }
        }
    }
}