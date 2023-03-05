using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Svam.EF;
using System.Globalization;
using Svam.Models;
using System.Text.RegularExpressions;
using Svam.UtilityManager;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Text;

namespace Traders.Controllers
{
    public class ExcelImportExport
    {
        #region Import Data
        public static string Import_Interview(string FilePath, string Extension, string isHDR, int uid, Int32 BranchID, Int32 CompanyID)
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);

            var getMsg = ImportInterView(dt, uid, BranchID, CompanyID);
            connExcel.Close();
            return getMsg;
        }

        public static string ImportInterView(DataTable dt, int uid, Int32 BranchID, Int32 CompanyID)
        {
            var msg = "";
            try
            {
                niscrmEntities db = new niscrmEntities();
                #region SaveData-in Database
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    var email = Convert.ToString(dr["Email"]);
                    var CheckExistUser = db.crm_interviewscheduletbl.Where(em => em.Email == email && em.BranchID == BranchID && em.CompanyID == CompanyID).SingleOrDefault();
                    if (CheckExistUser == null)
                    {
                        #region Add-InterView
                        crm_interviewscheduletbl ISt = new crm_interviewscheduletbl();
                        ISt.UID = uid;
                        ISt.CandidateName = Convert.ToString(dr["Name of the Candidate"]);
                        ISt.ResumeId = Convert.ToString(dr["Resume ID"]);
                        ISt.PostalAddress = Convert.ToString(dr["Postal Address"]);
                        var tlp = Convert.ToString(dr["Telephone No#"]);
                        if (tlp != null && tlp != "")
                        {
                            var Telephone = tlp.Replace("'", "");
                            ISt.Telephone = Telephone;
                        }

                        var Mb = Convert.ToString(dr["Mobile No#"]);
                        if (Mb != null && Mb != "")
                        {
                            var Mobile = Mb.Replace("'", "");
                            ISt.MobileNo = Mobile;
                        }

                        ISt.DateOfBirth = Convert.ToString(dr["Date of Birth"]);
                        ISt.Email = Convert.ToString(dr["Email"]);
                        ISt.WorkExperiance = Convert.ToString(dr["Work Experience"]);
                        ISt.ResumeTitle = Convert.ToString(dr["Resume Title"]);
                        ISt.CurrentLocation = Convert.ToString(dr["Current Location"]);
                        ISt.PreferredLocation = Convert.ToString(dr["Preferred Location"]);
                        ISt.CurrentEmployer = Convert.ToString(dr["Current Employer"]);
                        ISt.CurrentDesignation = Convert.ToString(dr["Current Designation"]);
                        ISt.AnnualSalary = Convert.ToString(dr["Annual Salary"]);
                        ISt.UGCourses = Convert.ToString(dr["U#G# Course"]);
                        ISt.PGCourses = Convert.ToString(dr["P# G# Course"]);
                        ISt.PPGCourses = Convert.ToString(dr["P#P#G# Course"]);
                        ISt.LastActiveDate = Convert.ToString(dr["Last Active Date"]);
                        ISt.CreateDate = Constant.GetBharatTime().ToString("MM/dd/yyyy");
                        ISt.Created_at = Constant.GetBharatTime();
                        db.crm_interviewscheduletbl.Add(ISt);
                        db.SaveChanges();
                        i++;
                        #endregion
                    }
                }
                msg = i + " Records addedd successfully";
                #endregion

                return msg;

            }
            catch (Exception eSave)
            {
                msg = eSave.Message.ToString();
                return msg;
            }
        }

        public static string CRMFB_Import_To_Grid(string FilePath, string Extension, string isHDR, int uid, Int32 BranchID, Int32 CompanyID)
        {
            String conStr = "";
            //if (Extension == ".xls")
            //{
            //    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
            //}
            //else if (Extension == ".xlsx")
            //{
            //    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;ImportMixedType=Text;TypeGuessRows=0\"";
            //}
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            var getMsg = FBImportLeads(dt, uid, BranchID, CompanyID);
            connExcel.Close();
            return getMsg.Result;
        }

        public static async Task<string> FBImportLeads(DataTable dt, int uid, Int32 BranchID, Int32 CompanyID)
        {
            string msg = string.Empty;
            string errormsg = string.Empty;
            int row = 2;
            try
            {
                niscrmEntities db = new niscrmEntities();
                var todayDate = Constant.GetBharatTime();
                var m = db.crm_leadsource_tbl.Count();
                var getleadSourceStatus = await db.crm_leadsource_tbl.FirstOrDefaultAsync(em => em.LeadsourceName == "Facebook" && em.BranchID == BranchID && em.CompanyID == CompanyID);
                var getStatus = await db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == "Open" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                //if(getStatus!=null)
                //{
                foreach (DataRow dr in dt.Rows)
                {
                    var Mobno = Convert.ToString(dr["phone_number"]);
                    var customer = Convert.ToString(dr["full_name"]);
                    var email = Convert.ToString(dr["email"]);
                    string city = string.Empty;
                    string state = string.Empty;
                    string country = string.Empty;
                    string whatsappNo = string.Empty;
                    string companyName = string.Empty;
                    string jobTitle = string.Empty;

                    DataColumnCollection columns = dt.Columns;
                    if (columns.Contains("city"))
                    {
                        city = Convert.ToString(dr["city"]);
                    }
                    if (columns.Contains("state"))
                    {
                        state = Convert.ToString(dr["state"]);
                    }
                    if (columns.Contains("country"))
                    {
                        country = Convert.ToString(dr["country"]);
                    }
                    if (columns.Contains("enter_your_whatsapp_number"))
                    {
                        whatsappNo = Convert.ToString(dr["enter_your_whatsapp_number"]);
                    }
                    if (columns.Contains("company_name"))
                    {
                        companyName = Convert.ToString(dr["company_name"]);
                    }
                    if (columns.Contains("job_title"))
                    {
                        jobTitle = Convert.ToString(dr["job_title"]);
                    }

                    //if (Mobno.Contains("+91"))
                    //    {
                    //        Mobno = Mobno.Replace("+91", "");
                    //    }

                    //bool numeronly = Regex.IsMatch(Mobno.Replace("+91", ""), @"^[0][1-9]\d{9}$|^[1-9]\d{9}$");

                    if (!string.IsNullOrEmpty(Mobno) && !string.IsNullOrEmpty(customer))
                    {
                        if (Mobno.Length < 8 || Mobno.Length == 8)//check mobile no length is less then 8 or equal 8 then add 00 for 9 digit
                        {
                            Mobno = "00" + Mobno;
                        }
                        string Mobno1 = Mobno.Substring(Mobno.Length - 9, 9);//get last line digits 
                        var Getexists = await db.crm_createleadstbl.Where(em => (!string.IsNullOrEmpty(em.MobileNo) && em.MobileNo.Substring(em.MobileNo.Length - 9, 9) == Mobno1) && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                        if (Getexists != null)
                        {
                            var cDate = Getexists.Createddate.Value.Date;
                            if (cDate != todayDate.Date)
                            {
                                //var getStatus2 = await db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == "Reinquiry" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();

                                //Getexists.LeadStatusID = getStatus2 != null ? getStatus2.Id : Getexists.LeadStatusID;
                                //Getexists.LeadStatus = getStatus2 != null ? getStatus2.LeadStatusName : Getexists.LeadStatus;

                                //Getexists.FollowDate = DateTime.ParseExact(todayDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                //Getexists.IsLeadReminder = true;
                                await db.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            crm_createleadstbl CL = new crm_createleadstbl();

                            var getState = new com_state();
                            var getCity = new com_city();
                            var GetCountry = new acc_countries();
                            if (!string.IsNullOrEmpty(country))
                            {
                                GetCountry = await db.acc_countries.Where(em => (em.country_name.ToLower() == country.ToLower() || em.country_code.ToLower() == country.ToLower())).FirstOrDefaultAsync();
                            }
                            if (!string.IsNullOrEmpty(state))
                            {
                                if (GetCountry != null && GetCountry.id == 1)
                                {
                                    getState = await db.com_state.Where(em => em.State.Substring(3).ToLower() == state.ToLower()).FirstOrDefaultAsync();
                                }
                                else
                                {
                                    getState = await db.com_state.Where(em => em.State.ToLower() == state.ToLower()).FirstOrDefaultAsync();
                                }
                            }

                            if (!string.IsNullOrEmpty(city))
                            {
                                getCity = await db.com_city.Where(em => em.City.ToLower() == city.ToLower()).FirstOrDefaultAsync();

                                if (getCity != null && !string.IsNullOrEmpty(getCity.Country) && GetCountry == null)//if coutry null then get countryid from getcity table to get country
                                {
                                    int countryId = Convert.ToInt32(getCity.Country);
                                    GetCountry = await db.acc_countries.Where(em => em.id == countryId).FirstOrDefaultAsync();
                                }

                                if (getCity != null && !string.IsNullOrEmpty(getCity.State) && getState == null)//if getState null then get state id from getcity table to get State
                                {
                                    int stateId = Convert.ToInt32(getCity.State);
                                    getState = await db.com_state.Where(em => em.ID == stateId).FirstOrDefaultAsync();
                                }
                            }

                            CL.LeadOwner = uid;
                            CL.MobileNo = Mobno.Trim();
                            CL.Customer = customer.Trim();
                            CL.EmailId = email;
                            CL.LeadStatusID = getStatus != null ? getStatus.Id : 0;
                            CL.LeadStatus = getStatus != null ? getStatus.LeadStatusName : string.Empty;
                            CL.LeadSourceID = getleadSourceStatus == null ? 0 : getleadSourceStatus.Id;
                            CL.LeadResource = getleadSourceStatus == null ? string.Empty : getleadSourceStatus.LeadsourceName;
                            CL.FollowDate = DateTime.ParseExact(todayDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            CL.date = todayDate.ToString("dd/MM/yyyy");
                            CL.Createddate = todayDate;
                            CL.Status = true;
                            CL.LeadsType = "Facebook";
                            CL.CountryID = GetCountry != null ? GetCountry.id : 0;
                            CL.StateID = getState != null ? getState.ID : 0;
                            CL.CityID = getCity != null ? getCity.ID : 0;
                            CL.CompanyID = CompanyID;
                            CL.BranchID = BranchID;
                            CL.OrganizationName = companyName;
                            CL.Designation = jobTitle.Length > 150 ? jobTitle.Substring(0, 100) : jobTitle;//get jobtitle if Length>150 then substring for 100 Length
                            CL.ExtraCol1 = whatsappNo;
                            //CL.FollowUpTime= todayDate.ToString("hh:mm tt"); //set followup time
                            //CL.ZoneName = Constant.GetCompanyTimeZone(CompanyID);
                            //CL.ConvertedFupDateTime = todayDate;
                            CL.IsLeadReminder = true;
                            db.crm_createleadstbl.Add(CL);
                            await db.SaveChangesAsync();
                        }

                        //else
                        //{
                        //    errormsg = "** Facebook lead upload from excel is already exists.";
                        //}
                    }//if mobile no and customer not null end
                    else
                    {
                        if (string.IsNullOrEmpty(Mobno) && string.IsNullOrEmpty(customer))
                        {
                            errormsg = "Full name and phone number is blank in row no.:" + row;
                        }
                        else if (string.IsNullOrEmpty(Mobno) && !string.IsNullOrEmpty(customer))
                        {
                            errormsg = "Phone number is blank in row no.:" + row;
                        }
                        else if (!string.IsNullOrEmpty(Mobno) && string.IsNullOrEmpty(customer))
                        {
                            errormsg = "Full name is blank in row no.:" + row;
                        }
                        break;// get out of the loop
                    }
                    row++;
                }
                //}
                //else
                //{
                //    msg = "Lead status not found";
                //}


                if (errormsg == string.Empty)
                {
                    msg = "Facebook uploaded successfully";
                }
                else
                {
                    msg = errormsg;
                }
                return msg;
            }
            catch (Exception eSave)
            {
                msg = eSave.Message.ToString();
                ExceptionLogging.SendExcepToDB(eSave);
                return msg + ", Row no.:" + row;
            }
        }

        public static string CRM_Import_To_Grid(string FilePath, string Extension, string isHDR, int uid, Int32 BranchID, Int32 CompanyID)
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            var getMsg = ImportLeads(dt, uid, BranchID, CompanyID);
            connExcel.Close();
            return getMsg.Result;
        }

        public static async Task<string> ImportLeads(DataTable dt, int uid, Int32 BranchID, Int32 CompanyID)
        {
            niscrmEntities db = new niscrmEntities();
            string msg = string.Empty;
            string errormsg = string.Empty;
            int row = 0;
            //using (var trans = db.Database.BeginTransaction())
            //{
            try
            {
                var todayDate = Constant.GetBharatTime();
                string FullName = string.Empty;
                var getUserName = await db.crm_usertbl.Where(em => em.Id == uid && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                if (getUserName != null)
                {
                    FullName = string.Format("{0} {1}", getUserName.Fname, getUserName.Lname);
                }
                bool isExist = false;
                bool isInsertOne = false;
                var sb = new StringBuilder();

                foreach (DataRow dr in dt.Rows)
                {
                    string Mobno = Convert.ToString(dr["Phone Number"]);
                    string customer = Convert.ToString(dr["Customer Name"]);
                    string organizationname = Convert.ToString(dr["Organization Name"]);
                    string email = string.Empty;
                    string fdate = string.Empty;
                    string uploadDescription = string.Empty;
                    string FollowUpDate = string.Empty;
                    string LeadStatus = string.Empty;
                    bool checkdate = false;
                    //DataColumnCollection columns = dt.Columns;
                    //if (columns.Contains("Email"))
                    //{
                    email = Convert.ToString(dr["Email"]);
                    //}
                    //if (columns.Contains("Follow Up Date"))
                    //{
                    fdate = Convert.ToString(dr["Follow Up Date"]);
                    //}
                    //if (columns.Contains("Description"))
                    //{
                    uploadDescription = Convert.ToString(dr["Description"]);
                    //}

                    DateTime datetime;
                    if (!string.IsNullOrEmpty(fdate))
                    {
                        checkdate = DateTime.TryParseExact(fdate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out datetime);

                        if (checkdate == true)
                        {
                            FollowUpDate = fdate;
                        }
                        else
                        {
                            DateTime dDate;
                            if (DateTime.TryParse(fdate, out dDate))
                            {
                                fdate = dDate.ToString("dd/MM/yyyy");
                                FollowUpDate = fdate;
                            }
                        }
                    }

                    //if (columns.Contains("Lead Status"))
                    //{
                    LeadStatus = Convert.ToString(dr["Lead Status"]);
                    //}

                    if (!string.IsNullOrEmpty(customer) && !string.IsNullOrEmpty(Mobno))
                    {
                        //if (Mobno.Contains("+91"))
                        //{
                        //    Mobno = Mobno.Replace("+91", "");
                        //}

                        //String DuplicateRecord = String.Empty;
                        //bool numeronly = true;
                        //bool numeronly = Regex.IsMatch(Mobno.Replace("+91", ""), @"^[0][1-9]\d{9}$|^[1-9]\d{9}$");
                        //if (numeronly == true)
                        //{
                        //var getStatus = db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == StatusName && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        //if (getStatus != null)
                        //{

                        if (Mobno.Length < 8 || Mobno.Length == 8)//check mobile no length is less then 8 or equal 8 then add 00 for 9 digit
                        {
                            Mobno = "00" + Mobno;
                        }
                        string Mobno1 = Mobno.Substring(Mobno.Length - 9, 9);//get last line digits 
                        var Getexists = await db.crm_createleadstbl.Where(em => (!string.IsNullOrEmpty(em.MobileNo) && em.MobileNo.Substring(em.MobileNo.Length - 9, 9) == Mobno1) && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                        if (Getexists == null)
                        {
                            //string StatusName = LeadStatus;
                            string LeadSource = Convert.ToString(dr["Lead Source"]);
                            string ProductType = Convert.ToString(dr["Product Type"]);
                            string Country = Convert.ToString(dr["Country"]);
                            string State = Convert.ToString(dr["State"]);
                            string City = Convert.ToString(dr["City"]);

                            var GetCountry = new acc_countries();
                            var getState = new com_state();
                            var getCity = new com_city();
                            var getLeadSource = new crm_leadsource_tbl();
                            var getProductType = new crm_producttypetbl();
                            var getStatus = new crm_leadstatus_tbl();
                            //Country = Country == "" ? "India" : Country;
                            if (!string.IsNullOrEmpty(Country))
                            {
                                GetCountry = await db.acc_countries.Where(em => (em.country_name.ToLower() == Country.ToLower() || em.country_code.ToLower() == Country.ToLower())).FirstOrDefaultAsync();
                            }

                            if (!string.IsNullOrEmpty(State))
                            {
                                if (GetCountry != null && GetCountry.id == 1)
                                {
                                    getState = await db.com_state.Where(em => em.State.Substring(3).ToLower() == State.ToLower()).FirstOrDefaultAsync();
                                }
                                else
                                {
                                    getState = await db.com_state.Where(em => em.State.ToLower() == State.ToLower()).FirstOrDefaultAsync();
                                }
                            }

                            if (!string.IsNullOrEmpty(City))
                            {
                                getCity = await db.com_city.Where(em => em.City.ToLower() == City.ToLower()).FirstOrDefaultAsync();

                                if (getCity != null && !string.IsNullOrEmpty(getCity.Country) && GetCountry == null)//if coutry null then get countryid from getcity table to get country
                                {
                                    int countryId = Convert.ToInt32(getCity.Country);
                                    GetCountry = await db.acc_countries.Where(em => em.id == countryId).FirstOrDefaultAsync();
                                }

                                if (getCity != null && !string.IsNullOrEmpty(getCity.State) && getState == null)//if getState null then get state id from getcity table to get State
                                {
                                    int stateId = Convert.ToInt32(getCity.State);
                                    getState = await db.com_state.Where(em => em.ID == stateId).FirstOrDefaultAsync();
                                }
                            }

                            if (!string.IsNullOrEmpty(LeadSource))
                            {
                                getLeadSource = await db.crm_leadsource_tbl.Where(em => em.LeadsourceName.ToLower() == LeadSource.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();

                            }

                            if (!string.IsNullOrEmpty(ProductType))
                            {
                                getProductType = await db.crm_producttypetbl.Where(em => em.ProductTypeName.ToLower() == ProductType.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                            }

                            if (!string.IsNullOrEmpty(LeadStatus))
                            {
                                getStatus = await db.crm_leadstatus_tbl.Where(em => em.LeadStatusName.ToLower() == LeadStatus.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                            }                            
                            crm_createleadstbl CL = new crm_createleadstbl();
                            CL.LeadOwner = uid;
                            CL.MobileNo = Mobno.Trim();
                            CL.Customer = customer.Trim();
                            CL.OrganizationName = organizationname.Trim();
                            CL.EmailId = email;
                            CL.LeadStatusID = getStatus != null ? getStatus.Id : 0;
                            CL.LeadStatus = getStatus != null ? getStatus.LeadStatusName : string.Empty;
                            CL.FollowDate = FollowUpDate.Trim() == string.Empty ? DateTime.ParseExact(todayDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.ParseExact(FollowUpDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            CL.date = todayDate.ToString("dd/MM/yyyy");
                            CL.Createddate = todayDate;
                            CL.Status = true;
                            CL.LeadsType = "Manual";
                            CL.CountryID = GetCountry == null ? 1 : GetCountry.id;
                            //CL.Country = GetCountry==null ? "India" : GetCountry.country_name;
                            CL.StateID = getState == null ? 0 : getState.ID;
                            //CL.State = getState == null ? State : getState.State;
                            CL.CityID = getCity == null ? 0 : getCity.ID;
                            //CL.City = getCity == null ? City : getCity.City;                                    
                            CL.ProductTypeID = getProductType == null ? 0 : getProductType.Id;
                            //CL.ProductTypeName = getProductType == null ? String.Empty : getProductType.ProductTypeName;
                            CL.LeadSourceID = getLeadSource == null ? 0 : getLeadSource.Id;
                            //CL.LeadResource = getLeadSource == null ? String.Empty : getLeadSource.LeadsourceName;
                            CL.CompanyID = CompanyID;
                            CL.BranchID = BranchID;
                            //CL.FollowUpTime = todayDate.ToString("hh:mm tt"); //set followup time
                            //CL.ZoneName = Constant.GetCompanyTimeZone(CompanyID);
                            //CL.ConvertedFupDateTime = todayDate;
                            CL.IsLeadReminder = true;
                            db.crm_createleadstbl.Add(CL);
                            if (db.SaveChanges() > 0)
                            {
                                if (!string.IsNullOrEmpty(uploadDescription))
                                {
                                    var lid = CL.Id;
                                    crm_leaddescriptiontbl cdtbl = new crm_leaddescriptiontbl();
                                    cdtbl.LeadId = lid;
                                    cdtbl.Date = todayDate.ToString("dd/MM/yyyy");
                                    cdtbl.Description = uploadDescription;
                                    cdtbl.ByUID = Convert.ToInt32(uid);
                                    cdtbl.ByUserName = FullName;
                                    cdtbl.CreatedDateTime = todayDate;
                                    cdtbl.BranchID = BranchID;
                                    cdtbl.CompanyID = CompanyID;
                                    cdtbl.LeadStatusName = getStatus == null ? string.Empty : getStatus.LeadStatusName;
                                    db.crm_leaddescriptiontbl.Add(cdtbl);
                                    await db.SaveChangesAsync();
                                }
                            }
                            //trans.Commit();
                            isInsertOne = true;
                        }
                        else
                        {
                            isExist = true;

                            //DuplicateRecord += "Customer: " + customer + "/Email=" + email + "/Mobile No.:" + Mobno + "";

                            errormsg = row.ToString();
                            sb.AppendFormat("{0}, ", errormsg);
                        }
                        //}
                        //else
                        //{
                        //    errormsg = "** Lead upload in excel lead status is not match with system, please check the excelsheet. thank you";
                        //    break;
                        //}
                        //}
                    }//if customer name and mobile no is empty end
                    else
                    {
                        if (string.IsNullOrEmpty(Mobno) && string.IsNullOrEmpty(customer))
                        {
                            //errormsg = "Customer name and phone number is blank in row no.:" + row;
                            errormsg = "Uploaded Successfully";
                        }
                        else if (string.IsNullOrEmpty(Mobno) && !string.IsNullOrEmpty(customer))
                        {
                            errormsg = "Phone number is blank in row no.:" + row;
                        }
                        else if (!string.IsNullOrEmpty(Mobno) && string.IsNullOrEmpty(customer))
                        {
                            errormsg = "Customer name is blank in row no.:" + row;
                        }
                        break;// get out of the loop
                    }

                    row++;
                }

                if (errormsg == string.Empty)
                {
                    msg = "Uploaded Successfully";
                }
                else
                {
                    msg = errormsg;

                    if (isExist)
                    {
                        var sb1 = new StringBuilder();
                        if (isInsertOne)
                        {
                            sb1.Append("Uploaded Successfully! But these row numbers records are exist in our database: ");
                            sb1.Append(sb.ToString());
                            string existMsg = sb1.ToString();
                            msg = existMsg.TrimEnd(',');
                        }
                        else
                        {
                            sb1.Append("These row numbers. records are exist in our database: ");
                            sb1.Append(sb.ToString());
                            string existMsg = sb1.ToString();
                            msg = existMsg.TrimEnd(',');
                        }
                    }
                }
                return msg;
            }
            catch (Exception eSave)
            {
                //trans.Rollback();
                //msg = "Error has been occurred! Due to some mandatory fields are required";
                msg = eSave.Message.ToString();
                ExceptionLogging.SendExcepToDB(eSave);
                return msg + ", Row no.:" + row;
            }
            //}

        }


        //New Normal Lead upload
        public static string ImportNormalLead(DataTable dt, int uid, Int32 BranchID, Int32 CompanyID)
        {
            string msg = string.Empty;
            string errormsg = string.Empty;
            try
            {
                niscrmEntities db = new niscrmEntities();
                foreach (DataRow dr in dt.Rows)
                {
                    string Mobno = Convert.ToString(dr["PhoneNumber"]);
                    string customer = Convert.ToString(dr["CustomerName"]);
                    string email = Convert.ToString(dr["Email"]);
                    string FollowUpDate = Convert.ToString(dr["FollowUpDate"]);
                    string StatusName = Convert.ToString(dr["Status"]);
                    if (Convert.ToString(dr["PhoneNumber"]).Contains("+91"))
                    {
                        Mobno = Mobno.Replace("+91", "");
                    }

                    String DuplicateRecord = String.Empty;

                    bool numeronly = Regex.IsMatch(Mobno.Replace("+91", ""), @"^[0][1-9]\d{9}$|^[1-9]\d{9}$");
                    if (numeronly == true)
                    {
                        var getStatus = db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == "Open" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (getStatus != null)
                        {
                            var Getexists = db.crm_createleadstbl.Where(em => em.MobileNo.Replace("+91", "").Trim() == Mobno.Trim() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (Getexists == null)
                            {
                                crm_createleadstbl CL = new crm_createleadstbl();
                                CL.LeadOwner = uid;
                                CL.MobileNo = Mobno.Trim();
                                CL.Customer = customer.Trim();
                                CL.EmailId = email;
                                CL.LeadStatusID = getStatus.Id;
                                CL.LeadStatus = getStatus.LeadStatusName;
                                CL.FollowDate = FollowUpDate == string.Empty ? DateTime.ParseExact(Constant.GetBharatTime().ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.ParseExact(FollowUpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                CL.date = Constant.GetBharatTime().ToString("dd/MM/yyyy");
                                CL.Createddate = Constant.GetBharatTime();
                                CL.Status = true;
                                CL.LeadsType = "Manual";
                                CL.CompanyID = CompanyID;
                                CL.BranchID = BranchID;
                                db.crm_createleadstbl.Add(CL);
                                db.SaveChanges();
                            }
                            else
                            {
                                DuplicateRecord += "Customer: " + customer + "/Email=" + email + "/Mobile No.:" + Mobno + "";
                                errormsg = "** Lead upload from excel is already exists.";
                            }
                        }
                        else
                        {
                            errormsg = "** Lead upload in excel status 'Open' is not match with system, please check the excelsheet. thank you";
                            break;
                        }
                    }
                }

                if (errormsg == string.Empty)
                {
                    msg = "Uploaded Successfully";
                }
                else
                {
                    msg = errormsg;
                }
                return msg;
            }
            catch (Exception eSave)
            {
                msg = eSave.Message.ToString();
                ExceptionLogging.SendExcepToDB(eSave);
                return msg;
            }
        }

        //New Facebook Lead upload
        public static string ImportFacebookLeads(DataTable dt, int uid, Int32 BranchID, Int32 CompanyID)
        {
            string msg = string.Empty;
            string errormsg = string.Empty;
            try
            {
                niscrmEntities db = new niscrmEntities();
                var getleadSourceStatus = db.crm_leadsource_tbl.Where(em => em.LeadsourceName == "Facebook" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                var getStatus = db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == "Open" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();

                foreach (DataRow dr in dt.Rows)
                {
                    var Mobno = Convert.ToString(dr["PhoneNumber"]);
                    var customer = Convert.ToString(dr["FullName"]);
                    var email = Convert.ToString(dr["email"]);
                    if (Convert.ToString(dr["PhoneNumber"]).Contains("+91"))
                    {
                        Mobno = Mobno.Replace("+91", "");
                    }

                    bool numeronly = Regex.IsMatch(Mobno.Replace("+91", ""), @"^[0][1-9]\d{9}$|^[1-9]\d{9}$");
                    if (numeronly == true)
                    {
                        var Getexists = db.crm_createleadstbl.Where(em => em.MobileNo.Replace("+91", "").Trim() == Mobno.Trim() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (Getexists == null)
                        {
                            crm_createleadstbl CL = new crm_createleadstbl();
                            CL.LeadOwner = uid;
                            CL.MobileNo = Mobno.Trim();
                            CL.Customer = customer.Trim();
                            CL.EmailId = email;
                            CL.LeadStatusID = getStatus.Id;
                            CL.LeadStatus = getStatus.LeadStatusName;
                            CL.LeadSourceID = getleadSourceStatus.Id;
                            CL.LeadResource = getleadSourceStatus.LeadsourceName;
                            CL.FollowDate = DateTime.ParseExact(Constant.GetBharatTime().ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            CL.date = Constant.GetBharatTime().ToString("dd/MM/yyyy");
                            CL.Createddate = Constant.GetBharatTime();
                            CL.Status = true;
                            CL.LeadsType = "Facebook";
                            CL.CompanyID = CompanyID;
                            CL.BranchID = BranchID;
                            db.crm_createleadstbl.Add(CL);
                            db.SaveChanges();
                        }
                        else
                        {
                            errormsg = "** Facebook lead upload from excel is already exists.";
                        }
                    }
                }

                if (errormsg == string.Empty)
                {
                    msg = "Facebook uploaded successfully";
                }
                else
                {
                    msg = errormsg;
                }
                return msg;
            }
            catch (Exception eSave)
            {
                msg = eSave.Message.ToString();
                ExceptionLogging.SendExcepToDB(eSave);
                return msg;
            }
        }
        /// <summary>
        /// Button Click Code
        /// </summary>
        #endregion

        #region Export Data
        private void ExportExcelData()
        {
            //Excel.Application xlApp;
            //Excel.Workbook xlWorkBook;
            //Excel.Worksheet xlWorkSheet;
            //object misValue = System.Reflection.Missing.Value;

            //xlApp = new Excel.ApplicationClass();
            //xlWorkBook = xlApp.Workbooks.Add(misValue);
            //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            //int i = 0;
            //int j = 0;

            //for (i = 0; i <= dataGridViewExport.RowCount - 1; i++)
            //{
            //    for (j = 0; j <= dataGridViewExport.ColumnCount - 1; j++)
            //    {
            //        DataGridViewCell cell = dataGridViewExport[j, i];
            //        xlWorkSheet.Cells[i + 1, j + 1] = cell.Value;
            //    }
            //}

            //xlWorkBook.SaveAs("csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            //xlWorkBook.Close(true, misValue, misValue);
            //xlApp.Quit();

            //releaseObject(xlWorkSheet);
            //releaseObject(xlWorkBook);
            //releaseObject(xlApp);

            //essageBox.Show("Excel file created , you can find the file c:\\csharp.net-informations.xls");
        }

        private void GetExcelSheet()
        {
            //Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            //// creating new WorkBook within Excel application
            //Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            //// creating new Excelsheet in workbook
            //Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            //// see the excel sheet behind the program
            ////Funny
            //app.Visible = true;
            //// get the reference of first sheet. By default its name is Sheet1.
            //// store its reference to worksheet
            //try
            //{
            //    //Fixed:(Microsoft.Office.Interop.Excel.Worksheet)
            //    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
            //    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
            //    // changing the name of active sheet
            //    worksheet.Name = "Sheet1";
            //    // storing header part in Excel
            //    for (int i = 1; i < dataGridViewOrderProced.Columns.Count + 1; i++)
            //    {
            //        worksheet.Cells[1, i] = dataGridViewOrderProced.Columns[i - 1].HeaderText;
            //    }
            //    // storing Each row and column value to excel sheet
            //    for (int i = 0; i < dataGridViewOrderProced.Rows.Count - 1; i++)
            //    {
            //        for (int j = 0; j < dataGridViewOrderProced.Columns.Count; j++)
            //        {
            //            worksheet.Cells[i + 2, j + 1] = dataGridViewOrderProced.Rows[i].Cells[j].Value.ToString();
            //        }
            //    }

            //    // save the application
            //    string fileName = string.Empty;
            //    //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //    saveFileExcel.InitialDirectory = @"Desktop";
            //    saveFileExcel.FileName = DateTime.Now.ToString("yyMMddHHmmss");
            //    saveFileExcel.Filter = "Excel files |*.xls|All files (*.*)|*.*";
            //    saveFileExcel.FilterIndex = 2;
            //    saveFileExcel.RestoreDirectory = true;

            //    if (saveFileExcel.ShowDialog() == DialogResult.OK)
            //    {
            //        fileName = saveFileExcel.FileName;
            //        //Fixed-old code :11 para->add 1:Type.Missing
            //        workbook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            //    }
            //    else
            //        return;

            //    // Exit from the application
            //    //app.Quit();
            //}
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
            //finally
            //{
            //    app.Quit();
            //    workbook = null;
            //    app = null;
            //}
        }
        #endregion
    }
}