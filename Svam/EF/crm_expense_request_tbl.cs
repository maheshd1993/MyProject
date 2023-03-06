
namespace Svam.EF
{
    using System;
    using System.Collections.Generic;

    public class crm_expense_request_tbl
    {
        public long Id { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> ExpenseTypeId { get; set; }
        public string travelledKMS { get; set; }
        public string expense { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public string ProcessStatus { get; set; }
        public Nullable<System.DateTime> ProcessDate { get; set; }
        public string Comment { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> CompanyID { get; set; }
    }
}
