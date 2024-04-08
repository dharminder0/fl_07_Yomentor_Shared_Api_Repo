using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController :BaseApiController {
        private readonly IBookService bookService;
        public BooksController(IBookService _bookService)
        {
                bookService= _bookService;  
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Upsert")]
        public async Task<IActionResult> UpsertBooks(BookRequest book) {
          var response=  await bookService.UpsertBook(book); 
            return Ok(response);

        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpsertBookExchange")]
        public async Task<IActionResult> UpsertBooks(BookExchange book) {
            var response = await bookService.UpsertBookExchange(book);
            return Ok(response);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> GetBooks() {
            var response = await bookService.GetBooksList();
            return Ok(response);

        }
        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("StateList")]
        public  IActionResult GetStateList() {
            var response =  bookService.GetState();
            return Ok(response);

        }
    }
}
