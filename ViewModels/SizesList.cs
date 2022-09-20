using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularCoreEmarketing.Models
{
    public class SizesList
    {
        public Dictionary<string,string> ListofSizes { get; set; }
        public SizesList()
        {
            ListofSizes.Add("XS", "XS");
            ListofSizes.Add("S", "S");
            ListofSizes.Add("M", "M");
            ListofSizes.Add("L", "L");
            ListofSizes.Add("XL", "XL");
            ListofSizes.Add("XXL", "XXL");
            ListofSizes.Add("XXXL", "XXXL");
        }
    }
}