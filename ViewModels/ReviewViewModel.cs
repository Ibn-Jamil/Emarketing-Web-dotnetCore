using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AngularCoreEmarketing.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        [Required]
        public int RatingStars { get; set; }
        [Required]
        [Display(Name ="Comments")]
        public string ReviewsDescription { get; set; }
        public int ProductDetailsId { get; set; }

    }
}