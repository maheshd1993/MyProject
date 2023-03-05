using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Models.ApiModel
{
    public class ViewGeneralRemarkApiModel
    {
        public int Id { get; set; }
        public int UID { get; set; }
        public string UserName { get; set; }
        public string Profile { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public string Date { get; set; }
        //public string DateFormat { get; set; }
    }
    public class CreateGeneralRemarkModel
    {
        public int UID { get; set; }
        [AllowHtml]
        public string Remark  { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string Token  { get; set; }
    }
}