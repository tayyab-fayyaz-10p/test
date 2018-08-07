using System.Linq;

namespace SSH.Common.Extension
{
    public static class StringExtension
    {
        public static string Stringize(this string[] array, string delimeter)
        {
            if (array == null)
            {
                return string.Empty;
            }

            return array.Aggregate(string.Empty, (current, s) => current + ((current != string.Empty ? delimeter : string.Empty) + s));
        }

        public static char ToUpper(this char character)
        {
            return char.ToUpper(character);
        }

        public static char ToLower(this char character)
        {
            return char.ToLower(character);
        }

        public static string ToCapitalize(this string str)
        {
            string[] strList = str.Split(' ');

            for (int i = 0; i < strList.Length; i++)
            {
                string item = strList[i];
                if (!string.IsNullOrEmpty(item))
                {
                    if (item.Count() > 2)
                    {
                        item = item[0].ToUpper() + item.Substring(1).ToLower();
                    }
                    else
                    {
                        item = item[0].ToUpper().ToString();
                    }

                    strList[i] = item;
                }
            }

            return string.Join(" ", strList);
        }
    }
}
