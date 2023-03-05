using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Models.ViewModel
{
    public class ViewUserModel
    {
        public Nullable<Int32> CompanyTypeID { get; set; }
        public String CompanyTypeName { get; set; }
        public int? FilterBranchId { get; set; }
        public SelectList BranchList { get; set; }
        public List<UsersModel> UsersList  { get; set; }
        public List<ViewUserModel> CompanyTypeList { get; set; }
    }

    public class UsersModel
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Designation { get; set; }
        public string EmployeeCode { get; set; }
        public string UserName { get; set; }
        public string ProfileName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public bool Status { get; set; }
        public string CompanyTypeName { get; set; }
    }

    public class Empcode
    {
        public string Id { get; set; }
        public string EmployeeCode { get; set; }

        public List<Empcode> empcodeList = new List<Empcode>();
    }
}