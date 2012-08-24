using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatTmdb.V3
{
    public class ParentCompany
    {
        public string name { get; set; }
        public int id { get; set; }
        public string logo_path { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

    public class TmdbCompany
    {
        public object description { get; set; }
        public object headquarters { get; set; }
        public object homepage { get; set; }
        public int id { get; set; }
        public string logo_path { get; set; }
        public string name { get; set; }
        public ParentCompany parent_company { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
