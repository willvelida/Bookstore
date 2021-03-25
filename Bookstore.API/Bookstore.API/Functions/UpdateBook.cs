using AutoMapper;
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
    public class UpdateBook
    {
        private readonly IBookService _bookService;
        private readonly ILogger<UpdateBook> _logger;
        private readonly IMapper _mapper;

        public UpdateBook(
            IBookService bookService,
            ILogger<UpdateBook> logger,
            IMapper mapper)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [FunctionName(nameof(UpdateBook))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Book/{category}/{bookId}")] HttpRequest req,
            string category,
            string bookId)
        {
            IActionResult result;

            try
            {
                var bookToUpdate = await _bookService.GetBook(category, bookId);

                if (bookToUpdate == null)
                {
                    result = new NotFoundResult();
                }
                else
                {
                    var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                    var updatedBook = JsonConvert.DeserializeObject<BookForUpdateDTO>(requestBody);

                    _mapper.Map(updatedBook, bookToUpdate);

                    await _bookService.UpdateBook(bookToUpdate.Id, bookToUpdate);

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

