using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOCDIntegrations.Models.MOEcNERBL
{
    public class MOEcOwnerDetail
    {
        [Key]
        public int MOEcOwnerDetailId { get; set; }

        public string CBLSSubmissionID { get; set; }
        [ForeignKey("MOEcOwnerDetailId")]

        public virtual ICollection<Message> Message { get; set; }
        [ForeignKey("MOEcOwnerDetailId")]
        public virtual ICollection<MOEcBusinessLicenseDetial> BLList { get; set; }
        public string OwnerCBLSId { get; set; }
        public string OwnerFullNameAR { get; set; }
        public string OwnerFullNameEN { get; set; }
        public string UnifiedId { get; set; }

        public string EmiratesId { get; set; }
        public string PassportNo { get; set; }
        [Required]
        public DateTime InsertDate { get; set; } = DateTime.Now;
    }

    public class Message
    {
        [Key]
        public int MessageID { get; set; }
        public int MOEcOwnerDetailId { get; set; }
        public int MessageType { get; set; }
        public int MessageCode { get; set; }
        public string MessageTextAR { get; set; }
        public string MessageTextEN { get; set; }
    }

    public class MOEcBusinessLicenseDetial
    {
        [Key]
        public int MOEcBusinessLicenseId { get; set; }
        public int MOEcOwnerDetailId { get; set; }
        public string BLCBLSId { get; set; }
        public string EDLicenseID { get; set; }
        public string LicenseNameEn { get; set; }
        public string LicenseNameAr { get; set; }
        
        public string LicenseLastModifyDateString { get; set; }
        public string LicenseExiryDateString { get; set; }


        private DateTime? LicenseExiryDate_;

        public DateTime LicenseExiryDate
        {
            get { return Convert.ToDateTime(LicenseExiryDate_); }
            set
            {
                DateTime tempDate;
                if (DateTime.TryParse(value.ToString(), out tempDate) && value.ToString("MM/dd/yyyy") != "01/01/0001")
                {
                    LicenseExiryDate_ = value;
                    LicenseExiryDateString = value.ToString("MM/dd/yyyy");
                }
                else
                {
                    LicenseExiryDate_ = null;
                    LicenseExiryDateString = "";
                }
            }
        }
        private DateTime? LicenseLastModifyDate_;
        public DateTime LicenseLastModifyDate
        {
            get { return Convert.ToDateTime(LicenseLastModifyDate_); }
            set
            {
                DateTime tempDate;
                if (DateTime.TryParse(value.ToString(), out tempDate) && value.ToString("MM/dd/yyyy") != "01/01/0001")
                {
                    LicenseLastModifyDate_ = value;
                    LicenseLastModifyDateString = value.ToString("MM/dd/yyyy");
                }
                else
                {
                    LicenseLastModifyDate_ = null;
                    LicenseLastModifyDateString = "";
                }

            }
        }



        //public DateTime LicenseExiryDate { get; set; }
        //public DateTime LicenseLastModifyDate { get; set; }
        public int IssuanceEDID { get; set; }
        public string IssuanceEDAR { get; set; }
        public string IssuanceEDEN { get; set; }
        public int LicenseTypeID { get; set; }
        public string LicenseTypeAR { get; set; }
        public string LicenseTypeEN { get; set; }
        public int OwnerRelationshipTypeID { get; set; }
        public string OwnerRelationshipTypeEn { get; set; }
        public string OwnerRelationshipTypeAr { get; set; }
        public string OwnerSharePercentage { get; set; }
    }
}
