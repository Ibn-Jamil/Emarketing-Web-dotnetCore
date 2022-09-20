using AngularCoreEmarketing.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularCoreEmarketing.ViewModels
{
    public class EditViewModel
    {
        public ProductDetails Prod { get; set; }
        public List<string> Images { get; set; }
    }
}