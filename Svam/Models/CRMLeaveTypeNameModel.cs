using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class CRMLeaveTypeNameModel
    {
        public int ID  { get; set; }
        [Required(ErrorMessage = "* Enter leave name")]
        public string LeaveName  { get; set; }
        public string StatusName { get; set; }
        public bool IsActive { get; set; }
        public List<CRMLeaveTypeNameModel> CRMLeaveTypeModelList  { get; set; }
    }
}