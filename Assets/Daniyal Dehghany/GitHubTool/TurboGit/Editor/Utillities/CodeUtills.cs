using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

namespace Daniyal_Dehghany_GitHubTool_TurboGit.Utillities
{
    public class CodeUtills
    {
        /// <summary>
        /// IT allows you to easily draw a texture using parameters
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="texture2D"></param>
        /// <param name="scaleMode"></param>
        public static void OnDrawTextur(Rect rect, Texture2D texture2D, ScaleMode scaleMode)
            => GUI.DrawTexture(rect, texture2D, scaleMode);

        /// <summary>
        /// Gets the current date and time of the EU region
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentDateTime()
        {
            TimeZoneInfo euTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime euTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, euTimeZone);

            return euTime;
        }

        /// <summary>
        /// Encodes given string to Base64 format.
        /// </summary>
        /// <param name="text">Plain text which should be Base64 encoded.</param>
        /// <returns>Encoded Base64 string.</returns>
        public static string Base64Encode(string textToBase64)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(textToBase64);

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Decodes given Base64 formatted string back to an string.
        /// </summary>
        /// <param name="encodeText">Base64 encoded string which should be decoded.</param>
        /// <returns>Decoded string.</returns>
        public static string Base64Decode(string encodeBase64Text)
        {
            byte[] bytes = Convert.FromBase64String(encodeBase64Text);

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// It will give you star signs (*) for each password characters --> if it hasnt to be shown
        /// </summary>
        /// <param name="passwordText"></param>
        /// <returns></returns>
        public static string PasswordAsStarsString(string passwordText)
        {
            string passAsStars = string.Empty;
            for (int i = 0; i < passwordText.Length; i++)
            {
                passAsStars += "*";
            }

            return passAsStars;
        }

        /// <summary>
        /// Set the password text as a MD5 Hash text
        /// The problem with this form is: you cannt turn the password text as MD5 back to standard
        /// Also its not readable anyway!
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
