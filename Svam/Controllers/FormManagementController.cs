using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Svam.EF;
using Traders.Models;
using System.Collections.Generic;
using Svam.Models;
using Svam.UtilityManager;
using Svam._Classes;
using System.Net.Http;

namespace Svam.Controllers
{
    public class FormManagementController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        //
        // GET: /FormManagement/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form16Request(Int64? RequestID)
        {
            Form16RequestModel LRM = new Form16RequestModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ProfileId != null).OrderBy(em => em.Fname).ToList();
                if (getEmployeeList != null)
                {
                    List<Form16RequestModel> CUMEmployeeList = new List<Form16RequestModel>();
                    foreach (var item in getEmployeeList)
                    {
                        Form16RequestModel CUMEmployee = new Form16RequestModel();
                        CUMEmployee.EmployeeID = item.Id;
                        CUMEmployee.FullName = item.Fname + ' ' + item.Lname;
                        CUMEmployeeList.Add(CUMEmployee);
                    }
                    LRM.EmployeeList = CUMEmployeeList;
                }

                //Call Only Employee is Login
                if (Convert.ToString(Session["UserType"]) != "SuperAdmin")
                {
                    int UID = Convert.ToInt32(Session["UID"]);
                    LRM.EmployeeList = LRM.EmployeeList.Where(em => em.EmployeeID == UID).ToList();
                }

                //List<LeaveRequestModel> LeaveTypeList = new List<LeaveRequestModel>
                //{
                //    new LeaveRequestModel { LeaveTypeID =1, LeaveName = "Casual Leave" },
                //    new LeaveRequestModel { LeaveTypeID =2, LeaveName = "Medical Leave" }
                //};
                //LRM.LeaveTypeList = LeaveTypeList;

                var geterrortypeList = db.crm_leavetypename.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                if (geterrortypeList.Count > 0)
                {
                    List<Form16RequestModel> FormTypeList = new List<Form16RequestModel>();
                    //foreach (var item in geterrortypeList)
                    //{
                        Form16RequestModel cError = new Form16RequestModel();
                        cError.FormTypeID = 1;
                        cError.FormName = "Form16";
                        FormTypeList.Add(cError);
                   // }
                    LRM.FormTypeList = FormTypeList;
                }

                //if (RequestID > 0)
                //{
                //    var GetEmployeeLeave = db.crm_leaverequest_tbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.Id == RequestID).FirstOrDefault();
                //    if (GetEmployeeLeave != null)
                //    {
                //        LRM.RequestID = GetEmployeeLeave.Id;
                //        LRM.EmployeeID = GetEmployeeLeave.EmployeeID;
                //        LRM.LeaveTypeID = GetEmployeeLeave.LeaveTypeID;
                //        LRM.Subject = GetEmployeeLeave.Subject;
                //        LRM.Message = GetEmployeeLeave.Message;
                //        LRM.ProcessStatus = GetEmployeeLeave.ProcessStatus;
                //    }
                //}
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "There is some problem";
                return Redirect("/FormManagement/Form16Request");
            }
            return View(LRM);
        }

        apiclasses apiclass = new apiclasses();
       
        [HttpPost]
        public ActionResult Form16Request(Form16RequestModel LRM, Int32? RequestID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            try
            {
                if (RequestID > 0)
                {
                    var getUpdateRecord = db.crm_leaverequest_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id == RequestID).FirstOrDefault();
                    if (getUpdateRecord != null)
                    {
                        getUpdateRecord.EmployeeID = LRM.EmployeeID;
                        getUpdateRecord.LeaveTypeID = LRM.FormTypeID;
                        //getUpdateRecord.Subject = LRM.Subject;
                        //getUpdateRecord.Message = LRM.Message;
                        getUpdateRecord.RequestDate = Constant.GetBharatTime();
                      //  db.SaveChanges();
                        TempData["success"] = "Leave updated successfully";
                        return Redirect("/FormManagement/Form16RequestView");
                    }
                    else
                    {
                        TempData["alert"] = "There is some problem";
                        return Redirect("/FormManagement/Form16Request");
                    }
                }
                else
                {
                    crm_formrequest_tbl CLM = new crm_formrequest_tbl();
                    CLM.EmployeeID = LRM.EmployeeID;
                    CLM.FormTypeID = LRM.FormTypeID;
                    //  CLM.Subject = LRM.Subject;
                    CLM.FileName = "";
                    CLM.FilePath = "";
                    CLM.Message = LRM.Message;
                    CLM.ProcessStatus = "In Process";
                    CLM.RequestDate = Constant.GetBharatTime();
                    CLM.CompanyID = CompanyID;
                    CLM.BranchID = BranchID;
                    db.crm_formrequest_tbl.Add(CLM);
                    db.SaveChanges();
                    TempData["success"] = "Form16 added successfully";
                    return Redirect("/FormManagement/Form16RequestView");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "There is some problem";
                return Redirect("/FormManagement/Form16RequestView");
            }
        }
        public ActionResult Form16RequestView(Int32? EmployeeID)
        {
            Form16RequestModel LRM = new Form16RequestModel();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            LRM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

            var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ProfileId != null).OrderBy(em => em.Fname).ToList();
            if (getEmployeeList != null)
            {
                List<Form16RequestModel> CUMEmployeeList = new List<Form16RequestModel>();
                foreach (var item in getEmployeeList)
                {
                    Form16RequestModel CUMEmployee = new Form16RequestModel();
                    CUMEmployee.EmployeeID = item.Id;
                    CUMEmployee.FullName = item.Fname + ' ' + item.Lname;
                    CUMEmployeeList.Add(CUMEmployee);
                }
                LRM.EmployeeList = CUMEmployeeList;
            }
            //DataTable getEmployeeLeaveRequest = DataAccessLayer.GetDataTable("call CRM_RequestLeaveList(" + BranchID + "," + CompanyID + ")");
            DataTable getEmployeeLeaveRequest = DataAccessLayer.GetDataTable("call CRM_Form16RequestList_demo(" + BranchID + "," + CompanyID + ")");
            if (getEmployeeLeaveRequest.Rows.Count > 0)
            {
                List<Form16RequestModel> LRMList = new List<Form16RequestModel>();
                for (int i = 0; i < getEmployeeLeaveRequest.Rows.Count; i++)
                {
                    Form16RequestModel LModel = new Form16RequestModel();
                    LModel.EmployeeID = Convert.ToInt32(getEmployeeLeaveRequest.Rows[i]["EmployeeID"]);
                   // LModel.LeaveName = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["LeaveName"]);
                    LModel.RequestID = Convert.ToInt64(getEmployeeLeaveRequest.Rows[i]["RequestID"]);
                   // LModel.Subject = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["Subject"]);
                    LModel.ProcessStatus = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["ProcessStatus"]);
                    LModel.RequestDate = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["RequestDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                    if (!String.IsNullOrWhiteSpace(Convert.ToString(getEmployeeLeaveRequest.Rows[i]["ProcessDate"])))
                    {
                        LModel.ProcessDate = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["ProcessDate"]);
                    }
                    else
                    {
                        LModel.ProcessDate = string.Empty;
                    }
                    LModel.EmployeeCode = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["EmployeeCode"]);
                    LModel.FullName = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["FullName"]);
                    LRMList.Add(LModel);
                }
                LRM.LeaveEmployeeList = LRMList.OrderByDescending(em => em.RequestDate).ToList();

                if (LRM.LeaveEmployeeList.Count > 0)
                {
                    if (Convert.ToString(Session["UserType"]) != "SuperAdmin" && Convert.ToString(Session["UserType"]) != "")
                    {
                        Int32 UID = Convert.ToInt32(Session["UID"]);
                        LRM.LeaveEmployeeList = LRM.LeaveEmployeeList.Where(em => em.EmployeeID == UID).ToList();
                    }

                    if (EmployeeID > 0 && Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        LRM.LeaveEmployeeList = LRM.LeaveEmployeeList.Where(em => em.EmployeeID == EmployeeID).ToList();
                    }
                }
            }


            return View(LRM);
        }
    }
}
