using System;
using System.ComponentModel.DataAnnotations;

namespace AngularCoreEmarketing.Models
{
    public class ProductFeedback
    {
        public int Id { get; set; }
        [Required]
        public string Questions { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Answar { get; set; }
        [Required]
        public DateTime QuestionTime { get; set; }
        public DateTime AnswarTime { get; set; }
        [Required]
        public int ProductDetailsId { get; set; }
    }
}