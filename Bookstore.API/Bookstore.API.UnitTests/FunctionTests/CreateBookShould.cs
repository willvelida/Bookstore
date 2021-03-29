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
    public class CreateBookShould
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<ILogger<CreateBook>> _loggerMock;
        private readonly Mock<HttpRequest> _httpRequestMock;

        private CreateBook _func;

        public CreateBookShould()
        {
            _bookServiceMock = new Mock<IBookService>();
            _loggerMock = new Mock<ILogger<CreateBook>>();
            _httpRequestMock = new Mock<HttpRequest>();

            _func = new CreateBook(
                _bookServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CreateNewBook()
        {
            // Arrange
            var testBook = new Book
            {
                Id = Guid.NewGuid().ToString(),
                BookName = "BookName",
                Author = "Test Author",
                Category = "Test Category",
                Price = 5.99m
            };

            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testBook));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);

            _bookServiceMock.Setup(x => x.AddBook(testBook)).Returns(Task.CompletedTask);

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(OkResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(200, responseAsStatusCode.StatusCode);

            _bookServiceMock.Verify(x => x.AddBook(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task Throw400OnBadRequest()
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

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(BadRequestResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(400, responseAsStatusCode.StatusCode);
        }

        [Fact]
        public async Task Throw500OnInternalServerError()
        {
            // Arrange
            var testBook = new Book
            {
                Category = "Test Category"
            };

            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testBook));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);

            _bookServiceMock.Setup(x => x.AddBook(It.IsAny<Book>())).ThrowsAsync(new Exception());

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(StatusCodeResult), response.GetType());
            var responseAsStatusCodeResult = (StatusCodeResult)response;
            Assert.Equal(500, responseAsStatusCodeResult.StatusCode);
        }
    }
}
