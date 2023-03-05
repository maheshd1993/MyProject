using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class CRMUrgencyModel
    {
        public int UrgencyId { get; set; }
        [Required(ErrorMessage = "* Enter urgency name")]
        public string UrgencyName { get; set; }

        public string StatusName { get; set; }

        public List<CRMUrgencyModel> CRMUrgencyModelList { get; set; }
    }
}