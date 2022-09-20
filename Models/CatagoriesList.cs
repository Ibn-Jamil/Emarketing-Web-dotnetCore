using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AngularCoreEmarketing.Models
{
    public class CatagoryMajor
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Parent Category")]
        public string MajorName { get; set; }
        public ICollection<CatagoryMajorSub> MajorSubCatagory { get; set; }
       
    }
    public class CatagoryMajorSub
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Child Category")]
        public string MajorSubName { get; set; }
        [ForeignKey("CatagoriesMajor")]
        public ICollection<CatagorySpecific> SpecificCatagory{ get; set; }
        

    }
    public class CatagorySpecific
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Grandchild Category")]
        public string CatagoriesName { get; set; }
    }

}