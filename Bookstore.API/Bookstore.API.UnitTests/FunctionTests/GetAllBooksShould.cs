using Bookstore.API.Functions;
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
    public class GetAllBooksShould
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly Mock<ILogger<GetAllBooks>> _loggerMock;
        private readonly Mock<HttpRequest> _httpRequestMock;

        private GetAllBooks _func;

        public GetAllBooksShould()
        {
            _bookServiceMock = new Mock<IBookService>();
            _loggerMock = new Mock<ILogger<GetAllBooks>>();
            _httpRequestMock = new Mock<HttpRequest>();

            _func = new GetAllBooks(
                _bookServiceMock.Object,
                _loggerMock.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public async Task GetAllBooks_ReturnsBooks(int numberOfBooks)
        {
            // Arrange
            var testBooks = TestDataGenerator.GenerateBooks("TestCategory", numberOfBooks);
            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testBooks));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);

            _bookServiceMock.Setup(x => x.GetBooks()).ReturnsAsync(testBooks);

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(OkObjectResult), response.GetType());
        }

        [Fact]
        public async Task GetAllBooks_NoResults()
        {
            // Arrange
            var testBooks = TestDataGenerator.GenerateBooks("testCategory", 0);
            byte[] byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testBooks));
            MemoryStream memoryStream = new MemoryStream(byteArray);
            _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);

            _bookServiceMock.Setup(x => x.GetBooks()).ReturnsAsync(testBooks);

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

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

            _bookServiceMock.Setup(x => x.GetBooks()).ThrowsAsync(new Exception());

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(StatusCodeResult), response.GetType());
            var responseAsStatusCodeResult = (StatusCodeResult)response;
            Assert.Equal(500, responseAsStatusCodeResult.StatusCode);
        }
    }
}
