using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class CRMIndiaMartModel
    {
        public Int32? IndiaMartID { get; set; }

        [Required(ErrorMessage = "Please enter register mobile")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public String MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter indiamart crm key")]
        public String IndiaMartCRMKey { get; set; }

        public Int32? CompanyID { get; set; }
        public Int32? BranchID { get; set; }

        public string Token { get; set; }
    }
    public class RESPONSE
    {
        public string UNIQUE_QUERY_ID { get; set; }
        public string QUERY_TYPE { get; set; }
        public string QUERY_TIME { get; set; }
        public string SENDER_NAME { get; set; }
        public string SENDER_MOBILE { get; set; }
        public string SENDER_EMAIL { get; set; }
        public string SENDER_COMPANY { get; set; }
        public string SENDER_ADDRESS { get; set; }
        public string SENDER_CITY { get; set; }
        public string SENDER_STATE { get; set; }
        public string SENDER_COUNTRY_ISO { get; set; }
        public string SENDER_MOBILE_ALT { get; set; }
        public string SENDER_EMAIL_ALT { get; set; }
        public string QUERY_PRODUCT_NAME { get; set; }
        public string QUERY_MESSAGE { get; set; }
        public string CALL_DURATION { get; set; }
        public string RECEIVER_MOBILE { get; set; }
    }

    public class Root
    {
        public int CODE { get; set; }
        public string STATUS { get; set; }
        public string MESSAGE { get; set; }
        public int TOTAL_RECORDS { get; set; }
        public List<RESPONSE> RESPONSE { get; set; }
    }


    public class CRMIndiaMartResponseModel
    {
        //public string RN { get; set; }
        //public string QUERY_ID { get; set; }
        //public string QTYPE { get; set; }
        //public string SENDERNAME { get; set; }
        //public string SENDEREMAIL { get; set; }
        //public string SUBJECT { get; set; }
        //public string DATE_RE { get; set; }
        //public string DATE_R { get; set; }
        //public string DATE_TIME_RE { get; set; }
        //public string GLUSR_USR_COMPANYNAME { get; set; }
        //public string READ_STATUS { get; set; }
        //public string SENDER_GLUSR_USR_ID { get; set; }
        //public string MOB { get; set; }
        //public string COUNTRY_FLAG { get; set; }
        //public string QUERY_MODID { get; set; }
        //public string LOG_TIME { get; set; }
        //public string QUERY_MODREFID { get; set; }
        //public string DIR_QUERY_MODREF_TYPE { get; set; }
        //public string ORG_SENDER_GLUSR_ID { get; set; }
        //public string ENQ_MESSAGE { get; set; }
        //public string ENQ_ADDRESS { get; set; }
        //public string ENQ_CALL_DURATION { get; set; }
        //public string ENQ_RECEIVER_MOB { get; set; }
        //public string ENQ_CITY { get; set; }
        //public string ENQ_STATE { get; set; }
        //public string PRODUCT_NAME { get; set; }
        //public string COUNTRY_ISO { get; set; }
        //public string EMAIL_ALT { get; set; }
        //public string MOBILE_ALT { get; set; }
        //public string PHONE { get; set; }
        //public string PHONE_ALT { get; set; }
        //public string IM_MEMBER_SINCE { get; set; }
        //public string TOTAL_COUNT { get; set; }

        //public string CODE { get; set; }
        //public string STATUS { get; set; }
        //public string MESSAGE { get; set; }
        //public string TOTAL_RECORDS { get; set; }
        //public string RESPONSE { get; set; }
        //public string UNIQUE_QUERY_ID { get; set; }
        //public string QUERY_TYPE { get; set; }
        //public string QUERY_TIME { get; set; }
        //public string SENDER_NAME { get; set; }
        //public string SENDER_MOBILE { get; set; }
        //public string SENDER_EMAIL { get; set; }
        //public string SENDER_COMPANY { get; set; }
        //public string SENDER_ADDRESS { get; set; }
        //public string SENDER_CITY { get; set; }
        //public string SENDER_STATE { get; set; }
        //public string SENDER_COUNTRY_ISO { get; set; }
        //public string SENDER_MOBILE_ALT { get; set; }
        //public string SENDER_EMAIL_ALT { get; set; }
        //public string QUERY_PRODUCT_NAME { get; set; }
        //public string QUERY_MESSAGE { get; set; }
        //public string CALL_DURATION { get; set; }
        //public string RECEIVER_MOBILE { get; set; }

        public string UNIQUE_QUERY_ID { get; set; }
        public string QUERY_TYPE { get; set; }
        public string QUERY_TIME { get; set; }
        public string SENDER_NAME { get; set; }
        public string SENDER_MOBILE { get; set; }
        public string SENDER_EMAIL { get; set; }
        public string SENDER_COMPANY { get; set; }
        public string SENDER_ADDRESS { get; set; }
        public string SENDER_CITY { get; set; }
        public string SENDER_STATE { get; set; }
        public string SENDER_COUNTRY_ISO { get; set; }
        public string SENDER_MOBILE_ALT { get; set; }
        public string SENDER_EMAIL_ALT { get; set; }
        public string QUERY_PRODUCT_NAME { get; set; }
        public string QUERY_MESSAGE { get; set; }
        public string CALL_DURATION { get; set; }
        public string RECEIVER_MOBILE { get; set; }
        //public int CODE { get; set; }
        //public string STATUS { get; set; }
        //public string MESSAGE { get; set; }
        //public int TOTAL_RECORDS { get; set; }

    }
}