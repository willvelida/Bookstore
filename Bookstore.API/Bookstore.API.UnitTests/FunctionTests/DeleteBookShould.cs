using Bookstore.API.Functions;
using Bookstore.API.Models;
using Bookstore.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bookstore.API.UnitTests.FunctionTests
{
    public class DeleteBookShould
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<ILogger<DeleteBook>> _loggerMock;
        private readonly Mock<HttpRequest> _httpRequestMock;

        private DeleteBook _func;

        public DeleteBookShould()
        {
            _bookServiceMock = new Mock<IBookService>();
            _loggerMock = new Mock<ILogger<DeleteBook>>();
            _httpRequestMock = new Mock<HttpRequest>();

            _func = new DeleteBook(
                _bookServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task DeleteBookSuccessfully()
        {
            // Arrange
            var testBook = new Book
            {
                Id = Guid.NewGuid().ToString(),
                BookName = "BookName",
                Author = "Test Author",
                Category = null,
                Price = 5.99m
            };

            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testBook));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);

            _bookServiceMock.Setup(x => x.GetBook(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(testBook);

            // Act
            var response = await _func.Run(_httpRequestMock.Object, testBook.Category, testBook.Id);

            // Assert
            Assert.Equal(typeof(NoContentResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(204, responseAsStatusCode.StatusCode);
        }

        [Fact]
        public async Task ThrowNotFoundResultWhenBookToDeleteIsNull()
        {
            // Arrange
            var testBook = new Book();
            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testBook));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);

            // Act
            var response = await _func.Run(_httpRequestMock.Object, "testCategory", "testBookId");

            // Assert
            Assert.Equal(typeof(NotFoundResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(404, responseAsStatusCode.StatusCode);
        }

        [Fact]
        public async Task Throw500OnInternalServerError()
        {
            // Arrange
            var testBook = new Book
            {
                Id = Guid.NewGuid().ToString(),
                BookName = "BookName",
                Author = "Test Author",
                Category = null,
                Price = 5.99m
            };

            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testBook));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);

            _bookServiceMock.Setup(x => x.GetBook(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            // Act
            var response = await _func.Run(_httpRequestMock.Object, testBook.Category, testBook.Id);

            // Assert
            Assert.Equal(typeof(StatusCodeResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(500, responseAsStatusCode.StatusCode);
        }
    }
}
