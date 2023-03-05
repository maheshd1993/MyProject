using Svam.EF;
using Svam.Models.DTO;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Traders.Models;

namespace Svam.Controllers
{
    public class TicketEscalationSchedularController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        public void RubJobSchduler()  
        {
            #region deafult date wise tickets data get
            var dd = Constant.GetBharatTime();
            DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
            DateTime MonthendDate = new DateTime(dd.Year,dd.Month,dd.Day);//monthStartDate.AddMonths(1).AddDays(-1);

            #endregion
            var ticketData= new List<TicketsEscalateDTO>();
            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetTicketEscalation('" + monthStartDate.ToString("dd/MM/yyyy") + "','" + MonthendDate.ToString("dd/MM/yyyy") + "')");
            if(GetRecords.Rows.Count>0)
            {
                 ticketData = (from dr in GetRecords.AsEnumerable()
                                              select new TicketsEscalateDTO()
                                              {
                                                  TicketID = Convert.ToInt32(dr["TicketID"]),
                                                  CreatedOn = Convert.ToDateTime(dr["CreatedOn"]),
                                                  CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                                                  //ModifiedBy = dr["ModifiedBy"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ModifiedBy"]),
                                                  ModifiedOn = dr["ModifiedOn"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedOn"]),
                                                  CompanyId = Convert.ToInt32(dr["CompanyId"]),
                                                  BranchID = Convert.ToInt32(dr["BranchID"]),
                                                  AssignedTo = dr["AssignedTo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AssignedTo"]),
                                                  AssignedDate = dr["AssignedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedDate"]),

                                              }).ToList();
            }
            
            //var ticketData = db.crm_tickets.Where(a => (DbFunctions.TruncateTime(a.CreatedOn)>= monthStartDate.Date && DbFunctions.TruncateTime(a.CreatedOn) <= MonthendDate.Date || DbFunctions.TruncateTime(a.AssignedDate) >= monthStartDate.Date && DbFunctions.TruncateTime(a.AssignedDate) <= MonthendDate.Date)&& a.StatusID==1 && a.CompanyId != null && a.BranchID != null).ToList();
            if(ticketData!=null && ticketData.Count>0)
            {
                foreach(var item in ticketData)
                {
                    if(item.AssignedTo>0 && !string.IsNullOrEmpty(item.AssignedDate))
                    {
                        //get esclate user by ticket assigned to user id
                        var escalateUser = db.crm_usertbl.Where(a => a.Id == item.AssignedTo && a.CompanyID == item.CompanyId && a.BranchID == item.BranchID).FirstOrDefault();
                        if(escalateUser!=null && escalateUser.EscalateUserId!=null && escalateUser.EscalateUserId>0 && escalateUser.EscalateTime>0 )
                        {
                            if(string.IsNullOrEmpty(item.ModifiedOn) )
                            {
                                TimeSpan diff = dd - Convert.ToDateTime(item.AssignedDate);
                                double hours = diff.TotalHours;//calculate total hours
                                if(hours> escalateUser.EscalateTime)//if hours greater then escalate time then ticket assigned to escalation user
                                {
                                    //item.AssignedBy = item.AssignedTo;
                                    //item.AssignedTo = escalateUser.EscalateUserId;
                                    //item.AssignedDate = dd;
                                    //db.SaveChanges();
                                    db.Database.ExecuteSqlCommand(@"update crm_tickets set AssignedBy="+ item.AssignedTo + ",AssignedTo="+ escalateUser.EscalateUserId + ",AssignedDate='"+ dd + "' where TicketID==" + item.TicketID);
                                }
                            }
                            else if (!string.IsNullOrEmpty(item.ModifiedOn))
                            {
                                var newModifiedDate = Convert.ToDateTime(item.ModifiedOn).AddHours(escalateUser.EscalateTime);//add esclate hour to get new modified date
                                //TimeSpan diff =  item.AssignedDate.Value- item.ModifiedOn.Value ;
                                //double hours = diff.TotalHours;//calculate total hours
                                if (Convert.ToDateTime(item.ModifiedOn).Date < newModifiedDate)//if modified date less then new modifed date then ticket assigned to escalation user
                                {
                                    //item.AssignedBy = item.AssignedTo;
                                    //item.AssignedTo = escalateUser.EscalateUserId;
                                    //item.AssignedDate = dd;
                                    //db.SaveChanges();
                                    db.Database.ExecuteSqlCommand(@"update crm_tickets set AssignedBy=" + item.AssignedTo + ",AssignedTo=" + escalateUser.EscalateUserId + ",AssignedDate='" + dd + "' where TicketID==" + item.TicketID);

                                }
                            }
                        }
                    }
                    else if (item.CreatedBy>0 && (item.AssignedTo == 0|| item.AssignedTo == null))
                    {
                        //get esclate user by ticket assigned to user id
                        var escalateUser = db.crm_usertbl.Where(a => a.Id == item.CreatedBy && a.CompanyID == item.CompanyId && a.BranchID == item.BranchID).FirstOrDefault();
                        if (escalateUser != null && escalateUser.EscalateUserId != null && escalateUser.EscalateUserId > 0 && escalateUser.EscalateTime > 0)
                        {
                            if (item.ModifiedOn == null)
                            {
                                TimeSpan diff = dd - Convert.ToDateTime(item.AssignedDate);
                                double hours = diff.TotalHours;//calculate total hours
                                if (hours > escalateUser.EscalateTime)//if hours greater then escalate time then ticket assigned to escalation user
                                {
                                    //item.AssignedBy = item.AssignedTo;
                                    //item.AssignedTo = escalateUser.EscalateUserId;
                                    //item.AssignedDate = dd;
                                    //db.SaveChanges();
                                    db.Database.ExecuteSqlCommand(@"update crm_tickets set AssignedBy=" + item.AssignedTo + ",AssignedTo=" + escalateUser.EscalateUserId + ",AssignedDate='" + dd + "' where TicketID==" + item.TicketID);

                                }
                            }
                            else if (item.ModifiedOn != null)
                            {
                                var newModifiedDate = Convert.ToDateTime(item.ModifiedOn).AddHours(escalateUser.EscalateTime);//add esclate hour to get new modified date
                                //TimeSpan diff =  item.AssignedDate.Value- item.ModifiedOn.Value ;
                                //double hours = diff.TotalHours;//calculate total hours
                                if (Convert.ToDateTime(item.ModifiedOn).Date < newModifiedDate)//if modified date less then new modifed date then ticket assigned to escalation user
                                {
                                    //item.AssignedBy = item.CreatedBy;
                                    //item.AssignedTo = escalateUser.EscalateUserId;
                                    //item.AssignedDate = dd;
                                    //db.SaveChanges();
                                    db.Database.ExecuteSqlCommand(@"update crm_tickets set AssignedBy=" + item.AssignedTo + ",AssignedTo=" + escalateUser.EscalateUserId + ",AssignedDate='" + dd + "' where TicketID==" + item.TicketID);

                                }
                            }
                        }
                    }
                }
            }
            //return View();
        }

    }
}
