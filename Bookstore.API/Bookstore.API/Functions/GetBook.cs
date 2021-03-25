using Bookstore.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Bookstore.API.Functions
{
    public class GetBook
    {
        private readonly IBookService _bookService;
        private readonly ILogger<GetBook> _logger;

        public GetBook(
            IBookService bookService,
            ILogger<GetBook> logger)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [FunctionName(nameof(GetBook))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Book/{category}/{bookId}")] HttpRequest req,
            string category,
            string bookId)
        {
            IActionResult result;

            try
            {
                var book = await _bookService.GetBook(category, bookId);

                if (book == null)
                {
                    result = new NotFoundResult();
                }
                else
                {
                    result = new OkObjectResult(book);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception thrown: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }
    }
}

