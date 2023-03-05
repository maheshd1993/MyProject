using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam._Class
{
    public class UserRegModel
    {
        public string id { get; set; }
        public string userName { get; set; }
    }
    public class UserDetailModel
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string TimeZone { get; set; }
        public string ProfileName { get; set; }
        public string ProfileId { get; set; }
        public string MappedUsers { get; set; }
        public string FirstName { get; set; }
        public string UserId { get; set; }
        public string EmailID { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }

    }
    public class UserDetailModelList
    {
        public List<UserDetailModel> liUserDetailModel { get; set; }
    }

    public class CRMUserModel 
    {
        public int Id { get; set; }
        public int? ByUID { get; set; }      
        public string UserName { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TimeZone { get; set; }
        public string ProfileName { get; set; }
        public string ProfileId { get; set; }
        public bool? Status { get; set; }
        public string MappedUsers { get; set; }
        public DateTime? Created_at { get; set; }
        public decimal? CasualLeave { get; set; }
        public decimal? MedicalLeave { get; set; }
        public string Year { get; set; }
        public int? CompanyID { get; set; }
        public int? BranchID { get; set; }
        public string Token  { get; set; }
        public Int32? CountryID { get; set; }
        public string CountryName { get; set; }
        public string DateFormat  { get; set; }


        
    }
}