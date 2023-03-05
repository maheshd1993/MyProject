using System.Collections.Generic;

namespace Svam.Models
{
    public class FlatObject
    {
        //public Int64 Id { get; set; }
        //public Int64 ParentId { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string data { get; set; }
        public bool chk { get; set; }
        //public FlatObject(string name, Int64 id, Int64 parentId)
        //{
        //    data = name;
        //    Id = id;
        //    ParentId = parentId;
        //}
        public List<FlatObject> flatobjectList = new List<FlatObject>();
    }

    public class RecursiveObject
    {
        public string data { get; set; }
        //public Int64 id { get; set; }
        public string id { get; set; }
        public FlatTreeAttribute attr { get; set; }
        public List<RecursiveObject> children { get; set; }
    }

    public class FlatTreeAttribute
    {
        public string id;
        public bool selected;
    }
}