using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class DeathCertificateDetails
    {
        public class DeathCertificateDetailsRequestParams
        {
            public string EmiratesId { get; set; }
        }

        public class Rooot
        {
            public data data { get; set; }
        }
        public class data
        {
            public string EmiratesId { get; set; }
            public string StatusCode { get; set; }
            public string StatusDescriptionEnglish { get; set; }
            public string StatusDescriptionArabic { get; set; }
            public string DeathReferenceNo { get; set; }
            public string FullNameEnglish { get; set; }
            public string FullNameArabic { get; set; }
            public string DateOfBirth { get; set; }
            public string GenderCode { get; set; }
            public string GenderDescriptionEnglish { get; set; }
            public string GenderDescriptionArabic { get; set; }
            public string ReligonCode { get; set; }
            public string ReligonDescriptionEnglish { get; set; }
            public string ReligonDescriptionArabic { get; set; }
            public string NationalityCode { get; set; }
            public string NationalityDescriptionEnglish { get; set; }
            public string NationalityDescriptionArabic { get; set; }
            public string MaritalStatusCode { get; set; }
            public string MartialStatusDescriptionEnglish { get; set; }
            public string MartialStatusDescriptionArabic { get; set; }
            public string OccupationCode { get; set; }
            public string OccupationDescriptionEnglish { get; set; }
            public string OccupationDescriptionArabic { get; set; }
            public string CertificatePlaceOfIssueCode { get; set; }
            public string CertificatePlaceOfIssueDescriptionEnglish { get; set; }
            public string CertificatePlaceOfIssueDescriptionArabic { get; set; }
            public string DeathPlace { get; set; }
            public string DateOfDeath { get; set; }
            public string DateOfDeathInWordsEnglish { get; set; }
            public string DateOfDeathHijri { get; set; }
            public string Age { get; set; }
            public List<Details> details { get; set; }

        }
        public class Details
        {
            public string Id { get; set; }
            public string FullName { get; set; }
            public string HelpKind { get; set; }
            public string HelpType { get; set; }
            public string HelpType_Ar { get; set; }
            public string Amount { get; set; }
            public string ApprovalDate { get; set; }
            public string Start_Date { get; set; }
            public string No_Of_Installments { get; set; }
        }
        public class IEUtils
        {
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
                catch (Exception exp)
                {
                    throw exp;
                }

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
                catch (Exception exp)
                {
                    return 0;
                }
            }

            public static string ToString(object o)
            {
                if (o == null) return "";
                if (o.GetType().Equals(typeof(System.DBNull)))
                    return "";
                return o.ToString();
            }

            public static System.Data.DataTable CreateTable<T>()
            {
                Type entityType = typeof(T);
                System.Data.DataTable table = new System.Data.DataTable(entityType.Name);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

                foreach (PropertyDescriptor prop in properties)
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
                 prop.PropertyType) ?? prop.PropertyType);
                }

                return table;
            }
        }

    }
}