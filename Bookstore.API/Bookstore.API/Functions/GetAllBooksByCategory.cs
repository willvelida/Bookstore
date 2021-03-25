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
    public class GetAllBooksByCategory
    {
        private readonly IBookService _bookService;
        private readonly ILogger<GetAllBooksByCategory> _logger;

        public GetAllBooksByCategory(
            IBookService bookService,
            ILogger<GetAllBooksByCategory> logger)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [FunctionName(nameof(GetAllBooksByCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Book/{category}")] HttpRequest req,
            string category)
        {
            IActionResult result;

            try
            {
                var books = await _bookService.GetBooks(category);

                result = new OkObjectResult(books);
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

