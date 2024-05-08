using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StocksManagement.Tool
{
    public static class Tool
    {
        public static bool IsUpper(string str1)
        {
            // Checking if the input string is equal to its uppercase version OR its lowercase version
            // If the string is either completely uppercase or completely lowercase, it returns true, otherwise false
            char[] chars = str1.ToCharArray();
            foreach (char c in chars)
            {
                if (char.IsLower(c))
                {
                    return false;
                }
            }
            return true;
        }
        public static string FormatDateTo_DDMMYYY(string dateString)
        {
            string[] arrDate = dateString.Split('-');
            string date = "";
            string month = "";
            if (int.Parse(arrDate[0]) < 10 && !arrDate[0].Contains("0"))
            {
                date = "0" + arrDate[0];
            }
            else
            {
                date = arrDate[0];
            }
            if (int.Parse(arrDate[1]) < 10 && !arrDate[1].Contains("0"))
            {
                month = "0" + arrDate[1];
            }
            else
            { month = arrDate[1]; }
            return date + "-" + month + "-" + arrDate[2];
        }
        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        public static List<string> Sort_File_Name_By_Date_DESC(List<string> listFile)
        {

            List<double> listTimeStamp = new List<double>();
            foreach (string fileName in listFile)
            {
                listTimeStamp.Add(Convert_DDMMYYYY_To_Timestamp(fileName.Replace(".xlsx", "")));
            }
            listTimeStamp = listTimeStamp.OrderByDescending(x => x).ToList();
            List<string> listResult = new List<string>();
            foreach (double item in listTimeStamp)
            {
                listResult.Add(Convert_TimeStamp_To_DateString(item) + ".xlsx");
            }
            return listResult;
        }
        public static string RemoveAccents(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }
        public static string titleToKeyID(this string phrase)
        {
            phrase = phrase.Replace("Đ", "d");
            phrase = phrase.Replace("đ", "d");

            // Remove all accents and make the string lower case.  
            string output = phrase.RemoveAccents().ToLower();

            // Remove all special characters from the string.  
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, @"\s+", " ").Trim();

            // Replace all spaces with the hyphen.  
            output = Regex.Replace(output, @"\s", "-");

            // Return the slug.  
            return output;
        }

        public static List<string> GetAllFolderName(string link)
        {
            List<string> listFileName = new List<string>();
            DirectoryInfo d = new DirectoryInfo(link); //Assuming Test is your Folder

            FileInfo[] Files = d.GetFiles("*.*"); //Getting Text files
            var filtered = Files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));


            foreach (FileInfo file in filtered)
            {
                listFileName.Add(file.Name);
            }
            return listFileName;
        }
        public static double Convert_DDMMYYYY_To_Timestamp(string date)
        {
            string test = date;
            DateTime myDate = DateTime.ParseExact(date + " 00:00:00", "dd-MM-yyyy HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
            double timeStamp = ((DateTimeOffset)myDate).ToUnixTimeSeconds();
            return timeStamp * 1000;

        }

        public static double Get_Previous_Month_Date_By_TimeStamp(double timeStamp)
        {
            string date = Convert_TimeStamp_To_DateString(timeStamp);
            string[] arrString = date.Split('-');
            int month = int.Parse(arrString[1]);
            int year = int.Parse(arrString[2]);
            month = month - 1;
            if (month == 0)
            {
                month = 12;
                year = year - 1;
            }
            string monthResult = month.ToString();
            if (month < 10)
            {
                monthResult = "0" + month;
            }
            date = "01" + "-" + monthResult + "-" + year.ToString();
            return Convert_DDMMYYYY_To_Timestamp(date);
        }
        public static string Get_Previous_Month_Date_By_TimeStamp_MMYYY(double timeStamp)
        {
            string date = Convert_TimeStamp_To_DateString(timeStamp);
            string[] arrString = date.Split('-');
            int month = int.Parse(arrString[1]);
            int year = int.Parse(arrString[2]);
            month = month - 1;
            if (month == 0)
            {
                month = 12;
                year = year - 1;
            }
            string monthResult = month.ToString();
            if (month < 10)
            {
                monthResult = "0" + month;
            }
            date = monthResult + "-" + year.ToString();
            return date;
        }

        public static double Get_Next_Month_Date_By_TimeStamp(double timeStamp)
        {
            string date = Convert_TimeStamp_To_DateString(timeStamp);
            string[] arrString = date.Split('-');
            int month = int.Parse(arrString[1]);
            int year = int.Parse(arrString[2]);
            month = month + 1;
            if (month == 13)
            {
                month = 1;
                year = year + 1;
            }
            string monthResult = month.ToString();
            if (month < 10)
            {
                monthResult = "0" + month;
            }
            date = "01" + "-" + monthResult + "-" + year.ToString();
            return Convert_DDMMYYYY_To_Timestamp(date);
        }
        public static string Get_Next_Month_Date_By_TimeStamp_MMYYYY(double timeStamp)
        {
            string date = Convert_TimeStamp_To_DateString(timeStamp);
            string[] arrString = date.Split('-');
            int month = int.Parse(arrString[1]);
            int year = int.Parse(arrString[2]);
            month = month + 1;
            if (month == 13)
            {
                month = 1;
                year = year + 1;
            }
            string monthResult = month.ToString();
            if (month < 10)
            {
                monthResult = "0" + month;
            }
            date = monthResult + "-" + year.ToString();
            return date;
        }

        public static double Get_Previous_Year_TimeStamp(double timestamp)
        {
            string date = Convert_TimeStamp_To_DateString(timestamp);
            string[] arrString = date.Split('-');
            string month = arrString[1];
            int year = int.Parse(arrString[2]);
            year = year - 1;
            date = "01" + "-" + month + "-" + year.ToString();

            return Convert_DDMMYYYY_To_Timestamp(date);
        }


        public static string Get_Previous_Month_Date(string date)
        {
            string[] arrString = date.Split('-');
            int month = int.Parse(arrString[1]);
            int year = int.Parse(arrString[2]);
            month = month - 1;
            if (month == 0)
            {
                month = 12;
                year = year - 1;
            }
            string monthResult = month.ToString();
            if (month < 10)
            {
                monthResult = "0" + month;
            }
            return "01" + "-" + monthResult + "-" + year.ToString();
        }
        public static string Convert_TimeStamp_To_DateString(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp / 1000).ToLocalTime();
            string month = "";
            string day = "";
            if (dateTime.Month < 10)
            {
                month = "0" + dateTime.Month.ToString();
            }
            else
            {
                month = dateTime.Month.ToString();
            }
            if (dateTime.Day < 10)
            {
                day = "0" + dateTime.Day.ToString();
            }
            return day + "-" + month + "-" + dateTime.Year;
        }
        public static int Get_Year_From_TimeStamp(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp / 1000).ToLocalTime();
            return dateTime.Year;
        }

        public static string Get_Month_Year_From_TimeStamp(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp / 1000).ToLocalTime();
            string month = "";
            if (dateTime.Month < 10)
            {
                month = "0" + dateTime.Month.ToString();
            }
            else { month = dateTime.Month.ToString(); }
            return month + "-" + dateTime.Year;
        }
        public static string Get_Quarter_Year_From_TimeStamp(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp / 1000).ToLocalTime();
            string quarter = "";
            if (dateTime.Month >= 1 && dateTime.Month <= 3)
            {
                quarter = "Q1";
            }
            if (dateTime.Month >= 4 && dateTime.Month <= 6)
            {
                quarter = "Q2";
            }
            if (dateTime.Month >= 7 && dateTime.Month <= 9)
            {
                quarter = "Q3";
            }
            if (dateTime.Month >= 10 && dateTime.Month <= 12)
            {
                quarter = "Q4";
            }
            return quarter + "-" + dateTime.Year;
        }

        public static int Get_Month_From_TimeStamp(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp / 1000).ToLocalTime();
            return dateTime.Month;
        }

        public static int GetLastestID(List<dynamic> list)
        {
            if (list.Count == 0) { return 1; }
            if (list.Count == 1) { return 2; }

            list = list.OrderBy(q => q.ID).ToList();

            int maxID = list.Max(t => t.ID);

            for (int i = 1; i <= maxID; i++)
            {
                if (list[i - 1].ID != i)
                {
                    return i;
                }
            }

            return maxID + 1;
        }

        public static double Get_TimeStamp_YoY_Month_From_TimeStamp(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            string currentDate = Convert_TimeStamp_To_DateString(unixTimeStamp);
            string[] arrCurrentDate = currentDate.Split('-');

            string previousYoY = "01-" + arrCurrentDate[1] + "-" + (int.Parse(arrCurrentDate[2]) - 1).ToString();
            return Convert_DDMMYYYY_To_Timestamp(previousYoY);
        }
        public static double Get_Value_By_TimeStamp_From_List(double timeStamp, List<dynamic> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (timeStamp == list[i].TimeStamp)
                {
                    return list[i].Value;
                }
            }
            return double.NaN;
        }
        public static bool Check_Exist_List_Data(double timeStamp, List<dynamic> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (timeStamp == list[i].TimeStamp)
                {
                    return true;
                }
            }
            return false;
        }

        public static double Get_TimeStamp_Previous_Quarter(double timeStamp)
        {
            double result = Get_Previous_Month_Date_By_TimeStamp(timeStamp);
            result = Get_Previous_Month_Date_By_TimeStamp(result);
            result = Get_Previous_Month_Date_By_TimeStamp(result);
            return result;

        }


        public static List<double> MinusList(List<double> listRoot, List<double> listMinus)
        {
            foreach (var item in listMinus)
            {
                listRoot.Remove(item);
            }
            return listRoot;

        }
        public static List<double> ParseToList(List<double> list)
        {
            List<double> result = new List<double>();
            foreach (var item in list)
            {
                result.Add(item);
            }
            return result;
        }
        public static List<List<T>> SplitList<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }

}
