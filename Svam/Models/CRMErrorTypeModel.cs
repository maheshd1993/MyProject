using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class CRMErrorTypeModel
    {
        public int ErrorId { get; set; }
        [Required(ErrorMessage = "* Enter error name")]
        public string ErrorName { get; set; }

        public string StatusName { get; set; }
        public bool IsActive { get; set; }
        public List<CRMErrorTypeModel> CRMErrorTypeModelList { get; set; }
    }
}