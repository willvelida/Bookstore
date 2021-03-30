using Bookstore.API.Models;
using Bookstore.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bookstore.API.Functions
{
    public class CreateBook
    {
        private readonly IBookService _bookService;
        private readonly ILogger<CreateBook> _logger;

        public CreateBook(
            IBookService bookService,
            ILogger<CreateBook> logger)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [FunctionName(nameof(CreateBook))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Book")] HttpRequest req)
        {
            IActionResult result;

            try
            {
                var request = await new StreamReader(req.Body).ReadToEndAsync();

                var book = JsonConvert.DeserializeObject<Book>(request);

                if (string.IsNullOrWhiteSpace(book.Category))
                {
                    result = new BadRequestResult();
                }
                else
                {
                    book.Id = Guid.NewGuid().ToString();
                    await _bookService.AddBook(book);
                    result = new OkResult();
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

