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
    public class NavbarApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NavbarApiController(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
      
        public List<CatagoryMajorDto> MajorTableList()
        {
            var result = _context.CatagoriesMajor.Include(v => v.MajorSubCatagory).ThenInclude(b => b.SpecificCatagory)
                .Select(_mapper.Map <CatagoryMajor, CatagoryMajorDto>).ToList();
           
            return result;
        }
    }
}
