using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Svam.EF
{
    using System;
    using System.Collections.Generic;

    public class crm_formrequest_tbl
    {
        public long Id { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> FormTypeID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public string ProcessStatus { get; set; }
        public Nullable<System.DateTime> ProcessDate { get; set; }
        public string Comment { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> CompanyID { get; set; }
    }
}
