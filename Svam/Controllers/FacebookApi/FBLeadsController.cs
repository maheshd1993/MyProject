using Facebook;
using Newtonsoft.Json;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Svam.Models.CRMLeadModel;

namespace Svam.Controllers.FacebookApi
{
    public class FBLeadsController : Controller
    {

        niscrmEntities db = new niscrmEntities();
        public async Task<ActionResult> GetFBLeads()
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            //int uid = Convert.ToInt32(Session["UID"]);
            var dt = Constant.GetBharatTime();

            try
            {
                var pageData = await db.crm_fbleadadsmaster.Where(a => a.CompanyID == CompanyID && a.BranchID == BranchID && a.IsActive == true).ToListAsync();
                if (pageData != null && pageData.Count > 0)
                {
                    foreach (var item in pageData)
                    {
                        var fb = new FacebookClient
                        {
                            AccessToken = item.Access_Token
                        };
                        //var json = fb.Get("page_id or ad_id/leadgen_forms");
                        var json = fb.Get(item.Page_Id + "/leadgen_forms?access_token=" + item.Access_Token);

                        var forms = JsonConvert.DeserializeObject<FormsLeads>(json.ToString());
                        if (forms != null && forms.data.Count() > 0 )
                        {
                            forms.data = forms.data.Where(a => a.status == "ACTIVE").ToArray();//get form only active

                            var getleadSourceStatus = await db.crm_leadsource_tbl.Where(em => em.LeadsourceName == "Facebook" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                            var getStatus = await db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == "Open" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                            //if (getStatus != null)
                            //{
                                foreach (var form in forms.data/*.Where(a => a.status == "ACTIVE")*/)
                                {
                                    var jsonre = fb.Get(form.id + "/leads?access_token=" + item.Access_Token);//get leads by form id
                                    var leads = JsonConvert.DeserializeObject<Leads>(jsonre.ToString());
                                    if (leads != null && leads.data.Count() > 0 && leads.data.Any(a => a.created_time.Date >= dt.Date))
                                    {
                                        leads.data = leads.data.Where(a => a.created_time.Date >= dt.Date).ToArray();
                                    }
                                    else
                                    {
                                        leads.paging = null;
                                        //leads = null;
                                    }
                                    while (leads.paging != null)
                                    {
                                        foreach (var lead in leads.data/*.Where(a => a.created_time.Date >= dt.Date)*/)
                                        {
                                        //string Mobno = string.Empty;
                                        //string customer = string.Empty;
                                        //string email = string.Empty;
                                        //string city = string.Empty;
                                        //string state = string.Empty;
                                        //string country = string.Empty;

                                        LeadModel leadModel = new LeadModel();

                                        
                                        if (lead.field_data.Any(a=>a.name== "full_name"))
                                        {
                                            leadModel.Customer = lead.field_data.Where(a=>a.name=="full_name").SelectMany(a=>a.values).FirstOrDefault();
                                        }
                                        if (lead.field_data.Any(a => a.name == "enter_your_whatsapp_number"))
                                        {
                                            leadModel.WhatsAppNo = lead.field_data.Where(a => a.name == "enter_your_whatsapp_number").SelectMany(a => a.values).FirstOrDefault();
                                        }
                                        if (lead.field_data.Any(a => a.name == "email"))
                                        {
                                            leadModel.Email = lead.field_data.Where(a => a.name == "email").SelectMany(a => a.values).FirstOrDefault();
                                        }
                                        if (lead.field_data.Any(a => a.name == "phone_number"))
                                        {
                                            leadModel.Mobno = lead.field_data.Where(a => a.name == "phone_number").SelectMany(a => a.values).FirstOrDefault();
                                        }
                                        if (lead.field_data.Any(a => a.name == "city"))
                                        {
                                            leadModel.City = lead.field_data.Where(a => a.name == "city").SelectMany(a => a.values).FirstOrDefault();
                                        }
                                        if (lead.field_data.Any(a => a.name == "state"))
                                        {
                                            leadModel.State = lead.field_data.Where(a => a.name == "state").SelectMany(a => a.values).FirstOrDefault();
                                        }
                                        if (lead.field_data.Any(a => a.name == "country"))
                                        {
                                            leadModel.Country = lead.field_data.Where(a => a.name == "country").SelectMany(a => a.values).FirstOrDefault();
                                        }
                                        if (lead.field_data.Any(a => a.name == "company_name"))
                                        {
                                            leadModel.CompanyName = lead.field_data.Where(a => a.name == "company_name").SelectMany(a => a.values).FirstOrDefault();
                                        }
                                        if (lead.field_data.Any(a => a.name == "job_title"))
                                        {
                                            leadModel.JobTitle = lead.field_data.Where(a => a.name == "job_title").SelectMany(a => a.values).FirstOrDefault();
                                        }
                                        //foreach (var t in lead.field_data)
                                        //{
                                        //    if (t.name == "full_name")
                                        //    {
                                        //        leadModel.Customer = t.values.FirstOrDefault();
                                        //    }
                                        //    else if (t.name == "email")
                                        //    {
                                        //        leadModel.Email = t.values.FirstOrDefault();
                                        //    }
                                        //    else if (t.name == "phone_number")
                                        //    {
                                        //        leadModel.Mobno = t.values.FirstOrDefault();
                                        //    }
                                        //    else if (t.name == "city")
                                        //    {
                                        //        leadModel.City = t.values.FirstOrDefault();
                                        //    }
                                        //    else if (t.name == "state")
                                        //    {
                                        //        leadModel.State = t.values.FirstOrDefault();
                                        //    }
                                        //    else if (t.name == "country")
                                        //    {
                                        //        leadModel.Country = t.values.FirstOrDefault();
                                        //    }
                                        //}


                                        //bool numeronly = Regex.IsMatch(Mobno.Replace("+91", ""), @"^[0][1-9]\d{9}$|^[1-9]\d{9}$");
                                        //bool numeronly = true;
                                        if (leadModel != null && !string.IsNullOrEmpty(leadModel.Mobno) && !string.IsNullOrEmpty(leadModel.Customer))
                                            {
                                            //foreach(var item1  in ldList)
                                            //{
                                            //if (leadModel.Mobno.Contains("+91"))
                                            //{
                                            //    leadModel.Mobno = leadModel.Mobno.Replace("+91", "");
                                            //}
                                            if (leadModel.Mobno.Length < 8 || leadModel.Mobno.Length == 8)//check mobile no length is less then 8 or equal 8 then add 00 for 9 digit
                                            {
                                                leadModel.Mobno = "00" + leadModel.Mobno;
                                            }
                                           
                                            string Mobno1 = leadModel.Mobno.Substring(leadModel.Mobno.Length - 9, 9);//get last line digits 
                                            var Getexists = await db.crm_createleadstbl.Where(em => (em.MobileNo.Substring(em.MobileNo.Length - 9, 9) == Mobno1) && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                                                if (Getexists != null)
                                                {
                                                    var cDate = Getexists.Createddate.Value.Date;
                                                    if (cDate!=dt.Date)
                                                    {
                                                        var getStatus2 = await db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == "Reinquiry" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();

                                                        Getexists.LeadStatusID = getStatus2 != null ? getStatus2.Id : Getexists.LeadStatusID;
                                                        Getexists.LeadStatus = getStatus2 != null ? getStatus2.LeadStatusName : Getexists.LeadStatus;
                                                       
                                                        Getexists.FollowDate = DateTime.ParseExact(dt.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                        await db.SaveChangesAsync();
                                                    }
                                                }
                                                else
                                                {
                                                    crm_createleadstbl CL = new crm_createleadstbl();

                                                    var getState = new com_state();
                                                    var getCity = new com_city();
                                                    var GetCountry = new acc_countries();
                                                    if (!string.IsNullOrEmpty(leadModel.Country))
                                                    {
                                                        GetCountry = await db.acc_countries.Where(em => (em.country_name.ToLower() == leadModel.Country.ToLower() || em.country_code.ToLower() == leadModel.Country.ToLower())).FirstOrDefaultAsync();
                                                    }
                                                    if (!string.IsNullOrEmpty(leadModel.State))
                                                    {
                                                        if (GetCountry != null && GetCountry.id == 1)
                                                        {
                                                            getState = await db.com_state.Where(em => em.State.Substring(3).ToLower() == leadModel.State.ToLower()).FirstOrDefaultAsync();
                                                        }
                                                        else
                                                        {
                                                            getState = await db.com_state.Where(em => em.State.ToLower() == leadModel.State.ToLower()).FirstOrDefaultAsync();
                                                        }
                                                    }

                                                    if (!string.IsNullOrEmpty(leadModel.City))
                                                    {
                                                        getCity = await db.com_city.Where(em => em.City.ToLower() == leadModel.City.ToLower()).FirstOrDefaultAsync();

                                                         if(getCity!=null && !string.IsNullOrEmpty(getCity.Country) && GetCountry == null)//if coutry null then get countryid from getcity table to get country
                                                         {
                                                           int countryId =Convert.ToInt32(getCity.Country);
                                                           GetCountry = await db.acc_countries.Where(em => em.id== countryId).FirstOrDefaultAsync();
                                                         }

                                                    if (getCity != null && !string.IsNullOrEmpty(getCity.State) && getState == null)//if getState null then get state id from getcity table to get State
                                                    {
                                                        int stateId  = Convert.ToInt32(getCity.State);
                                                        getState = await db.com_state.Where(em => em.ID == stateId).FirstOrDefaultAsync();
                                                    }
                                                }
                                                    CL.LeadOwner = item.CreatedBy;
                                                    CL.MobileNo = leadModel.Mobno.Trim();
                                                    CL.Customer = leadModel.Customer.Trim();
                                                    CL.EmailId = leadModel.Email;
                                                    CL.LeadStatusID = getStatus != null ? getStatus.Id : 0;
                                                    CL.LeadStatus = getStatus != null ? getStatus.LeadStatusName : string.Empty;
                                                    CL.LeadSourceID = getleadSourceStatus == null ? 0 : getleadSourceStatus.Id;
                                                    CL.LeadResource = getleadSourceStatus == null ? string.Empty : getleadSourceStatus.LeadsourceName;
                                                    CL.FollowDate = DateTime.ParseExact(dt.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                    CL.date = dt.ToString("dd/MM/yyyy");
                                                    CL.Createddate = dt;
                                                    CL.Status = true;
                                                    CL.LeadsType = "Facebook";
                                                    CL.CountryID = GetCountry != null ? GetCountry.id : 0;
                                                    CL.StateID = getState != null ? getState.ID : 0;
                                                    CL.CityID = getCity != null ? getCity.ID : 0;
                                                    CL.CompanyID = CompanyID;
                                                    CL.BranchID = BranchID;
                                                    CL.OrganizationName = leadModel.CompanyName;
                                                    CL.Designation = leadModel.JobTitle.Length > 150 ? leadModel.JobTitle.Substring(0, 100) : leadModel.JobTitle;//get jobtitle if Length>150 then substring for 100 Length
                                                CL.ExtraCol1 = leadModel.WhatsAppNo;
                                                //CL.FollowUpTime = dt.ToString("hh:mm tt"); //set followup time
                                                //CL.ZoneName = Constant.GetCompanyTimeZone(CompanyID);
                                                //CL.ConvertedFupDateTime = dt;
                                                db.crm_createleadstbl.Add(CL);
                                                    await db.SaveChangesAsync();
                                                }
                                                //}


                                            }

                                        }
                                        if (leads.paging != null)
                                        {
                                            jsonre = fb.Get(form.id + "/leads?after=" + leads.paging.cursors.after);
                                            leads = JsonConvert.DeserializeObject<Leads>(jsonre.ToString());
                                            if(leads!=null && leads.data.Count()>0 && leads.data.Any(a => a.created_time.Date >= dt.Date))
                                            {                                               
                                               leads.data = leads.data.Where(a => a.created_time.Date >= dt.Date).ToArray();
                                            }
                                            else
                                            {
                                                leads.paging =null;
                                                //leads = null;
                                            }
                                        }
                                    }
                                }
                            //}

                        }//if end
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
            
            return Json("ok",JsonRequestBehavior.AllowGet);
        }

    }
}
