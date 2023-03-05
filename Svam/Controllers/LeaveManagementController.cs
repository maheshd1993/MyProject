using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Svam.EF;
using Traders.Models;
using System.Collections.Generic;
using Svam.Models;
using Svam.UtilityManager;

namespace Traders.Controllers
{
    public class LeaveManagementController : Controller
    {
        niscrmEntities db = new niscrmEntities();        

        public ActionResult LeaveRequest(Int64? RequestID)
        {
            LeaveRequestModel LRM = new LeaveRequestModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
               
                var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ProfileId != null).OrderBy(em => em.Fname).ToList();
                if (getEmployeeList != null)
                {
                    List<LeaveRequestModel> CUMEmployeeList = new List<LeaveRequestModel>();
                    foreach (var item in getEmployeeList)
                    {
                        LeaveRequestModel CUMEmployee = new LeaveRequestModel();
                        CUMEmployee.EmployeeID = item.Id;
                        CUMEmployee.FullName = item.Fname + ' ' + item.Lname;
                        CUMEmployeeList.Add(CUMEmployee);
                    }
                    LRM.EmployeeList = CUMEmployeeList;
                }

                //Call Only Employee is Login
                if (Convert.ToString(Session["UserType"]) != "SuperAdmin")
                {
                    int UID=Convert.ToInt32(Session["UID"]);
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
                    List<LeaveRequestModel> LeaveTypeList = new List<LeaveRequestModel>();
                    foreach (var item in geterrortypeList)
                    {
                        LeaveRequestModel cError = new LeaveRequestModel();
                        cError.LeaveTypeID = item.ID;
                        cError.LeaveName = item.LeaveName;
                        LeaveTypeList.Add(cError);
                    }
                    LRM.LeaveTypeList = LeaveTypeList;
                }

                if (RequestID > 0)
                {
                    var GetEmployeeLeave = db.crm_leaverequest_tbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.Id==RequestID).FirstOrDefault();
                    if (GetEmployeeLeave != null)
                    {
                        LRM.RequestID = GetEmployeeLeave.Id;
                        LRM.EmployeeID = GetEmployeeLeave.EmployeeID;
                        LRM.LeaveTypeID = GetEmployeeLeave.LeaveTypeID;
                        LRM.Subject = GetEmployeeLeave.Subject;
                        LRM.Message = GetEmployeeLeave.Message;
                        LRM.ProcessStatus = GetEmployeeLeave.ProcessStatus;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "There is some problem";
                return Redirect("/LeaveManagement/LeaveRequest");
            }
            return View(LRM);
        }


        [HttpPost]
        public ActionResult LeaveRequest(LeaveRequestModel LRM, Int32? RequestID)
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
                        getUpdateRecord.LeaveTypeID = LRM.LeaveTypeID;
                        getUpdateRecord.Subject = LRM.Subject;
                        getUpdateRecord.Message = LRM.Message;
                        getUpdateRecord.RequestDate = Constant.GetBharatTime();
                        db.SaveChanges();
                        TempData["success"] = "Leave updated successfully";
                        return Redirect("/LeaveManagement/LeaveRequestView");
                    }
                    else
                    {
                        TempData["alert"] = "There is some problem";
                        return Redirect("/LeaveManagement/LeaveRequest");
                    }
                }
                else
                {
                    crm_leaverequest_tbl CLM = new crm_leaverequest_tbl();
                    CLM.EmployeeID = LRM.EmployeeID;
                    CLM.LeaveTypeID = LRM.LeaveTypeID;
                    CLM.Subject = LRM.Subject;
                    CLM.Message = LRM.Message;
                    CLM.ProcessStatus = "In Process";
                    CLM.RequestDate = Constant.GetBharatTime();
                    CLM.CompanyID = CompanyID;
                    CLM.BranchID = BranchID;
                    db.crm_leaverequest_tbl.Add(CLM);
                    db.SaveChanges();
                    TempData["success"] = "Leave added successfully";
                    return Redirect("/LeaveManagement/LeaveRequestView");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);  
                TempData["alert"] = "There is some problem";
                return Redirect("/LeaveManagement/LeaveRequest");
            }           
        }

        public ActionResult LeaveRequestView(Int32? EmployeeID)
        {
            LeaveRequestModel LRM = new LeaveRequestModel();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            LRM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

            var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ProfileId != null).OrderBy(em => em.Fname).ToList();
            if (getEmployeeList != null)
            {
                List<LeaveRequestModel> CUMEmployeeList = new List<LeaveRequestModel>();
                foreach (var item in getEmployeeList)
                {
                    LeaveRequestModel CUMEmployee = new LeaveRequestModel();
                    CUMEmployee.EmployeeID = item.Id;
                    CUMEmployee.FullName = item.Fname + ' ' + item.Lname;
                    CUMEmployeeList.Add(CUMEmployee);
                }
                LRM.EmployeeList = CUMEmployeeList;
            }
            //DataTable getEmployeeLeaveRequest = DataAccessLayer.GetDataTable("call CRM_RequestLeaveList(" + BranchID + "," + CompanyID + ")");
            DataTable getEmployeeLeaveRequest = DataAccessLayer.GetDataTable("call CRM_LeaveRequestList_demo(" + BranchID + "," + CompanyID + ")");
            if (getEmployeeLeaveRequest.Rows.Count > 0)
            {
                List<LeaveRequestModel> LRMList = new List<LeaveRequestModel>();
                for (int i = 0; i < getEmployeeLeaveRequest.Rows.Count; i++)
                {
                    LeaveRequestModel LModel = new LeaveRequestModel();
                    LModel.EmployeeID = Convert.ToInt32(getEmployeeLeaveRequest.Rows[i]["EmployeeID"]);
                    LModel.LeaveName = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["LeaveName"]);
                    LModel.RequestID = Convert.ToInt64(getEmployeeLeaveRequest.Rows[i]["RequestID"]);
                    LModel.Subject = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["Subject"]);
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

        public ActionResult ViewComment(Int32? RequestID)
        {
            string ViewComment = string.Empty;
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var GetViewComment = db.crm_leaverequest_tbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.Id == RequestID).FirstOrDefault();
            if (GetViewComment != null)
            {
                ViewComment = GetViewComment.Comment;
            }

            return Json(ViewComment, JsonRequestBehavior.AllowGet);
        }
    }
}
