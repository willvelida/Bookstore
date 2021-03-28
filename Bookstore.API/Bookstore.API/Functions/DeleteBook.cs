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
    public class DeleteBook
    {
        private readonly IBookService _bookService;
        private readonly ILogger<DeleteBook> _logger;

        public DeleteBook(
            IBookService bookService,
            ILogger<DeleteBook> logger)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [FunctionName(nameof(DeleteBook))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Book/{category}/{bookId}")] HttpRequest req,
            string category,
            string bookId)
        {
            IActionResult result;

            try
            {
                var bookToDelete = await _bookService.GetBook(category, bookId);

                if (bookToDelete == null)
                {
                    result = new NotFoundResult();
                }
                else
                {
                    await _bookService.DeleteBook(category, bookId);
                    result = new NoContentResult();
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

