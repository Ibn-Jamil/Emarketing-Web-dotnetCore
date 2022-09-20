using AngularCoreEmarketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AngularCoreEmarketing.ViewModels
{
    public class CatagoryListViewModel
    {   
        public CatagoryMajor CatagoriesMajor { get; set; }
        public CatagoryMajorSub CatagoriesMajorSub { get; set; }
        public CatagorySpecific CatagoriesSpecific { get; set; }
    }
}