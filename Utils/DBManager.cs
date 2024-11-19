using MOCDIntegrations.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static MOCDIntegrations.Models.POD;
using static MOCDIntegrations.Models.StudentEFile;

namespace MOCDIntegrations.Utils
{
    public class DBManager
    {
        #region Queries
        string QRY_RETRIEVE_RENEW_POD_CARD = @"SELECT TranReferenceNo, TranRequestDate,  
ISNULL(AppFirstName, '') + ' ' + ISNULL(AppMiddleName, '') + ' '  + ISNULL(AppLastName, '') + ' ' + ISNULL(AppFamilyName, '') AS CustomerNameEn, 
ISNULL(DisabledFirstNameAr, '') + ' ' + ISNULL(DisabledMiddleNameAr, '') + ' ' + ISNULL(DisabledLastNameAr, '') + ' '  +  ISNULL(DisabledFamilyNameAr, '') AS CustomerNameAr ,
CASE Gender  WHEN 1 THEN N'ذكر'  WHEN 0 THEN N'أنثى'  END AS Gender,
CASE CanLiveAlone  WHEN 1 THEN N'نعم'  WHEN 0 THEN N'لا'  END AS CanLiveAlone,
CASE NeedSupporter  WHEN 1 THEN N'نعم'  WHEN 0 THEN N'لا'  END AS NeedSupporter,
DOB, MStatusAr, EduNameAr,  WorkingStatusNameAr, Company, VisaNo, PhoneNumber, MobileNumber, Email, TypeTitleAr, LevelTitleAr, WorkFieldNameAr,
EmirateTitleAr, CountryNameAr, DisabledCardNo, CardIssueDate, CardExpiryDate, StatusTitleAr, Address1, POBox, OtherMobileNo, MakaniNo,
dbo.fnGetDisabledEquipment(Equips) AS SupportingEquipmentAr, N'دائم' AS CardType, AccommodationTypeAr, AuthTitleAr, DiagnosisInfo,
MSADisabledTypes.TypeTitleAr, MSADisabledTypes.TypeTitleEn, MSACountries.CountryNameAr, MSACountries.CountryNameEn, SubTypeTitleAr, SubTypeTitleEn 
FROM MSARenewDisabledCard
INNER JOIN MSATransaction ON MSATransaction.TranId = MSARenewDisabledCard.TranId
INNER JOIN MSADisabledTypes ON MSADisabledTypes.TypeId = MSARenewDisabledCard.Disabilities
INNER JOIN MSAServiceStatus ON MSAServiceStatus.StatusId = MSATransaction.StatusId
INNER JOIN MSAEmirate ON MSAEmirate.EmirateId = MSARenewDisabledCard.Emirate
LEFT OUTER JOIN MSACountries ON MSACountries.CountryId = MSARenewDisabledCard.Nationality 
LEFT OUTER JOIN MSADisabledSubTypes ON MSADisabledSubTypes.SubTypeId = MSARenewDisabledCard.SubTypeId
LEFT OUTER JOIN MSADisabledDiaAuthority ON MSADisabledDiaAuthority.AuthId = MSARenewDisabledCard.DiagnosisAuthority
LEFT OUTER JOIN MSAWorkingStatus ON MSAWorkingStatus.WorkingStatusId = MSARenewDisabledCard.WorkingStatusId
LEFT OUTER JOIN MSADisabledCardDisabilityLevel ON MSADisabledCardDisabilityLevel.LevelId =  MSARenewDisabledCard.DisabilityLevelId
LEFT OUTER JOIN MSAEducationLevel ON MSAEducationLevel.EduId = MSARenewDisabledCard.EduId
LEFT OUTER JOIN MSAMaritalStatus ON MSAMaritalStatus.MaritalStatusId = MSARenewDisabledCard.MaritalStatusId
LEFT OUTER JOIN MSAWorkField ON MSAWorkField.WorkFieldId = MSARenewDisabledCard.WorkFieldId
LEFT OUTER JOIN MSADisabledCardAccommodationType ON MSADisabledCardAccommodationType.AccommodationTypeId = MSARenewDisabledCard.AccommodationTypeId
WHERE 
 VisaNo = '{0}'
ORDER BY CardIssueDate DESC";

        string QRY_RETRIEVE_NEW_POD_CARD = @"SELECT TranReferenceNo, TranRequestDate,  
ISNULL(AppFirstName, '') + ' ' + ISNULL(AppMiddleName, '') + ' '  + ISNULL(AppLastName, '') + ' ' + ISNULL(AppFamilyName, '') AS CustomerNameEn, 
ISNULL(DisabledFirstNameAr, '') + ' ' + ISNULL(DisabledMiddleNameAr, '') + ' ' + ISNULL(DisabledLastNameAr, '') + ' '  +  ISNULL(DisabledFamilyNameAr, '') AS CustomerNameAr ,
CASE Gender  WHEN 1 THEN N'ذكر'  WHEN 0 THEN N'أنثى'  END AS Gender,
CASE CanLiveAlone  WHEN 1 THEN N'نعم'  WHEN 0 THEN N'لا'  END AS CanLiveAlone,
CASE NeedSupporter  WHEN 1 THEN N'نعم'  WHEN 0 THEN N'لا'  END AS NeedSupporter,
DOB, MStatusAr, EduNameAr,  WorkingStatusNameAr, Company, VisaNo, PhoneNumber, MobileNumber, Email, MSADisabledTypes.TypeTitleAr, LevelTitleAr, WorkFieldNameAr,
EmirateTitleAr, CountryNameAr, DisabledCardNo, CardIssueDate, CardExpiryDate, StatusTitleAr, Address1, POBox, OtherMobileNo, MakaniNo,
dbo.fnGetDisabledEquipment(Equips) AS SupportingEquipmentAr, MSADisabledCardType.TypeTitleAr AS CardType, AccommodationTypeAr, AuthTitleAr, DiagnosisInfo,
MSADisabledTypes.TypeTitleAr, MSADisabledTypes.TypeTitleEn, MSACountries.CountryNameAr, MSACountries.CountryNameEn, SubTypeTitleAr, SubTypeTitleEn 
FROM MSADisabledCard
INNER JOIN MSATransaction ON MSATransaction.TranId = MSADisabledCard.TranId
INNER JOIN MSADisabledTypes ON MSADisabledTypes.TypeId = MSADisabledCard.Disabilities
INNER JOIN MSAServiceStatus ON MSAServiceStatus.StatusId = MSATransaction.StatusId
INNER JOIN MSAEmirate ON MSAEmirate.EmirateId = MSADisabledCard.Emirate
LEFT OUTER JOIN MSACountries ON MSACountries.CountryId = MSADisabledCard.Nationality 
LEFT OUTER JOIN MSADisabledSubTypes ON MSADisabledSubTypes.SubTypeId = MSADisabledCard.SubTypeId
LEFT OUTER JOIN MSADisabledDiaAuthority ON MSADisabledDiaAuthority.AuthId = MSADisabledCard.DiagnosisAuthority
LEFT OUTER JOIN MSAWorkingStatus ON MSAWorkingStatus.WorkingStatusId = MSADisabledCard.WorkingStatusId
LEFT OUTER JOIN MSADisabledCardDisabilityLevel ON MSADisabledCardDisabilityLevel.LevelId =  MSADisabledCard.DisabilityLevelId
LEFT OUTER JOIN MSAEducationLevel ON MSAEducationLevel.EduId = MSADisabledCard.EduId
LEFT OUTER JOIN MSAMaritalStatus ON MSAMaritalStatus.MaritalStatusId = MSADisabledCard.MaritalStatusId
LEFT OUTER JOIN MSADisabledCardType ON MSADisabledCardType.TypeId = MSADisabledCard.CardTypeId
LEFT OUTER JOIN MSAWorkField ON MSAWorkField.WorkFieldId = MSADisabledCard.WorkFieldId
LEFT OUTER JOIN MSADisabledCardAccommodationType ON MSADisabledCardAccommodationType.AccommodationTypeId = MSADisabledCard.AccommodationTypeId
WHERE MSATransaction.ServiceId = 3

AND VisaNo = '{0}'";

        string QRY_RETRIEVE_PODCENTER_STUDENT = @"SELECT MSARegisterInRehablitationCenterRequest.*,    MSACountries.CountryNameEn, MSACountries.CountryNameAr, GenderNameEn, GenderNameAr, StatusTitleEn, StatusTitleAr,
MStatusEn, MStatusAr, MSATransaction.StatusId, MSATransaction.TranId, MSATransaction.TranReferenceNo, MSATransaction.TranRequestDate, RNameEn, RNameAr,
TypeTitleAr, TypeTitleEn, EmirateTitleAr,  EmirateTitleEn, CenterTitleAr, CenterTitleEn 
FROM MSARegisterInRehablitationCenterRequest
INNER JOIN MSATransaction ON MSATransaction.TranId = MSARegisterInRehablitationCenterRequest.TranId
INNER JOIN MSAServiceStatus ON MSAServiceStatus.StatusId = MSATransaction.StatusId
LEFT OUTER JOIN MSAGender ON MSAGender.GenderId = MSARegisterInRehablitationCenterRequest.GenderId
LEFT OUTER JOIN MSACountries ON MSACountries.CountryId = MSARegisterInRehablitationCenterRequest.NationalityId
LEFT OUTER JOIN MSAMaritalStatus ON MSAMaritalStatus.MaritalStatusId = MSARegisterInRehablitationCenterRequest.MaritalStatusId
LEFT OUTER JOIN MSARelationship ON MSARelationship.RelationshipId = MSARegisterInRehablitationCenterRequest.RelationshipId
LEFT OUTER JOIN MSARehablitationCenterDisabledTypes ON MSARehablitationCenterDisabledTypes.TypeId = MSARegisterInRehablitationCenterRequest.DisabilityTypeId
LEFT OUTER JOIN MSAEmirate ON MSAEmirate.EmirateId =  MSARegisterInRehablitationCenterRequest.EmirateId
LEFT OUTER JOIN MSARegisterInRehablitationCenterRequestCenters ON MSARegisterInRehablitationCenterRequestCenters.CenterId = MSARegisterInRehablitationCenterRequest.CenterId
WHERE NationalId = '{0}'";

        string QRY_RETRIEVE_MOJ_CaseDetails = @"SELECT  [EMIRATE]
      ,[COURT]
      ,a.[CASE_NUMBER]
      ,[YEAR]
      ,[CASE_STATUS]
      ,[COURT_TYPE]
      ,[PROCEEDING_TYPE]
      ,[SUB_PROCEEDING_TYPE]
      ,[FILING_PARTY_TYPE]
      ,[FILING_PARTY_NAME]
      ,[DATE_CASE_OPENED]
      ,[CLAIM_AMOUNT]
      ,[PLAINTIFF_NAME]
      ,[PLAINTIFF_NATIONALITY]
      ,[PLAINTIFF_E_ID]
      ,[PLAINTIFF_MOBILE]
      ,[PLAINTIFF_EMAIL]
      ,[DEFENDANT_NAME]
      ,[DEFENDANT_NATIONALITY]
      ,[DEFENDANT_E_ID]
      ,[DEFENDANT_MOBILE]
      ,[DEFENDANT_EMAIL]
      ,[FEE]
      ,[FEE_PAYMENT_STATUS]
	  ,[Type]
      ,[NAME]
      ,[NATIONALITY]
      ,[E_ID]
      ,[MOBILE]
      ,[EMAIL]
  FROM [UnifiedDB].[dbo].[MOJCaseDetails] as a inner join MOJCasePlaintiffDefendantDetails as b on a.CASE_NUMBER = b.CASE_NUMBER
WHERE CASE_NUMBER = '{0}'";



        #endregion
        SqlConnection con;
        SqlDataAdapter adapter;
        SqlCommand cmd;
        DataSet ds, dsDoc;
        private void OpenConnection()
        {
            try
            {
                con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.AppSettings["MoCD_ConStr"].ToString();
                //Helper.LogAudit(con.ConnectionString, "OpenConnection");
                con.Open();
            }
            catch (Exception ex)
            {
                //Helper.LogException(ex, "DBManager - " + new StackTrace(ex).GetFrame(0).GetMethod().Name);
            }
        }
        private void CloseConnection()
        {
            try
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
                adapter.Dispose();
                ds.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private PODCardResponse ConvertDSToModel(DataSet dsPOD)
        {
            PODCardResponse card = new PODCardResponse();
            try
            {
                if (dsPOD != null && dsPOD.Tables.Count > 0 && dsPOD.Tables.Contains(Tables.MSADisabledCard.TableName))
                {
                    DataRow dr = dsPOD.Tables[Tables.MSADisabledCard.TableName].Rows[0];
                    card.CustomerNameAr = dr[Tables.MSADisabledCard.CustomerNameAr].ToString();
                    card.CustomerNameEn = dr[Tables.MSADisabledCard.CustomerNameEn].ToString();
                    if (!string.IsNullOrEmpty(dr[Tables.MSADisabledCard.DOB].ToString()))
                        card.DateOfBirth = Utils.FormatDate(Convert.ToDateTime(dr[Tables.MSADisabledCard.DOB]));
                    card.DisabilityTitleAR = dr[Tables.MSADisabledCard.TypeTitleAr].ToString();
                    if (!string.IsNullOrEmpty(dr[Tables.MSADisabledCard.CardExpiryDate].ToString()))
                        card.CardExpiryDate = Utils.FormatDate(Convert.ToDateTime(dr[Tables.MSADisabledCard.CardExpiryDate]));
                    if (!string.IsNullOrEmpty(dr[Tables.MSADisabledCard.CardIssueDate].ToString()))
                        card.CardIssueDate = Utils.FormatDate(Convert.ToDateTime(dr[Tables.MSADisabledCard.CardIssueDate]));
                    card.NationalityAr = dr[Tables.MSADisabledCard.CountryNameAr].ToString();
                    card.DisabledCardNo = dr[Tables.MSADisabledCard.DisabledCardNo].ToString();
                    card.SupportingEquipmentAr = dr[Tables.MSADisabledCard.SupportingEquipmentAr].ToString();
                    card.NeedSupporter = dr[Tables.MSADisabledCard.NeedSupporter].ToString();
                    card.DisabilitySubTypeAr = dr[Tables.MSADisabledCard.SubTypeTitleAr].ToString();
                    card.TranReferenceNo = dr[Tables.MSADisabledCard.TranReferenceNo].ToString();
                    card.TranRequestDate = Utils.FormatDate(Convert.ToDateTime(dr[Tables.MSADisabledCard.TranRequestDate]));
                    card.StatusTitleAr = dr[Tables.MSADisabledCard.StatusTitleAr].ToString();
                    card.CardType = dr[Tables.MSADisabledCard.CardType].ToString();
                    card.GenderAr = dr[Tables.MSADisabledCard.Gender].ToString();
                    card.IdentificationNo = dr[Tables.MSADisabledCard.VisaNo].ToString();
                    card.EmirateAr = dr[Tables.MSADisabledCard.EmirateTitleAr].ToString();
                    card.Address = dr[Tables.MSADisabledCard.Address1].ToString();
                    card.PhoneNo = dr[Tables.MSADisabledCard.PhoneNumber].ToString();
                    card.POBox = dr[Tables.MSADisabledCard.POBox].ToString();
                    card.MobileNo = dr[Tables.MSADisabledCard.MobileNumber].ToString();
                    card.OtherMobileNo = dr[Tables.MSADisabledCard.OtherMobileNo].ToString();
                    card.Email = dr[Tables.MSADisabledCard.Email].ToString();
                    card.MakaniNo = dr[Tables.MSADisabledCard.MakaniNo].ToString();
                    card.WorkingStatusAr = dr[Tables.MSADisabledCard.WorkingStatusNameAr].ToString();
                    card.WorkFieldAr = dr[Tables.MSADisabledCard.WorkFieldNameAr].ToString();
                    card.MaritalStatusAr = dr[Tables.MSADisabledCard.MStatusAr].ToString();
                    card.QualificationAr = dr[Tables.MSADisabledCard.EduNameAr].ToString();
                    card.AccommodationTypeAr = dr[Tables.MSADisabledCard.AccommodationTypeAr].ToString();
                    card.DiagnosisAuthorityAr = dr[Tables.MSADisabledCard.AuthTitleAr].ToString();
                    card.DiagnosisInformation = dr[Tables.MSADisabledCard.DiagnosisInfo].ToString();
                    card.DisabilityTitleAR = dr[Tables.MSADisabledCard.TypeTitleAr].ToString();
                    card.DisabilityLevelAr = dr[Tables.MSADisabledCard.LevelTitleAr].ToString();
                    card.SupportingEquipmentAr = dr[Tables.MSADisabledCard.SupportingEquipmentAr].ToString();
                    card.NeedSupporter = dr[Tables.MSADisabledCard.NeedSupporter].ToString();
                    card.CanLiveAlone = dr[Tables.MSADisabledCard.CanLiveAlone].ToString();

                }
            }
            catch (Exception ex)
            {

            }

            return card;
        }


        public PODCardResponse RetrievePODCardDetails(string emiratesId)
        {
            PODCardResponse card = new PODCardResponse();
            string query = string.Empty;
            try
            {
                this.OpenConnection();

                query = string.Format(QRY_RETRIEVE_RENEW_POD_CARD, emiratesId);
                cmd = new SqlCommand(query, con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                dsDoc = new DataSet();
                adapter.Fill(ds);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].TableName = Tables.MSADisabledCard.TableName;
                    card = ConvertDSToModel(ds);
                }
                else
                {
                    query = string.Format(QRY_RETRIEVE_NEW_POD_CARD, emiratesId);
                    cmd.CommandText = query;
                    adapter.Fill(ds);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].TableName = Tables.MSADisabledCard.TableName;
                        card = ConvertDSToModel(ds);
                    }
                }

            }
            catch (Exception ex)
            {
                //Helper.LogException(ex, "DBManager - " + new StackTrace(ex).GetFrame(0).GetMethod().Name);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return card;
        }
        public string RetrieveRenewPODCardIDs()
        {
            string query = string.Empty, renewIDs = string.Empty;
            int renewDisabledCardId = 0;
            try
            {
                this.OpenConnection();

                query = "SELECT DISTINCT DisabledCardId FROM MSARenewDisabledCard WHERE DisabledCardId <> 0 ORDER BY DisabledCardId DESC";
                cmd = new SqlCommand(query, con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        renewDisabledCardId = RetrieveIDs(Convert.ToInt32(dr["DisabledCardId"]));
                        if (renewDisabledCardId != 0)
                            renewIDs = renewIDs + renewDisabledCardId + ",";
                    }
                }
            }
            catch (Exception ex)
            {
                //Helper.LogException(ex, "DBManager - " + new StackTrace(ex).GetFrame(0).GetMethod().Name);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return renewIDs;
        }
        private int RetrieveIDs(int disabledCardId)
        {
            string query = string.Empty;
            int renewDisabledCardId = 0;
            try
            {
                this.OpenConnection();

                query = string.Format("SELECT DisabledCardId, RenewDisabledCardID, CardExpiryDate FROM MSARenewDisabledCard WHERE DisabledCardId = {0} ORDER BY CardExpiryDate DESC", disabledCardId);
                cmd = new SqlCommand(query, con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DateTime dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["CardExpiryDate"]);
                    if (dt < DateTime.Now)
                        renewDisabledCardId = Convert.ToInt32(ds.Tables[0].Rows[0]["RenewDisabledCardID"]);
                }
            }
            catch (Exception ex)
            {
                //Helper.LogException(ex, "DBManager - " + new StackTrace(ex).GetFrame(0).GetMethod().Name);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return renewDisabledCardId;
        }

        #region Student E-File
        public StudentEFileResponse RetrievePODCenterStudentDetails(string emiratesId)
        {
            StudentEFileResponse student = new StudentEFileResponse();
            string query = string.Empty;
            try
            {
                this.OpenConnection();

                query = string.Format(QRY_RETRIEVE_PODCENTER_STUDENT, emiratesId);
                cmd = new SqlCommand(query, con);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                dsDoc = new DataSet();
                adapter.Fill(ds);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].TableName = Tables.MSARegisterInRehablitationCenterRequest.TableName;
                    student = ConvertPODCenterDSToModel(ds);
                }
            }
            catch (Exception ex)
            {
                //Helper.LogException(ex, "DBManager - " + new StackTrace(ex).GetFrame(0).GetMethod().Name);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return student;
        }

        private StudentEFileResponse ConvertPODCenterDSToModel(DataSet dsPOD)
        {
            StudentEFileResponse student = new StudentEFileResponse();
            try
            {
                if (dsPOD != null && dsPOD.Tables.Count > 0 && dsPOD.Tables.Contains(Tables.MSARegisterInRehablitationCenterRequest.TableName))
                {
                    DataRow dr = dsPOD.Tables[Tables.MSARegisterInRehablitationCenterRequest.TableName].Rows[0];
                    student.Address = dr[Tables.MSARegisterInRehablitationCenterRequest.Address].ToString();
                    student.Area = dr[Tables.MSARegisterInRehablitationCenterRequest.Area].ToString();
                    
                    student.CenterTitleAr = dr[Tables.MSARegisterInRehablitationCenterRequest.CenterTitleAr].ToString();
                    student.ChildName = dr[Tables.MSARegisterInRehablitationCenterRequest.ChildName].ToString();
                    student.CountryNameAr = dr[Tables.MSARegisterInRehablitationCenterRequest.CountryNameAr].ToString();
                    if (!string.IsNullOrEmpty(dr[Tables.MSARegisterInRehablitationCenterRequest.DateOfBirth].ToString()))
                        student.DateOfBirth = Utils.FormatDate(Convert.ToDateTime(dr[Tables.MSARegisterInRehablitationCenterRequest.DateOfBirth]));
                    student.DisabledCardNo = dr[Tables.MSARegisterInRehablitationCenterRequest.DisabledCardNo].ToString();
                    student.Email = dr[Tables.MSARegisterInRehablitationCenterRequest.Email].ToString();
                    student.EmirateTitleAr = dr[Tables.MSARegisterInRehablitationCenterRequest.EmirateTitleAr].ToString();
                    student.FatherMobileNumber = dr[Tables.MSARegisterInRehablitationCenterRequest.FatherMobileNo].ToString();
                    student.GenderNameAr = dr[Tables.MSARegisterInRehablitationCenterRequest.GenderNameAr].ToString();
                    student.GuardianName = dr[Tables.MSARegisterInRehablitationCenterRequest.GuardianName].ToString();
                    student.HomePhoneNumber = dr[Tables.MSARegisterInRehablitationCenterRequest.HomePhoneNo].ToString();
                    student.MobileNumber = dr[Tables.MSARegisterInRehablitationCenterRequest.MobileNo].ToString();
                    student.MotherMobileNumber = dr[Tables.MSARegisterInRehablitationCenterRequest.MotherMobileNo].ToString();
                    student.MStatusAr = dr[Tables.MSARegisterInRehablitationCenterRequest.MStatusAr].ToString();
                    student.NationalId = dr[Tables.MSARegisterInRehablitationCenterRequest.NationalId].ToString();
                    student.PhoneNumber = dr[Tables.MSARegisterInRehablitationCenterRequest.PhoneNo].ToString();
                    student.RNameAr = dr[Tables.MSARegisterInRehablitationCenterRequest.RNameAr].ToString();
                    student.StatusTitleAr = dr[Tables.MSARegisterInRehablitationCenterRequest.StatusTitleAr].ToString();
                    student.TranReferenceNo = dr[Tables.MSARegisterInRehablitationCenterRequest.TranReferenceNo].ToString();
                    if (!string.IsNullOrEmpty(dr[Tables.MSARegisterInRehablitationCenterRequest.TranRequestDate].ToString()))
                        student.TranRequestDate = Utils.FormatDate(Convert.ToDateTime(dr[Tables.MSARegisterInRehablitationCenterRequest.TranRequestDate]));
                    student.TypeTitleAr = dr[Tables.MSARegisterInRehablitationCenterRequest.TypeTitleAr].ToString();
                    
                }
            }
            catch (Exception ex)
            {

            }

            return student;
        }


        #endregion


        #region MOJ Case Details
        //public MOJCaseDetails RetrieveMOJCaseDetails(string emiratesId)
        //{
        //    MOJCaseDetails student = new MOJCaseDetails();
        //    string query = string.Empty;
        //    try
        //    {
        //        this.OpenConnection();

        //        query = string.Format(QRY_RETRIEVE_MOJ_CaseDetails, emiratesId);
        //        cmd = new SqlCommand(query, con);
        //        adapter = new SqlDataAdapter(cmd);
        //        ds = new DataSet();
        //        dsDoc = new DataSet();
        //        adapter.Fill(ds);

        //        if (ds != null && ds.Tables[0].Rows.Count > 0)
        //        {
        //            ds.Tables[0].TableName = "MOJCaseDetails";
        //            student = ConvertMOJCaseDetailsDSToModel(ds);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Helper.LogException(ex, "DBManager - " + new StackTrace(ex).GetFrame(0).GetMethod().Name);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        CloseConnection();
        //    }
        //    return student;
        //}

        //private MOJCaseDetails ConvertMOJCaseDetailsDSToModel(DataSet dsPOD)
        //{
        //    MOJCaseDetails student = new MOJCaseDetails();
        //    try
        //    {
        //        if (dsPOD != null && dsPOD.Tables.Count > 0 && dsPOD.Tables.Contains(Tables.MSARegisterInRehablitationCenterRequest.TableName))
        //        {
        //            DataRow dr = dsPOD.Tables["MOJCaseDetails"].Rows[0];
        //            student.EMIRATE = dr["MOJCaseDetails"].ToString();
        //            student.COURT = dr["MOJCaseDetails"].ToString();

        //            student.CASE_NUMBER = dr["MOJCaseDetails"].ToString();
        //            student.YEAR = dr["MOJCaseDetails"].ToString();
        //            student.CASE_STATUS = dr["MOJCaseDetails"].ToString();
                    
        //            student.PROCEEDING_TYPE = dr["MOJCaseDetails"].ToString();
        //            student.COURT_TYPE = dr["MOJCaseDetails"].ToString();
        //            student.SUB_PROCEEDING_TYPE = dr["MOJCaseDetails"].ToString();
        //            student.FILING_PARTY_NAME = dr["MOJCaseDetails"].ToString();
        //            student.FILING_PARTY_TYPE = dr["MOJCaseDetails"].ToString();
        //            student.DATE_CASE_OPENED = dr["MOJCaseDetails"].ToString();
        //            student.CLAIM_AMOUNT = (float)dr["MOJCaseDetails"];
        //            student.FEE = (float)dr["MOJCaseDetails"];
        //            student.FEE_PAYMENT_STATUS = dr["MOJCaseDetails"].ToString();
        //            //student.Plaintiff. = dr[Tables.MSARegisterInRehablitationCenterRequest.MStatusAr].ToString();
        //            //student.NationalId = dr[Tables.MSARegisterInRehablitationCenterRequest.NationalId].ToString();
        //            //student.PhoneNumber = dr[Tables.MSARegisterInRehablitationCenterRequest.PhoneNo].ToString();
        //            //student.RNameAr = dr[Tables.MSARegisterInRehablitationCenterRequest.RNameAr].ToString();
        //            //student.StatusTitleAr = dr[Tables.MSARegisterInRehablitationCenterRequest.StatusTitleAr].ToString();
        //            //student.TranReferenceNo = dr[Tables.MSARegisterInRehablitationCenterRequest.TranReferenceNo].ToString();
        //            //if (!string.IsNullOrEmpty(dr[Tables.MSARegisterInRehablitationCenterRequest.TranRequestDate].ToString()))
        //            //    student.TranRequestDate = Utils.FormatDate(Convert.ToDateTime(dr[Tables.MSARegisterInRehablitationCenterRequest.TranRequestDate]));
        //            //student.TypeTitleAr = dr[Tables.MSARegisterInRehablitationCenterRequest.TypeTitleAr].ToString();

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return student;
        //}


        #endregion
    }

}