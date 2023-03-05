using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Svam._Classes
{
    public class apiclasses
    {
        DataUtility du = new DataUtility();
        public List<ecomclasses.Login> getloginrecord(string email, string pass)
        {
            List<ecomclasses.Login> li = new List<ecomclasses.Login>();
            var dtlogin = du.GetDataTable("select tul.user_id,tul.user_name,tul.user_email,tul.user_password,cust.CompanyID,tlogin.branch from t_userlogin tul left join customers cust on cust.ID=tul.cust_id left join t_login tlogin on tlogin.company_id=cust.CompanyID where tul.user_email='" + email + "' and tul.user_password='" + pass + "' and tlogin.branch<>000 limit 1;");
            if (dtlogin.Rows.Count > 0)
            {
                // var res = db.com_branches.Where(em => em.OrgBranchCode == 100).FirstOrDefault();
                ecomclasses.Login tlogin = new ecomclasses.Login();
                tlogin.user_id = dtlogin.Rows[0]["user_id"].ToString();
                tlogin.user_name = dtlogin.Rows[0]["user_name"].ToString();
                tlogin.user_email = dtlogin.Rows[0]["user_email"].ToString();
                tlogin.password = dtlogin.Rows[0]["user_password"].ToString();
                tlogin.CompanyID = dtlogin.Rows[0]["CompanyID"].ToString();
                tlogin.branch = dtlogin.Rows[0]["branch"].ToString();
                li.Add(tlogin);
            }
            return li;
        }
        public List<ecomclasses.categories> getcategoriesrecord(int companyid, int branchid)
        {
            List<ecomclasses.categories> li = new List<ecomclasses.categories>();
            var dtCategory = du.GetDataTable("select Category,categoryID from inv_itemcategories where CompanyID='" + companyid + "' and BranchCode='" + branchid + "'");
            for (int i = 0; i < dtCategory.Rows.Count; i++)
            {
                ecomclasses.categories invitemcategories = new ecomclasses.categories();
                invitemcategories.Category = dtCategory.Rows[i]["Category"].ToString();
                invitemcategories.categoryID = dtCategory.Rows[i]["categoryID"].ToString();
                li.Add(invitemcategories);
            }
            return li;
        }
        public List<ecomclasses.products> getallproductsrecord(int companyid, int branchid)
        {
            List<ecomclasses.products> li = new List<ecomclasses.products>();
            var dtallproducts = du.GetDataTable("select invs.ID,invs.ItemName,invs.SKU,invs.UnitPrice as PurchasePrice,invs.OthCost4 as MRP, invs.thumbnail_image as ThumbNailImage,Sum(invstk.QuantityIn)- Sum(invstk.QuantityOut) as Quantity from inv_itemsku invs left join inv_inventorystock invstk on invstk.SKU = invs.SKU where invs.CompanyId='" + companyid + "' and invs.BranchCode='" + branchid + "'");
            for (int i = 0; i < dtallproducts.Rows.Count; i++)
            {
                ecomclasses.products invitemproducts = new ecomclasses.products();
                invitemproducts.ID = Convert.ToInt32(dtallproducts.Rows[i]["ID"]);
                invitemproducts.ItemName = dtallproducts.Rows[i]["ItemName"].ToString();
                invitemproducts.SKU = dtallproducts.Rows[i]["SKU"].ToString();
                invitemproducts.PurchasePrice = dtallproducts.Rows[i]["PurchasePrice"].ToString();
                invitemproducts.MRP = dtallproducts.Rows[i]["MRP"].ToString();
                invitemproducts.ThumbNailImage = dtallproducts.Rows[i]["ThumbNailImage"].ToString();
                invitemproducts.Quantity = dtallproducts.Rows[i]["Quantity"].ToString();
                li.Add(invitemproducts);
            }
            return li;
        }
        public List<ecomclasses.products> getcategorywiseproductsrecord(int companyid, int branchid, string categoryid)
        {
            List<ecomclasses.products> li = new List<ecomclasses.products>();
            var dtallproducts = du.GetDataTable("select invs.ID,invs.ItemName,invs.SKU,invs.UnitPrice as PurchasePrice,invs.OthCost4 as MRP, invs.thumbnail_image as ThumbNailImage, Sum(invstk.QuantityIn)- Sum(invstk.QuantityOut) as Quantity from inv_itemsku invs left join inv_inventorystock invstk on invstk.SKU = invs.SKU where invs.CompanyId='" + companyid + "' and invs.BranchCode='" + branchid + "' and invs.CategoryID='" + categoryid + "'");
            for (int i = 0; i < dtallproducts.Rows.Count; i++)
            {
                ecomclasses.products invitemproducts = new ecomclasses.products();
                invitemproducts.ID = Convert.ToInt32(dtallproducts.Rows[i]["ID"]);
                invitemproducts.ItemName = dtallproducts.Rows[i]["ItemName"].ToString();
                invitemproducts.SKU = dtallproducts.Rows[i]["SKU"].ToString();
                invitemproducts.PurchasePrice = dtallproducts.Rows[i]["PurchasePrice"].ToString();
                invitemproducts.MRP = dtallproducts.Rows[i]["MRP"].ToString();
                invitemproducts.ThumbNailImage = dtallproducts.Rows[i]["ThumbNailImage"].ToString();
                invitemproducts.Quantity = dtallproducts.Rows[i]["Quantity"].ToString();
                li.Add(invitemproducts);
            }
            return li;
        }
        public List<ecomclasses.products> Cartpoductsrecord(ecomclasses.Cartproducts cartproducts)
        {
            List<ecomclasses.products> li = new List<ecomclasses.products>();
            for (int i = 0; i < cartproducts.products.Count; i++)
            {
                var dtallproducts = du.GetDataTable("select invs.ID,invs.ItemName,invs.SKU,invs.UnitPrice as PurchasePrice,invs.OthCost4 as MRP, invs.thumbnail_image as ThumbNailImage,Sum(invstk.QuantityIn)- Sum(invstk.QuantityOut) as Quantity from inv_itemsku invs left join inv_inventorystock invstk on invstk.SKU = invs.SKU where invs.CompanyId='" + cartproducts.CompanyID + "' and invs.BranchCode='" + cartproducts.branchID + "' and invs.ID= '" + cartproducts.products[i].ID + "'");
                for (int j = 0; j < dtallproducts.Rows.Count; j++)
                {
                    ecomclasses.products invitemproducts = new ecomclasses.products();
                    invitemproducts.ID = Convert.ToInt32(dtallproducts.Rows[j]["ID"]);
                    invitemproducts.ItemName = dtallproducts.Rows[j]["ItemName"].ToString();
                    invitemproducts.SKU = dtallproducts.Rows[j]["SKU"].ToString();
                    invitemproducts.PurchasePrice = dtallproducts.Rows[j]["PurchasePrice"].ToString();
                    invitemproducts.MRP = dtallproducts.Rows[j]["MRP"].ToString();
                    invitemproducts.ThumbNailImage = dtallproducts.Rows[j]["ThumbNailImage"].ToString();
                    invitemproducts.Quantity = dtallproducts.Rows[j]["Quantity"].ToString();
                    li.Add(invitemproducts);
                }
            }
            return li;
        }
        int ID;
        private string GetCustomerID(string companyid)
        {
            string customerid = string.Empty;
            try
            {
                var dt = du.GetDataTable("select count(*) as count from Customers where CompanyId = " + companyid + "");
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString() == "0")
                    {
                        var dtcom = du.GetDataTable("select upper(lpad(Organization,3,0)) as companyname from company_profile where ID = " + companyid + "");
                        customerid = dtcom.Rows[0]["companyname"].ToString() + "-CUSD-" + (ID + 1);
                    }
                    else
                    {
                        var dtcom = du.GetDataTable("select upper(lpad(Organization,3,0)) as companyname from company_profile where ID = " + companyid + "");
                        ID = Convert.ToInt32(dt.Rows[0][0].ToString()) + 1;
                        customerid = dtcom.Rows[0]["companyname"].ToString() + "-CUSD-" + ID;
                    }
                }
            }
            catch
            {
            }
            return customerid;
        }
        public string registersrecord(ecomclasses.Register register)
        {
            int res = 0;
            var lastsyncidcustomers = du.GetDataTable("SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as syncid FROM customers  WHERE CompanyID='" + register.CompanyID + "' and SyncID RLIKE 'O'");
            string customerid = GetCustomerID(register.CompanyID);
            int syncidcust = Convert.ToInt32(lastsyncidcustomers.Rows[0]["syncid"]);
            syncidcust = syncidcust + 1;
            res = du.ExecuteSql("insert into customers (CustomerID,CustomerName,BillingAddress,DeliveryAddress,MobileNo,EmailID,CompanyID,BranchCode,CreatedDate,ModifiedDate,flag,SyncID) " +
                                    "values('" + customerid + "','" + register.Name + "','" + register.Address + "','" + register.Address + "','" + register.Mobile + "','" + register.Email + "','" + register.CompanyID + "','0','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','N','" + "O" + syncidcust.ToString() + "')");

            object insertedcustid = du.GetScalar("SELECT LAST_INSERT_ID();");

            var lastsynciduserlogin = du.GetDataTable("SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as syncid FROM t_userlogin  WHERE CompanyID='" + register.CompanyID + "' and SyncID RLIKE 'O'");
            int synciduserlogin = Convert.ToInt32(lastsynciduserlogin.Rows[0]["syncid"]);
            synciduserlogin = synciduserlogin + 1;
            res = du.ExecuteSql("insert into t_userlogin (user_name,user_email,user_mobile,user_password,user_address,cust_id,CreatedDate,IsActive,CompanyID,flag,SyncID)" +
                                "values('" + register.Name + "','" + register.Email + "','" + register.Mobile + "','" + register.Password + "','" + register.Address + "'," + insertedcustid.ToString() + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','yes','" + register.CompanyID + "','N','" + "O" + synciduserlogin.ToString() + "')");
            if (res > 0)
            {
                return "Registered Successfully";
            }
            else
            {
                return "Try Again";
            }
        }
        public List<ecomclasses.OrderHistory> getorderhistorysrecord(int companyid, int Userid)
        {
            List<ecomclasses.OrderHistory> li = new List<ecomclasses.OrderHistory>();
            var dtordernos = du.GetDataTable("SELECT `OrderId`, `OrderDate`, `user_name`, `DeliveryDate`, `DeliveryCharge`, `DeliveryTime`, SUM(Amount*Item_qun) as grand_total, SUM(Item_qun) as total_qty " +
                                             "FROM `online_orders` WHERE `User_Id` = '" + Userid + "' and CompanyID='" + companyid + "'  GROUP BY `OrderId` ORDER BY `order_id` DESC ;");
            for (int i = 0; i < dtordernos.Rows.Count; i++)
            {
                var dtordersinfo = du.GetDataTable("SELECT `online_orders`.*, `inv_itemsku`.`thumbnail_image`,cp.Organization, `inv_itemsku`.`CategoryID`, `inv_itemsku`.`SKU`, `inv_itemsku`.`ID`," +
                                                   "`inv_itemsku`.`ItemName`, `inv_itemsku`.`CategoryID`,  `inv_itemsku`.`UnitID`, `inv_itemsku`.`OthCost2` FROM `online_orders` Left JOIN `inv_itemsku` " +
                                                   "ON `inv_itemsku`.`SKU`=`online_orders`.`Sku_ID` Left Join company_profile cp on cp.ID=online_orders.companyid WHERE `OrderId` = '" + dtordernos.Rows[i]["OrderId"].ToString() + "'");
                for (int j = 0; j < dtordersinfo.Rows.Count; j++)
                {
                    ecomclasses.OrderHistory orderHistoryrecords = new ecomclasses.OrderHistory();
                    orderHistoryrecords.ShipTo = dtordersinfo.Rows[j]["user_name"].ToString();
                    orderHistoryrecords.Total = Convert.ToDecimal(dtordersinfo.Rows[j]["Amount"]) * Convert.ToDecimal(dtordersinfo.Rows[j]["Item_qun"]);
                    orderHistoryrecords.OrderNo = dtordersinfo.Rows[j]["OrderId"].ToString();
                    orderHistoryrecords.OrderPlacedDate = String.Format("{0:dd/MM/yyyy}", dtordersinfo.Rows[j]["OrderDate"].ToString());
                    orderHistoryrecords.Status = dtordersinfo.Rows[j]["Order_Status"].ToString();
                    orderHistoryrecords.DeliveryDateTime = dtordersinfo.Rows[j]["DeliveryDate"].ToString() + " " + dtordersinfo.Rows[j]["DeliveryTime"].ToString();
                    orderHistoryrecords.ItemName = dtordersinfo.Rows[j]["ItemName"].ToString();
                    orderHistoryrecords.SoldBy = dtordersinfo.Rows[j]["Organization"].ToString();
                    orderHistoryrecords.Price = dtordersinfo.Rows[j]["Amount"].ToString();
                    orderHistoryrecords.Quantity = dtordersinfo.Rows[j]["Item_qun"].ToString();
                    orderHistoryrecords.ThumbNailImage = dtordersinfo.Rows[j]["thumbnail_image"].ToString();
                    li.Add(orderHistoryrecords);
                }
            }
            return li;
        }
        public Dictionary<int, string> timeslot()
        {
            Dictionary<int, string> dictimeslot = new Dictionary<int, string>();
            dictimeslot.Add(1,"6 AM - 8 AM");
            dictimeslot.Add(2,"8 AM - 10 AM");
            dictimeslot.Add(3,"10 AM - 12 AM");
            dictimeslot.Add(4,"5 PM - 7 PM");
            dictimeslot.Add(5,"7 PM - 9 PM");
            dictimeslot.Add(6,"9 PM - 11 PM");
            return dictimeslot;
        }
        public ecomclasses.CheckOut Checkoutrecords(ecomclasses.Cartproducts cartproducts)
        {
            List<ecomclasses.products> li = new List<ecomclasses.products>();
            ecomclasses.CheckOut CO = new ecomclasses.CheckOut();
            List<ecomclasses.TimeSlot> lsttimeslot = new List<ecomclasses.TimeSlot>();
            for (int i = 0; i < cartproducts.products.Count; i++)
            {
                var dtallproducts = du.GetDataTable("select invs.ID,invs.ItemName,invs.SKU,invs.baseprice as MRP, invs.thumbnail_image as ThumbNailImage,Sum(invstk.QuantityIn)- Sum(invstk.QuantityOut) as Quantity from inv_itemsku invs left join inv_inventorystock invstk on invstk.SKU = invs.SKU where invs.CompanyId='" + cartproducts.CompanyID + "' and invs.BranchCode='" + cartproducts.branchID + "' and invs.ID= '" + cartproducts.products[i].ID + "'");
                for (int j = 0; j < dtallproducts.Rows.Count; j++)
                {
                    ecomclasses.products invitemproducts = new ecomclasses.products();
                    invitemproducts.ID = Convert.ToInt32(dtallproducts.Rows[j]["ID"]);
                    invitemproducts.ItemName = dtallproducts.Rows[j]["ItemName"].ToString();
                    invitemproducts.SKU = dtallproducts.Rows[j]["SKU"].ToString();
                    invitemproducts.MRP = dtallproducts.Rows[j]["MRP"].ToString();
                    invitemproducts.ThumbNailImage = dtallproducts.Rows[j]["ThumbNailImage"].ToString();
                    invitemproducts.Quantity = dtallproducts.Rows[j]["Quantity"].ToString();
                    li.Add(invitemproducts);
                }
            }
            var timeslots = timeslot().ToList();
            for (int j = 0; j < timeslots.Count; j++)
            {
                ecomclasses.TimeSlot ets = new ecomclasses.TimeSlot();
                ets.key = timeslots[j].Key;
                ets.value = timeslots[j].Value;
                lsttimeslot.Add(ets);
            }
            var dtuserdetails = du.GetDataTable("select user_name,user_email,user_mobile,user_address from t_userlogin where CompanyID='" + cartproducts.CompanyID + "' and user_email='" + cartproducts.Email + "'");
            CO.checkoutdetails.Name = dtuserdetails.Rows[0]["user_name"].ToString();
            CO.checkoutdetails.Email = dtuserdetails.Rows[0]["user_email"].ToString();
            CO.checkoutdetails.Mobile = dtuserdetails.Rows[0]["user_mobile"].ToString();
            CO.checkoutdetails.Address = dtuserdetails.Rows[0]["user_address"].ToString();
            CO.checkoutdetails.TimeSlot = lsttimeslot; 
            CO.checkoutdetails.COproducts = li; 
            return CO;
        }
        public string upddateuserdetails(string name, string mobile, string email, string address, string userid)
        {
            int res = 0;
            var dtgetcustid = du.GetDataTable("select cust_id from t_userlogin where user_id='" + userid + "'");

            res = du.ExecuteSql("Update Customers set CustomerName='" + name + "',BillingAddress='" + address + "',DeliveryAddress='" + address + "',MobileNo='" + mobile + "',EmailID='" + email + "' where ID='" + dtgetcustid.Rows[0]["cust_id"].ToString() + "'");
            res = du.ExecuteSql("Update t_userlogin set user_name='" + name + "',user_email='" + email + "',user_mobile='" + mobile + "',user_address='" + address + "' where user_id='" + userid + "'");
            if (res > 0)
            {
                return "Data has been successfully updated";
            }
            else
            {
                return "Error";
            }
        }

        public string orderPlaceDetails(ecomclasses.PlacedOrder placeorder)
        {
            int res = 0;

            for (int i = 0; i < placeorder.checkoutdetails.COproducts.Count; i++)
            {
                var lastsyncidorders = du.GetDataTable("SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as syncid FROM online_orders  WHERE CompanyID='" + placeorder.CompanyID + "' and User_Id='" + placeorder.User_Id + "' and SyncID RLIKE 'O'");
                res = du.ExecuteSql("insert into online_orders (Item_Id,Sku_ID,PurchasePrice,MRP,MrpDiscount,SavingAmt,Item_qun,Amount,User_Id,BranchCode,CompanyID,user_name,user_email," +
                                  "user_mobile,user_address,OrderId,DeliveryDate,DeliveryTime,DeliveryCharge,Order_Status,OrderDate,isactive,flag,SyncID) " +
                                  "values('" + placeorder.checkoutdetails.COproducts[i].ID + "','" + placeorder.checkoutdetails.COproducts[i].SKU + "','" + placeorder.checkoutdetails.COproducts[i].PurchasePrice + "','" + placeorder.checkoutdetails.COproducts[i].MRP + "'," +
                                  "'" + placeorder.checkoutdetails.COproducts[i].MrpDiscount + "','" + placeorder.checkoutdetails.COproducts[i].SavingAmt + "','" + placeorder.checkoutdetails.COproducts[i].Quantity + "','" + placeorder.checkoutdetails.COproducts[i].MRP + "'," +
                                  "'" + placeorder.User_Id + "','" + placeorder.BranchCode + "','" + placeorder.CompanyID + "','" + placeorder.checkoutdetails.Name + "','" + placeorder.checkoutdetails.Email + "','" + placeorder.checkoutdetails.Mobile + "','" + placeorder.checkoutdetails.Address + "','" + "ORD" + placeorder.BranchCode + DateTime.Now.ToString("ddMMyy") + "','" + placeorder.checkoutdetails.Deliverydate + "','" + placeorder.checkoutdetails.TimeSlot[i].value + "','0'," +
                                  "'Pending','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','yes','N','" + "O" + lastsyncidorders.Rows[0]["syncid"].ToString() + "')");
            }
            string html=createinvoice(placeorder.checkoutdetails.COproducts);
            SendEmail(placeorder.checkoutdetails.Email, html);
            if (res > 0)
            {
                return "Order Placed Successfully";
            }
            else
            {
                return "Error";
            }
        }
        public string createinvoice(List<ecomclasses.products> lstproducts)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < lstproducts.Count; i++)
            {
                sb.Append(@"<tr>						
					<td style='' align='center'>
						<table class='table-inner' width='600' cellspacing='0' cellpadding='0' border='0' align='center'>
							<tbody><tr><td>
									<table class='table-full1' width='183' cellspacing='0' cellpadding='0' border='0' align='left'>
										<tbody><tr>
											<td class='w3l-p' style='font-family: Candara, sans-serif; color:#7f8c8d; font-size:16px; line-height: 28px;' align='center'>
											</td> <td><img src='" + "http://www.smartcapita.com/" + lstproducts[i].ThumbNailImage + "' style=' height:100px;' class='scale' alt='' editable='true' width='100'></td></tr>");

                sb.Append(@"<tr>
											<td class='w3-w2' style='line-height: 0px;' height='35' align='center'>	
											</td>
										</tr></tbody></table><table width='1' cellspacing='0' cellpadding='0' border='0' align='left'>
										<tbody><tr>
											<td class='h' style='font-size: 0;line-height: 0px;border-collapse: collapse;' height='40'>
												<p style='padding-left: 24px;'>&nbsp;</p>
											</td></tr>
									</tbody></table><table class='table-full2 full_2' width='183' cellspacing='0' cellpadding='0' border='0' align='left'>
										<!--icon-->
										<tbody><tr></tr><tr class='all_new_echo_dot'>
											<td class='w3l-p' style='font-family: Candara, sans-serif; color:#7f8c8d; font-size:16px; line-height: 28px;' align='center'>
												<label class='content'><a href=''>"+lstproducts[i].ItemName+"</a> (Black)<p>Electronics</p></label></td></tr></tbody></table>");
                sb.Append(@"<table width='1' cellspacing='0' cellpadding='0' border='0' align='left'>
										<tbody><tr>
											<td class='h' style='font-size:0;line-height: 0px;border-collapse: collapse;' height='40'>
												<p style='padding-left: 24px;'>&nbsp;</p>
											</td>
										</tr>
									</tbody></table><table class='table-full3 full_3_tbl' width='183' cellspacing='0' cellpadding='0' border='0' align='left'>
										<tbody><tr class='price_for_all_new_echo'>
											<td class='w3l-p' style='font-family: Candara, sans-serif; color:#7f8c8d; font-size:16px; line-height: 28px;' align='center'>
												<label class='content'><b style='color:#000;'><p>Rs."+lstproducts[i].MRP+"</p></b></label><b style='color:#000;'></b></td></tr></tbody></table></td></tr></tbody></table></td></tr>");
            }
            return sb.ToString();
        }
        public static void SendEmail(string to,string htmlString)
        {
            try
            {
                //string from_name	= "Nicole";
		    	string from		= "demo@projectupdate.info";
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(from);
                message.To.Add(new MailAddress(to));
                message.ReplyToList.Add(new MailAddress(from));
                message.Subject = "Thank you for purchasing with us";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Host = "mail.projectupdate.info"; //for gmail host  
                smtp.Credentials = new System.Net.NetworkCredential("demo@projectupdate.info", "Demo#1234");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }  
    }
}

