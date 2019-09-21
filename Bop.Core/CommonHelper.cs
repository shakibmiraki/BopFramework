using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Bop.Core.Infrastructure;
using MD.PersianDateTime.Standard;
using Microsoft.AspNetCore.Hosting;

namespace Bop.Core
{
    public partial class CommonHelper
    {

        #region Fields

        private const string PhoneExpression = @"^(09)[13][0-9]\d{7}$";
        private const string EmailExpression = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";
        private static readonly Regex PhoneRegex;
        private static readonly Regex EmailRegex;

        #endregion

        #region Ctor

        static CommonHelper()
        {
            PhoneRegex = new Regex(PhoneExpression, RegexOptions.IgnoreCase);
            EmailRegex = new Regex(EmailExpression, RegexOptions.IgnoreCase);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default file provider
        /// </summary>
        public static IBopFileProvider DefaultFileProvider { get; set; }

        #endregion


        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value == null)
                return null;

            var sourceType = value.GetType();

            var destinationConverter = TypeDescriptor.GetConverter(destinationType);
            if (destinationConverter.CanConvertFrom(value.GetType()))
                return destinationConverter.ConvertFrom(null, culture, value);

            var sourceConverter = TypeDescriptor.GetConverter(sourceType);
            if (sourceConverter.CanConvertTo(destinationType))
                return sourceConverter.ConvertTo(null, culture, value, destinationType);

            if (destinationType.IsEnum && value is int)
                return Enum.ToObject(destinationType, (int)value);

            if (!destinationType.IsInstanceOfType(value))
                return Convert.ChangeType(value, destinationType, culture);

            return value;
        }

        /// <summary>
        /// Verifies that string is an valid IP-Address
        /// </summary>
        /// <param name="ipAddress">IPAddress to verify</param>
        /// <returns>true if the string is a valid IpAddress and false if it's not</returns>
        public static bool IsValidIpAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out IPAddress _);
        }

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.Trim();

            return EmailRegex.IsMatch(email);
        }

        /// <summary>
        /// Verifies that a string is in valid phone format
        /// </summary>
        /// <param name="phone">Phone to verify</param>
        /// <returns>true if the string is a valid phone address and false if it's not</returns>
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;

            phone = phone.Trim();

            return PhoneRegex.IsMatch(phone);
        }

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            var result = string.Empty;
            foreach (var c in str)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c;
                else
                    result += c.ToString();

            //ensure no spaces (e.g. when the first letter is upper case)
            result = result.TrimStart();
            return result;
        }


        /// <summary>
        /// Returns an random integer number within a specified rage
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            var str = string.Empty;
            for (var i = 0; i < length; i++)
                str = string.Concat(str, random.Next(10).ToString());
            return str;
        }


        /// <summary>
        /// Generate sms verification code
        /// </summary>
        /// <returns>Result string</returns>
        public static string GenerateSmsVerificationCode()
        {
            return GenerateRandomDigitCode(5);
        }


        /// <summary>
        /// Convert jalali date to zeipt dat format
        /// </summary>
        /// <param name="jalaliDate">YYYYMM</param>
        /// <returns>MM/YYYY</returns>
        public static string NormalizeDateForZeipt(string jalaliDate)
        {
            try
            {
                if (string.IsNullOrEmpty(jalaliDate))
                {
                    throw new ArgumentNullException("jalali_date_null_error");
                }
                var normalizedShamsiDate = NormalizeJalaliDate(jalaliDate);
                var jalaliDateTime = PersianDateTime.Parse(normalizedShamsiDate);
                jalaliDateTime.EnglishNumber = true;

                var zeiptDateFormat = jalaliDateTime.ToDateTime().ToString("MM/yyyy");
                return zeiptDateFormat;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// convert shamsi date(YYYYMM) to another shamsi date format (YYYYMMDD)
        /// </summary>
        /// <param name="jalaliDate">YYYYMM</param>
        /// <returns>YYYYMMDD</returns>
        public static int NormalizeJalaliDate(string jalaliDate)
        {
            if (string.IsNullOrEmpty(jalaliDate))
            {
                throw new ArgumentNullException("jalali_date_null_error");
            }
            if (jalaliDate.Trim().Length != 6)
            {
                throw new FormatException("jalali_date_format_error");
            }

            return int.Parse($"{jalaliDate}01");

        }

        public static void ValidateHostEnvironment(IHostingEnvironment hostingEnvironment)
        {
            if (string.IsNullOrWhiteSpace(hostingEnvironment.WebRootPath))
            {
                hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
        }
    }
}
