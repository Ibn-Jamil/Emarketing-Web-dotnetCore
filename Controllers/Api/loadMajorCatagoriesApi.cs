using AngularCoreEmarketing.Data;
using AngularCoreEmarketing.Dtos;
using AngularCoreEmarketing.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCoreEmarketing.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class loadMajorCatagoriesApi : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public loadMajorCatagoriesApi(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<CatagoryMajorDto> MajorTableFilter(int MajorId)
        {
            var FilterdTable = _context.CatagoriesMajor.Select(_mapper.Map<CatagoryMajor, CatagoryMajorDto>).ToList();
            return FilterdTable;
        }
    }
}
