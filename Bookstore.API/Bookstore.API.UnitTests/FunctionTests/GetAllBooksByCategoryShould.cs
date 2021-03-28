using Bookstore.API.Functions;
using Bookstore.API.Models;
using Bookstore.API.Repositories;
using Bookstore.API.UnitTests.TestHelpers;
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
    public class GetAllBooksByCategoryShould
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<ILogger<GetAllBooksByCategory>> _loggerMock;
        private readonly Mock<HttpRequest> _httpRequestMock;

        private GetAllBooksByCategory _func;

        public GetAllBooksByCategoryShould()
        {
            _bookServiceMock = new Mock<IBookService>();
            _loggerMock = new Mock<ILogger<GetAllBooksByCategory>>();
            _httpRequestMock = new Mock<HttpRequest>();

            _func = new GetAllBooksByCategory(
                _bookServiceMock.Object,
                _loggerMock.Object);
        }

        [Theory]
        [InlineData("testCategory", 1)]
        [InlineData("testCategory", 2)]
        [InlineData("testCategory", 10)]
        public async Task GetAllBooksByCategory_ReturnsBooks(string category, int numberOfBooks)
        {
            // Arrange
            var testBooks = TestDataGenerator.GenerateBooks(category, numberOfBooks);
            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testBooks));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);

            _bookServiceMock.Setup(x => x.GetBooks(It.IsAny<string>())).ReturnsAsync(testBooks);

            // Act
            var response = await _func.Run(_httpRequestMock.Object, category);

            // Assert
            Assert.Equal(typeof(OkObjectResult), response.GetType());
        }

        [Fact]
        public async Task GetAllBooksByCategory_NoResults()
        {
            // Arrange
            var testBooks = TestDataGenerator.GenerateBooks("testCategory", 0);
            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testBooks));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);

            _bookServiceMock.Setup(x => x.GetBooks(It.IsAny<string>())).ReturnsAsync(testBooks);

            // Act
            var response = await _func.Run(_httpRequestMock.Object, "testCategory");

            // Assert
            Assert.Equal(typeof(OkObjectResult), response.GetType());
        }

        [Fact]
        public async Task Throw500OnInternalServerError()
        {
            // Arrange
            var testBooks = TestDataGenerator.GenerateBooks("testCategory", 0);
            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testBooks));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);

            _bookServiceMock.Setup(x => x.GetBooks(It.IsAny<string>())).ThrowsAsync(new Exception());

            // Act
            var response = await _func.Run(_httpRequestMock.Object, "testCategory");

            // Assert
            Assert.Equal(typeof(StatusCodeResult), response.GetType());
            var responseAsStatusCodeResult = (StatusCodeResult)response;
            Assert.Equal(500, responseAsStatusCodeResult.StatusCode);
        }
    }
}
