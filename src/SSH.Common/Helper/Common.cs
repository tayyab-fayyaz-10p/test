using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SSH.Common.Helper
{
    public class CommonHelper
    {
        private static readonly Regex patternWhitespace = new Regex(@"\s+");

        private static readonly Regex patternSymbols = new Regex(@"\W|_");

        private static readonly Regex patternUrl = new Regex("^(?:(?:(?:https?):)?\\/\\/)(?:\\S+(?::\\S*)?@)?(?:(?!(?:10|127)(?:\\.\\d{1,3}){3})(?!(?:169\\.254|192\\.168)(?:\\.\\d{1,3}){2})(?!172\\.(?:1[6-9]|2\\d|3[0-1])(?:\\.\\d{1,3}){2})(?:[1-9]\\d?|1\\d\\d|2[01]\\d|22[0-3])(?:\\.(?:1?\\d{1,2}|2[0-4]\\d|25[0-5])){2}(?:\\.(?:[1-9]\\d?|1\\d\\d|2[0-4]\\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\\.(?:[a-z\u00a1-\uffff]{2,})))(?::\\d{2,5})?(?:[/?#]\\S*)?$");

        public static readonly string DriverIdPrefix = "FL-";

        private static readonly string regexRemoveSpecialCharacter= @"[^a-zA-Z0-9_.@\s]+";

        public string GenerateOTP()
        {
            Random random = new Random();
            int randomOtp = random.Next(1000, 9999);
            return randomOtp.ToString();
            //return "1234";
        }

        public static DateTime DateParse(string date)
        {
            return DateTime.Parse(date);
        }
        public string GetGMapBase64(string latitude, string longitude, string gmapUrl)
        {
            string latlng = latitude + "," + longitude;

            string url = string.Format(gmapUrl, latlng, latlng);

            WebClient wc = new WebClient();

            byte[] buffer = wc.DownloadData(url);

            return Convert.ToBase64String(buffer);
        }

        public string GetGoogleMapURL(string latitude, string longitude, string gmapUrl)
        {
            string latlng = latitude + "," + longitude;

            return string.Format(gmapUrl, latlng, latlng);
        }

        public static string BaseAddress()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        }

        public static string ReplaceWhitespace(string input, string replacement)
        {
            return patternWhitespace.Replace(input, replacement);
        }

        public static bool isValidAWB(string input)
        {
            return !patternSymbols.IsMatch(input);
        }

        public static bool isValidURL(string input)
        {            
            return patternUrl.IsMatch(input);
        }

        public static string[] ReplaceSpecialCharacterAndSplit(string searchTerm)
        {
            return Regex.Replace(searchTerm, regexRemoveSpecialCharacter, " ", RegexOptions.Compiled).Split(' ');
        }
    }
}
