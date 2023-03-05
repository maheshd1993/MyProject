﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.FBApiModel
{
    public class JsonData
    {
        [JsonProperty("entry")]
        public List<Entry> Entry { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }
    }
    public class Entry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("changes")]
        public List<Change> Changes { get; set; }
           
    }

    public class Change
    {
        [JsonProperty("value")]
        public Value Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }    
    }

    public class Value
    {
        //[JsonProperty("ad_id")]
        //public string AdId { get; set; }

        [JsonProperty("form_id")]
        public string FormId { get; set; }

        [JsonProperty("leadgen_id")]
        public string LeadGenId { get; set; }

        [JsonProperty("created_time")]
        public int CreatedTime { get; set; }

        [JsonProperty("page_id")]
        public string PageId { get; set; }

        //[JsonProperty("adgroup_id")]
        //public string AdGroupId { get; set; }
    }

    public class LeadData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_time")]
        public string CreatedTime { get; set; }

        [JsonProperty("field_data")]
        public List<FieldData> FieldData { get; set; }
    }

    public class FieldData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("values")]
        public List<string> Values { get; set; }
    }

    public class LeadFormData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("leadgen_export_csv_url")]
        public string CsvExportUrl { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}