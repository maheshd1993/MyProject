using Svam.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Models
{
    public class CRMQutationsaleModel
    {
        public Nullable<Int32> ID { get; set; }
        public string Organization { get; set; }


        public List<CRMQutationsaleModel> OrgList1 { get; set; }
        public List<CRMQutationsaleModel> OrgList2 { get; set; }

        public List<CRMQutationsaleModel> joinlist { get; set; }
    }

    public class Addcustomegrop
    {        
        public Int64 ID { get; set; }
        public string CustGroupName { get; set; }
        

    }
}