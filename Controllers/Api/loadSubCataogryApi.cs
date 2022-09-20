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
    public class loadSubCataogryApi : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public loadSubCataogryApi(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public  List<CatagoryMajorSubDto> MajorSubTableFilter(int MajorId)
        {
            var FilterdTable =  _context.CatagoriesMajor.Include(c=>c.MajorSubCatagory).Where(x => x.Id == MajorId)
                .SelectMany(x => x.MajorSubCatagory).Select(_mapper.Map<CatagoryMajorSub,CatagoryMajorSubDto>).ToList();
            return FilterdTable;
        }
    }
}
