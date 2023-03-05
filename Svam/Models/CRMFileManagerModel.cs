using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class CRMFileManagerModel
    {
        public Int64? FileID { get; set; }
        public String FileName { get; set; }

        public String FileUpload { get; set; }

        public String FileStatusName { get; set; }
        public String UploadDate { get; set; }
        public string DateFormat { get; set; }
        public List<CRMFileManagerModel> oCRMFileManagerModelList { get; set; }
    }
}