using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace MOCDIntegrations.Models
{
    public class IEUtils
    {
        public static string DataTableToJSONWithJavaScriptSerializer(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }


        public static String SafeSQLString(String str)
        {
            if (str != null)
            {
                return "'" + str.Trim().Replace("'", "''") + "'";
            }
            else
            {
                return "''";
            }
        }

        public static String SearchSafeSQLString(String str)
        {
            return str.Replace("'", "''").Trim();
        }

        public static String SafeSQLDate(DateTime d)
        {
            return "'" + d.ToString("G") + "'";
        }

        public static DateTime ToDate(object o, DateTime defaults)
        {
            if (o == null) return defaults;
            if (o.GetType().Equals(typeof(System.DBNull))) return defaults;
            try
            {
                if (o.GetType().Equals(typeof(System.String)))
                {
                    return Convert.ToDateTime(o.ToString());
                }
                return DateTime.Parse(o.ToString());
            }
            catch
            {
                return defaults;
            }
        }

        public static DateTime? ToDateNullable(object o, DateTime defaults)
        {
            if (o == null) return defaults;
            if (o.GetType().Equals(typeof(System.DBNull))) return defaults;
            try
            {
                if (o.GetType().Equals(typeof(System.String)))
                {
                    return Convert.ToDateTime(o.ToString());
                }
                return (DateTime?)DateTime.Parse(o.ToString());
            }
            catch
            {
                return defaults;
            }
        }

        public static DateTime? ToDateNullable(object o)
        {
            return ToDateNullable(o, new DateTime(1, 1, 1));
        }

        public static DateTime ToDate(object o)
        {
            return ToDate(o, new DateTime(1, 1, 1));
        }

        public static Boolean ToBoolean(object o)
        {
            if (o == null) return false;
            if (o.GetType().Equals(typeof(System.DBNull))) return false;
            if (o.GetType().Equals(typeof(System.Boolean))) return (bool)o;
            if (o.GetType().Equals(typeof(System.String))) return (o.ToString().ToLower().Equals("true"));
            return (ToInt(o) != 0);
        }

        static public Double ToDouble(object o)
        {
            if (o == null)
                return (System.Double)0;
            if (o.GetType().Equals(typeof(System.DBNull)))
                return (System.Double)0;
            if (o.GetType().Equals(typeof(System.Double)))
                return (System.Double)o;
            if (o.GetType().Equals(typeof(System.Single)))
                return (System.Double)(System.Single)o;
            else if (o.GetType().Equals(typeof(System.Decimal)))
                return (System.Double)(System.Decimal)o;
            else if (o.GetType().Equals(typeof(System.Int64)))
                return Double.Parse(o.ToString());
            else if (o.GetType().Equals(typeof(System.Int32)))
                return Double.Parse(o.ToString());
            else if (o.GetType().Equals(typeof(System.Int16)))
                return Double.Parse(o.ToString());
            else if (o.GetType().Equals(typeof(System.String)))
            {
                try
                {
                    return Double.Parse(o.ToString());
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return (System.Double)o;
            }
        }

        public static Decimal ToDecimal(object o)
        {
            return ToDecimal(o, (System.Decimal)0);
        }

        public static Decimal ToDecimal(object o, decimal defaults)
        {
            //if (o == null) return defaults;
            //if (o.GetType().Equals(typeof(System.DBNull))) return defaults;

            //if (o.GetType().Equals(typeof(System.Single)))
            //    return (System.Decimal)(System.Single)o;

            //if (o.GetType().Equals(typeof(System.Double)))
            //    return (System.Decimal)(System.Double)o;
            //try
            //{
            //    return Decimal.Parse(o.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowTrailingSign | System.Globalization.NumberStyles.AllowLeadingSign);
            //}
            //catch
            //{
            //    return defaults;
            //}
            try
            {
                return Decimal.Parse(o.ToString());
                //return Convert.ToDecimal(o);
            }
            catch
            {
                return defaults;
            }
        }

        static public Int64 ToInt64(object o)
        {
            return ToInt64(o, 0);
        }

        static public int ToInt(object o)
        {
            return ToInt(o, 0);
        }

        static public Int64 ToInt64(object o, Int64 defaultInt)
        {
            if (o == null)
                return defaultInt;


            if (o.GetType().Equals(typeof(System.Int32)))
                return (Int64)o;

            try
            {
                if (o.GetType().Equals(typeof(System.String)))
                    return Int64.Parse(o.ToString());
            }
            catch
            {
                return defaultInt;
            }


            if (o.GetType().Equals(typeof(System.Int64)))
                return (Int64)o;

            if (o.GetType().Equals(typeof(System.Decimal)))
                return System.Decimal.ToInt64((System.Decimal)o);

            if (o.GetType().Equals(typeof(System.Byte)))
                return (Int64)((Byte)o);

            if (o.GetType().Equals(typeof(System.DBNull)))
                return defaultInt;

            if (o.GetType().Equals(typeof(double)))
                return (Int64)((double)o);
            try
            {
                return Convert.ToInt64(o.ToString());
            }
#pragma warning disable CS0168 // The variable 'exp' is declared but never used
            catch (Exception exp)
#pragma warning restore CS0168 // The variable 'exp' is declared but never used
            {
                return 0;
            }

#pragma warning disable CS0162 // Unreachable code detected
            return 0;
#pragma warning restore CS0162 // Unreachable code detected

        }

        static public int ToInt(object o, int defaultInt)
        {
            if (o == null)
                return defaultInt;

            if (o.GetType().Equals(typeof(System.Boolean)))
            {
                if (((bool)o) == true)
                    return 1;
                else
                    return 0;
            }

            if (o.GetType().Equals(typeof(System.Int32)))
                return (int)o;

            try
            {
                if (o.GetType().Equals(typeof(System.String)))
                    return (int)Int32.Parse(o.ToString());
            }
            catch
            {
                return defaultInt;
            }

            if (o.GetType().Equals(typeof(System.Int16)))
                return (Int16)o;

            if (o.GetType().Equals(typeof(System.Decimal)))
                return System.Decimal.ToInt32((System.Decimal)o);

            if (o.GetType().Equals(typeof(System.Byte)))
                return (int)((Byte)o);

            if (o.GetType().Equals(typeof(System.DBNull)))
                return defaultInt;

            if (o.GetType().Equals(typeof(double)))
                return (int)((double)o);
            try
            {
                return Convert.ToInt32(o.ToString());
            }
#pragma warning disable CS0168 // The variable 'exp' is declared but never used
            catch (Exception exp)
#pragma warning restore CS0168 // The variable 'exp' is declared but never used
            {
                return 0;
            }
#pragma warning disable CS0162 // Unreachable code detected
            return 0;
#pragma warning restore CS0162 // Unreachable code detected
        }

        public static string ToString(object o)
        {
            if (o == null) return "";
            if (o.GetType().Equals(typeof(System.DBNull)))
                return "";
            return o.ToString();
        }

        public static DataTable ParseCSV(string inputString)
        {

            DataTable dt = new DataTable();

            // declare the Regular Expression that will match versus the input string |(?<field>[^\\s])
            //Regex re = new Regex("((?<field>[^\",\\r\\n]+)|\"(?<field>([^\"]|[^:blank:]|\"\")+)\")(,|(?<rowbreak>\\r\\n|\\n|$))");
            //str.replace( /['"](?!\b)/g , "").replace( /([^\w])['"]/g, "$1" ).replace( /^['"]|['"]$/g, "" );

            Regex re = new Regex("[^,]*,{0,1}");

            ArrayList colArray = new ArrayList();
            ArrayList rowArray = new ArrayList();

            int colCount = 0;
            int maxColCount = 0;
            int chkRow = 1;
            MatchCollection mc;
            String[] inputArr = inputString.Split("\r\n".ToCharArray());
            foreach (String InputStr in inputArr)
            {
                if (chkRow == 1)
                {
                    chkRow = 1;
                }
                if (InputStr.Length > 0)
                {
                    mc = re.Matches(InputStr);
                    String fieldVal = "";
                    bool QouteStarted = false;
                    foreach (Match m in mc)
                    {
                        if (m.Value.Length > 0)
                        {
                            if (m.Value.StartsWith("\""))
                            {
                                fieldVal = m.Value.Substring(1);
                                QouteStarted = true;
                            }
                            else
                            {
                                if (QouteStarted)
                                {
                                    if (m.Value.EndsWith("\","))
                                    {
                                        QouteStarted = false;
                                        fieldVal += m.Value.Substring(0, m.Value.Length - 2);
                                    }
                                    else
                                    {
                                        fieldVal += m.Value;
                                    }
                                }
                                else
                                {//Check if not ends with comma

                                    if (m.Value.EndsWith(","))
                                    {
                                        fieldVal = m.Value.Substring(0, m.Value.Length - 1);
                                    }
                                    else
                                    {
                                        fieldVal = m.Value;
                                    }
                                }
                            }
                            if (!QouteStarted)
                            {
                                //Field Values
                                if (chkRow == 1)
                                {
                                    colArray.Add(fieldVal);
                                }
                                else
                                {
                                    rowArray.Add(fieldVal);
                                }
                                colCount++;
                            }
                        }
                        if ((chkRow == 1) && (colCount > maxColCount))
                        {
                            dt.Columns.Add(colArray[maxColCount].ToString());
                        }
                        else if (colCount == maxColCount)
                        {
                            chkRow = 0;
                        }
                        if (colCount > maxColCount)
                        {
                            maxColCount = colCount;
                        }
                    }
                    if (rowArray.Count > 0)
                    {

                        dt.Rows.Add(rowArray.ToArray());
                        rowArray = new ArrayList();
                    }
                    colCount = 0;
                    maxColCount = 0;
                }
            }
            return dt;
        }

        public static bool IsEmail(string inputEmail)
        {
            string pattern = @"^([a-zA-Z0-9_\-\'\&\.]+)@((\[[0-9]{1,3}" +
                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Match match =
                Regex.Match(inputEmail.Trim(), pattern, RegexOptions.IgnoreCase);

            return match.Success;
        }

        public static String ReplaceCRwithBR(String Input)
        {
            return Input.Replace("\r", "<br>");
        }

        public static String ArrayToString(IList List, String Separator)
        {
            String str = "";
            for (int i = 0; i < List.Count; i++)
            {
                str += List[i].ToString() + Separator;
            }
            return str.Substring(0, str.LastIndexOf(Separator));
        }

        public static void EnableButton(ListItemCollection Items, Button btnSave)
        {
            if (Items.Count > 0 && (ToInt(Items[0].Value) > 0 && Items[0].Value != ""))
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        public static void AssignValueToDropDown(DropDownList Ddl, String Value)
        {
            if (Ddl.Items.FindByValue(Value) != null)
            {
                Ddl.SelectedValue = Value;
            }
        }

        public static String GenerateRandomPassword(int PasswordLength)
        {
            String AllowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            String strPWD = "";
            char[] chars = AllowedChars.ToCharArray();
            int length = AllowedChars.Length;
            Random r = new Random(0);
            for (int i = 0; i < PasswordLength; i++)
            {
                strPWD += chars[r.Next(length - 1)];
            }
            return strPWD;
        }

        public static string GenerateRandomPassword()
        {
            int passwordLength = 30;
            String AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sbPassword = new StringBuilder();
            for (int i = 0; i < passwordLength; i++)
            {
                int nextSeed = 0;
                int previousSeed = 0;
                do
                {
                    for (int j = 0; j < AllowedChars.Length; j++)
                    {
                        Random rnd = new Random();
                        nextSeed = rnd.Next(0, AllowedChars.Length);
                        if (previousSeed == 0)
                        {
                            previousSeed = nextSeed;
                        }
                    }
                }
                while (nextSeed == previousSeed);
                sbPassword.Append(AllowedChars[nextSeed]);
            }
            return sbPassword.ToString();
        }

        public static string GetVistaFormatDate(DateTime dt)
        {
            return dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0');
        }

        public static DateTime GetDateFromVistaFormat(String StrDate)
        {
            if (StrDate.Length >= 6)
            {
                DateTime dt = new DateTime(IEUtils.ToInt(StrDate.Substring(0, 4)), IEUtils.ToInt(StrDate.Substring(4, 2)), IEUtils.ToInt(StrDate.Substring(6, 2)));
                return dt;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static void SelectValueInDropDown(System.Web.UI.WebControls.DropDownList DDL, string Value)
        {
            if (DDL.Items.Count > 0)
            {
                if (DDL.Items.FindByValue(Value) != null)
                {
                    DDL.SelectedValue = Value;
                }
                else
                {
                    DDL.SelectedIndex = 0;
                }
            }
        }

        public static void FillCreditCardExpirationDate(DropDownList ddlPassedDropdown, string sType)
        {

            if (sType == "Month")
            {
                for (int iMonth = 1; iMonth <= 12; iMonth++)
                {
                    ddlPassedDropdown.Items.Add(new ListItem(iMonth.ToString(), iMonth.ToString()));
                }
            }
            if (sType == "Year")
            {
                int cYear = int.Parse(DateTime.Now.Year.ToString());//stores current year
                int nYear = cYear + 10;//stores Next 10 years
                for (int iYear = cYear; iYear <= nYear; iYear++)
                {
                    ddlPassedDropdown.Items.Add(new ListItem(iYear.ToString(), iYear.ToString()));
                }
            }

        }

        public static double CalculateShipping(int NoOfPages, int QtyOrdered)
        {
            double noOfPieceOfPaper = (NoOfPages * QtyOrdered) / 2;
            double weightOfPaper = noOfPieceOfPaper * 0.013779;
            double noOfCartonsNeeded = weightOfPaper / 36;
            double weightOfCarton = noOfCartonsNeeded * 3.5;
            double totalWeight = weightOfCarton + weightOfPaper;
            return Math.Round(GetShipping(totalWeight), 2);
        }
        static double GetShipping(double weight)
        {
            return weight * 5;
        }
        public static DataTable RestrictArticleTitleLength(DataTable dt, String TextField, int FixLength)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (IEUtils.ToString(row[TextField]).Length > FixLength)
                {
                    row[TextField] = IEUtils.ToString(row[TextField]).Replace(IEUtils.ToString(row[TextField]).Substring(FixLength), "...");
                }
            }
            return dt;
        }
        public static bool IsValidSqlDate(DateTime dtTime)
        {
            DateTime dtMin = DateTime.Parse("1/1/1753");
            DateTime dtMax = DateTime.Parse("12/31/9999");
            try
            {
                if (dtTime >= dtMin && dtTime <= dtMax)
                {
                    return true;

                }
                return false;

            }
#pragma warning disable CS0168 // The variable 'exp' is declared but never used
            catch (Exception exp)
#pragma warning restore CS0168 // The variable 'exp' is declared but never used
            {
                return false;
            }
        }

        public static MemoryStream GetMemoryStreamFromImage(System.Drawing.Image img)
        {
            MemoryStream imgStream = new MemoryStream();
            img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);
            imgStream.Position = 0;
            return imgStream;
        }

        public static byte[] GetBytesFromImage(System.Drawing.Image img)
        {

            MemoryStream imgStream = GetMemoryStreamFromImage(img);
            byte[] imgBytes = new byte[imgStream.Length];
            imgStream.Read(imgBytes, 0, (int)imgStream.Length);

            return imgBytes;
        }

        public static byte[] ResizeFromStream(int MaxSideSize, Stream Buffer)
        {

            int intNewWidth;
            int intNewHeight;

            System.Drawing.Image imgInput = System.Drawing.Image.FromStream(Buffer);

            //Determine image format 
            System.Drawing.Imaging.ImageFormat fmtImageFormat = imgInput.RawFormat;

            //get image original width and height 
            int intOldWidth = imgInput.Width;
            int intOldHeight = imgInput.Height;

            //determine if landscape or portrait 
            int intMaxSide;

            if (intOldWidth >= intOldHeight)
            {
                intMaxSide = intOldWidth;
            }
            else
            {
                intMaxSide = intOldHeight;
            }


            if (intMaxSide > MaxSideSize)
            {
                //set new width and height 
                double dblCoef = MaxSideSize / (double)intMaxSide;
                intNewWidth = Convert.ToInt32(dblCoef * intOldWidth);
                intNewHeight = Convert.ToInt32(dblCoef * intOldHeight);
            }
            else
            {
                intNewWidth = intOldWidth;
                intNewHeight = intOldHeight;
            }
            //create new bitmap 
            Bitmap bmpResized = new Bitmap(imgInput, intNewWidth, intNewHeight);


            //MemoryStream newImageStream = new MemoryStream();
            //bmpResized.Save(newImageStream, System.Drawing.Imaging.ImageFormat.Bmp);
            //newImageStream.Position = 0;                
            byte[] imgBytes = IEUtils.GetBytesFromImage(bmpResized);//IEUtils.GetBytesFromStream(newImageStream);
            //release used resources 
            imgInput.Dispose();
            bmpResized.Dispose();
            Buffer.Close();
            return imgBytes;

        }

        public static void SetUserLocale(string CurrencySymbol)
        {
            HttpRequest Request = HttpContext.Current.Request;
            if (Request.UserLanguages == null)
                return;

            string Lang = Request.UserLanguages[0];
            if (Lang != null)
            {
                if (Lang.Length < 3)
                    Lang = Lang + "-" + Lang.ToUpper();
                try
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture =
                                       new System.Globalization.CultureInfo(Lang);

                    if (CurrencySymbol != null && CurrencySymbol != "")
                        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol = CurrencySymbol;
                }
                catch
                {; }
            }
        }
        public static void SetDateTimeCulture()
        {
            DateTimeFormatInfo dateFormat = System.Globalization.CultureInfo.GetCultureInfo("de-DE").DateTimeFormat;
            // if (dateFormat != null)
            //System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = dateFormat;
        }

        public static DataTable ConvertTo<T>(IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }

            return table;
        }
        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
             prop.PropertyType) ?? prop.PropertyType);
            }

            return table;
        }
        public static String GenerateRandomNumber()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = chars[random.Next(chars.Length)];

            var finalString = new String(stringChars);
            return finalString.ToString();
        }
    }
    public class AESHelper
    {
        private AesCryptoServiceProvider _aes;
        private ICryptoTransform _crypto;

        public AESHelper(string key, string IV)
        {
            _aes = new AesCryptoServiceProvider();
            _aes.BlockSize = 128;
            _aes.KeySize = 256;
            _aes.Key = ASCIIEncoding.ASCII.GetBytes(key);
            if (!string.IsNullOrEmpty(IV))
            {
                _aes.IV = ASCIIEncoding.ASCII.GetBytes(IV);
            }
            else
            {
                _aes.IV = new byte[_aes.BlockSize / 8];
            }

            _aes.Padding = PaddingMode.PKCS7;
            _aes.Mode = CipherMode.CBC;
        }

        public string encrypt(string message)
        {
            _crypto = _aes.CreateEncryptor(_aes.Key, _aes.IV);
            byte[] encrypted = _crypto.TransformFinalBlock(
                ASCIIEncoding.ASCII.GetBytes(message), 0, ASCIIEncoding.ASCII.GetBytes(message).Length);
            _crypto.Dispose();
            return System.Convert.ToBase64String(encrypted);
        }

        public string decrypt(string message)
        {
            _crypto = _aes.CreateDecryptor(_aes.Key, _aes.IV);
            byte[] decrypted = _crypto.TransformFinalBlock(
                System.Convert.FromBase64String(message), 0, System.Convert.FromBase64String(message).Length);
            _crypto.Dispose();
            return ASCIIEncoding.ASCII.GetString(decrypted);
        }
    }

}