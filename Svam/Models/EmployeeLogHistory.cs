using System;
using System.Collections.Generic;

namespace Traders.Models
{
    public class EmployeeLogHistory
    {
        public Int64 Id { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string DateFormat  { get; set; }
        public string TimeZone { get; set; }
        public string ProfileName { get; set; }
        public string LoginDate { get; set; }
        public string LoginTime { get; set; }
        public string LogoutDate { get; set; }
        public string LogoutTime { get; set; }
        public string Duration { get; set; }
        public bool WorkingLateHours { get; set; }
        public bool ExtraWorking { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public Nullable<Int32> UserID { get; set; }
        public String UserName { get; set; }
        public List<EmployeeLogHistory> AssignUserList { get; set; }
        public List<EmployeeLogHistory> GetEmpLogHistoryModel = new List<EmployeeLogHistory>();
    }
}