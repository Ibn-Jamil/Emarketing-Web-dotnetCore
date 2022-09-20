using AngularCoreEmarketing.Dtos;
using AngularCoreEmarketing.Models;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularCoreEmarketing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AngularCoreEmarketing.ViewModels;

namespace AngularCoreEmarketing.Controllers.Api
{
    [Route("Api/[controller]")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CustomerApiController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<string> AskQuestion(InteractionData Model)
        {

            if (Model.Text != null && Model.FeedbackId > 0 &&User.Identity.IsAuthenticated)
            {
                string user = User.Identity.Name;
                var userName = await _context.Users.SingleOrDefaultAsync(x => x.UserName == user);
                var feedback = new ProductFeedback();
                feedback.UserName = userName.UserName;
                feedback.ProductDetailsId = Model.FeedbackId;
                feedback.Questions = Model.Text;
                feedback.QuestionTime = DateTime.Now;
                _context.ProductsFeedback.Add(feedback);
                await _context.SaveChangesAsync();
                return Model.Text;
            }
            return null;
        }
     
        [HttpPost]
        [Route("[action]")]
        public async Task<string> AnswerQuestion(InteractionData Model)
        {
            var productId = await _context.ProductsFeedback.SingleOrDefaultAsync(x => x.Id == Model.FeedbackId);
            var product = await _context.ProductsDetails.SingleOrDefaultAsync(x => x.Id == productId.ProductDetailsId);

            if (Model.Text != null && Model.FeedbackId > 0 && product.ProductSellerId == Model.User)
            {
                var TableEntry = await _context.ProductsFeedback.SingleOrDefaultAsync(x => x.Id == Model.FeedbackId);
                TableEntry.Answar = Model.Text;
                TableEntry.AnswarTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return Model.Text;
            }
            return null;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<string> AnswerReview(InteractionData Model)
        {
            var productId = await _context.CustomerReviews.SingleOrDefaultAsync(x => x.Id == Model.FeedbackId);
            var product = await _context.ProductsDetails.SingleOrDefaultAsync(x => x.Id == productId.ProductDetailsId);

            if (Model.Text != null && Model.FeedbackId > 0 && product.ProductSellerId == Model.User)
            {
                var TableEntry = await _context.CustomerReviews.SingleOrDefaultAsync(x => x.Id == Model.FeedbackId);
                TableEntry.ReviewResponse = Model.Text;
                TableEntry.ResponseTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return Model.Text;
            }
            return null;
        }
        [HttpGet("{Id}")]
        public List<ProductFeedbackDto> AllQuestion(int Id)
        {
            var QuestionList = _context.ProductsFeedback.Where(x => x.ProductDetailsId == Id).OrderByDescending(x => x.QuestionTime).ToList()
                .Select(_mapper.Map<ProductFeedback, ProductFeedbackDto>).ToList();

            return QuestionList;
        }
    }
}
