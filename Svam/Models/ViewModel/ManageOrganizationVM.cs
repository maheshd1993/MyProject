using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ViewModel
{
    public class ManageOrganizationVM
    {
        public int Id { get; set; }
        public int? AssignedId { get; set; }
        public int uid { get; set; }
        public string UserName { get; set; }
        public string EmployeeCode { get; set; }
        public string ProfileName  { get; set; }
        public string BranchName { get; set; }
        public int AssignToCompanyID { get; set; }
        //public int CompanyBranchID { get; set; }
        public int CompanyId { get; set; }
        public  int AssignedUserID { get; set; } 
        public string Address { get; set; }
        public string MobileNo  { get; set; }
        public string Country  { get; set; }
        public string CreateDate { get; set; }
        public string AssignedDate  { get; set; }
        public string AssignedBranchName { get; set; }
        public string State  { get; set; }
        public int TotalRecord { get; set; }
        public string City  { get; set; }
        public string EmailID  { get; set; }
        public string Term   { get; set; }
        public string FromDate  { get; set; }
        public string ToDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsChecked { get; set; }

        public List<ManageOrganizationVM> UserList  = new List<ManageOrganizationVM>();
        public IEnumerable<AssignToBranchModel> AssigntoList { get; set; }
    }

    public class AssignToBranchModel
    {
        public int Id  { get; set; }
        public string BranchName { get; set; }
    }
}