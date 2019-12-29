using GutenbergProjectVBS.Web.Models;
using GutenbergProjectVBS.Web.Models.Context;
using GutenbergProjectVBS.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GutenbergProjectVBS.Web.GVBSHelpers
{
    public static class GVBSHelperClass
    {
        private static IDictionary<string, string> languageCodes = new Dictionary<string, string>() {
            { "zh", "Chinese" },
            { "da", "Danish" },
            { "nl", "Dutch" },
            { "en", "English" },
            { "eo", "Esperanto" },
            { "fi", "Finnish" },
            { "fr", "French" },
            { "de", "German" },
            { "el", "Greek" },
            { "hu", "Hungarian" },
            { "it", "Italian" },
            { "la", "Latin" },
            { "pt", "Portuguese" },
            { "es", "Spanish" },
            { "sv", "Swedish" },
            { "tl", "Tagalog" },
            { "af", "Afrikaans" },
            { "ale", "Aleut" },
            { "ar", "Arabic" },
            { "arp", "Arapaho" },
            { "brx", "Bodo" },
            { "br", "Breton" },
            { "bg", "Bulgarian" },
            { "rmr", "Caló" },
            { "ca", "Catalan" },
            { "ceb", "Cebuano" },
            { "cs", "Czech" },
            { "et", "Estonian" },
            { "fa", "Farsi" },
            { "fy", "Frisian" },
            { "fur", "Friulian" },
            { "gla", "Gaelic, Scottish" },
            { "gl", "Galician" },
            { "kld", "Gamilaraay" },
            { "grc", "Greek, Ancient" },
            { "he", "Hebrew" },
            { "is", "Icelandic" },
            { "ilo", "Iloko" },
            { "ia", "Interlingua" },
            { "iu", "Inuktitut" },
            { "ga", "Irish" },
            { "ja", "Japanese" },
            { "csb", "Kashubian" },
            { "kha", "Khasi" },
            { "ko", "Korean" },
            { "lt", "Lithuanian" },
            { "mi", "Maori" },
            { "myn", "Mayan Languages" },
            { "enm", "Middle English" },
            { "nah", "Nahuatl" },
            { "nap", "Napoletano-Calabrese" },
            { "nav", "Navajo" },
            { "nai", "North American Indian" },
            { "no", "Norwegian" },
            { "oc", "Occitan" },
            { "oji", "Ojibwa" },
            { "ang", "Old English" },
            { "pl", "Polish" },
            { "ro", "Romanian" },
            { "ru", "Russian" },
            { "sa", "Sanskrit" },
            { "sr", "Serbian" },
            { "sl", "Slovenian" },
            { "bgs", "Tagabawa" },
            { "te", "Telugu" },
            { "cy", "Welsh" },
            { "yi", "Yiddish" }
        };

        /// <summary>
        /// remote file exists url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool RemoteFileExistsUrl(string url)
        {
            bool result = false;

            using (WebClient client = new WebClient())
            {
                try
                {
                    Stream stream = client.OpenRead(url);
                    if (stream != null)
                        result = true;
                    else
                        result = false;
                } catch(Exception ex)
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert Language
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ConvertLanguage(string code)
        {
            return languageCodes[code];
        }

        /// <summary>
        /// Generate MD5 password
        /// </summary>
        /// <returns></returns>
        public static string CreateMD5Password(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] originalBytes = encoder.GetBytes(password);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            password = BitConverter.ToString(encodedBytes).Replace("-", "");
            return password.ToLower();
        }

        /// <summary>
        /// Send Verification Email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="activationCode"></param>
        /// <param name="emailFor"></param>
        [NonAction]
        public static void SendVerificationLinkEmail(string email, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "Account/" + emailFor + "/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var link = "https://localhost:44329/" + verifyUrl;

            var fromEmail = new MailAddress("backenddeveloper35@gmail.com", "Gutenberg VBS - Forgot Password");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "emir@gutenberg+9393"; // Replace with actual password

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Gutenberg VBS account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }

            try
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };

                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })

                smtp.Send(message);
            } catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("forgot password mail error");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine("forgot password mail error");
            }            
        }

        public static User GetUser(LoginViewModel model)
        {
            using (VBSContext db = new VBSContext())
            {
                return db.Users.AsQueryable().FirstOrDefault(u => u.Email == model.Email && u.Password.Equals(model.Password) && u.IsAdmin == false);
            }
        }
    }
}