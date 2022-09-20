using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCoreEmarketing.Dtos
{
    public class CatagoryMajorDto
    {
        public int Id { get; set; }
        public string MajorName { get; set; }
        public ICollection<CatagoryMajorSubDto> CatagoryMajorSubDto { get; set; }
    }
    public class CatagoryMajorSubDto
    {
        public int Id { get; set; }
        public string MajorSubName { get; set; }
        public ICollection<CatagorySpecificDto> CatagorySpecificDto{ get; set; }

    }
    public class CatagorySpecificDto
    {
        public int Id { get; set; }
        public string CatagoriesName { get; set; }

    }
}
