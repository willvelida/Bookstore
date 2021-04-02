using System;
using System.IO;
using System.Threading.Tasks;
using Bookstore.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bookstore.API.Functions
{
    public class GetAllBooks
    {
        private readonly IBookService _bookService;
        private readonly ILogger<GetAllBooks> _logger;

        public GetAllBooks(
            IBookService bookService,
            ILogger<GetAllBooks> logger)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [FunctionName(nameof(GetAllBooks))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Books")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                var books = await _bookService.GetBooks();

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

