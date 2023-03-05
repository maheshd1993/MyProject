using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class LeadModel 
    {
        public string Customer { get; set; }
        public string Mobno { get; set; }
        public string WhatsAppNo { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string JobTitle  { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime createdOn { get; set; }
    }
}