using AngularCoreEmarketing.Data;
using AngularCoreEmarketing.Dtos;
using AngularCoreEmarketing.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCoreEmarketing.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class loadSpecficCatagoryApi : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public loadSpecficCatagoryApi(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
       
        public List<CatagorySpecificDto> MajorSubTableFilter(int MajorSubId)
        {
            var FilterdTable = _context.CatagoriesMajorSub.Include(c=>c.SpecificCatagory).Include(v=>v.SpecificCatagory)
                .Where(x => x.Id == MajorSubId).SelectMany(x => x.SpecificCatagory)
                .Select(_mapper.Map<CatagorySpecific, CatagorySpecificDto>).ToList();
            return FilterdTable;
        }
    }
}
