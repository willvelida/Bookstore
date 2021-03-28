using Bookstore.API.Functions;
using Bookstore.API.Models;
using Bookstore.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bookstore.API.UnitTests.FunctionTests
{
    public class GetBookShould
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<ILogger<GetBook>> _loggerMock;
        private readonly Mock<HttpRequest> _httpRequestMock;

        private GetBook _func;

        public GetBookShould()
        {
            _bookServiceMock = new Mock<IBookService>();
            _loggerMock = new Mock<ILogger<GetBook>>();
            _httpRequestMock = new Mock<HttpRequest>();

            _func = new GetBook(
                _bookServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetBookSuccessfully()
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

            _bookServiceMock.Setup(x => x.GetBook(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(testBook);

            // Act
            var response = await _func.Run(_httpRequestMock.Object, testBook.Category, testBook.Id);

            // Assert
            Assert.Equal(typeof(OkObjectResult), response.GetType());
        }

        [Fact]
        public async Task Throw404WhenBookIsNotFound()
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

            // Act
            var response = await _func.Run(_httpRequestMock.Object, "testCategory", "testId");

            // Assert
            Assert.Equal(typeof(NotFoundResult), response.GetType());
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
                Category = "Test Category",
                Price = 5.99m
            };
            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testBook));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);

            _bookServiceMock.Setup(x => x.GetBook(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception());

            // Act
            var response = await _func.Run(_httpRequestMock.Object, "testCategory", "testId");

            // Assert
            Assert.Equal(typeof(StatusCodeResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(500, responseAsStatusCode.StatusCode);
        }
    }
}
