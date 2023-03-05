using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class TktFormFieldsModel
    {
        public string TextName { get; set; }
        public string ColumnName   { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired  { get; set; }
        public string FieldType { get; set; }
        public int? Priority { get; set; }
        public bool CanChangeFieldType  { get; set; }
        public bool CanDeleteThisField { get; set; }
        public bool AlwaysActive { get; set; }
        public bool AlwaysRequired  { get; set; }
    }
}