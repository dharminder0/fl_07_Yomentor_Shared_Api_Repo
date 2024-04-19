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
        [Route("BookInfo")]
        public IActionResult GetBooks(int id, int type, int userId) {
            var response =  bookService.GetBooksList(id, type,userId);
            return JsonExt(response);
                 
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("updateStatus")]
        public  IActionResult UpdateStatus( int id, int statusId) {
            var response =  bookService.UpdateStatus(id,statusId);
            return JsonExt(response);

        }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bookExchange"></param>
    /// <returns></returns>
        [HttpPost]
        [Route("BooKList")]
        public async Task<IActionResult> GetBookList(BookRequestV2 bookExchange) {
            var response = await bookService.GetBooks(bookExchange);
            return JsonExt(response);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("updateBookStatus")]
        public IActionResult UpdateBookStatus(int id) {
            var response = bookService.UpdateBookStatus(id);
            return JsonExt(response);

        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteBook")]
        public IActionResult DeleteBook(int id) {
            var response = bookService.DeleteBook(id);
            return JsonExt(response);

        }

    }
}
