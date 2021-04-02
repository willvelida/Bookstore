using Bookstore.API.Models;
using Bookstore.API.Repositories;
using Bookstore.API.UnitTests.TestHelpers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bookstore.API.UnitTests.RepositoryTests
{
    public class BookServiceShould
    {
        private readonly Mock<CosmosClient> _cosmosClientMock;
        private readonly Mock<Container> _bookContainerMock;
        private readonly Mock<IConfiguration> _configMock;

        private BookService _sut;

        public BookServiceShould()
        {
            _cosmosClientMock = new Mock<CosmosClient>();
            _bookContainerMock = new Mock<Container>();
            _cosmosClientMock.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>())).Returns(_bookContainerMock.Object);
            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(x => x["BookDatabaseName"]).Returns("db");
            _configMock.Setup(x => x["BookContainerName"]).Returns("col");

            _sut = new BookService(
                _configMock.Object,
                _cosmosClientMock.Object);
        }

        [Fact]
        public async Task CreateItemSucessfully()
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

            _bookContainerMock.SetupCreateItemAsync<Book>();

            // Act
            await _sut.AddBook(testBook);

            // Assert
            _bookContainerMock.Verify(x => x.CreateItemAsync(
                It.IsAny<Book>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteItemSuccessfully()
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

            _bookContainerMock.SetupDeleteItemAsync<Book>();

            // Act
            await _sut.DeleteBook(testBook.Category, testBook.Id);

            // Assert
            _bookContainerMock.Verify(x => x.DeleteItemAsync<Book>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetItemSuccessfully()
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


            _bookContainerMock.SetupReadItemAsync(testBook);

            // Act
            var response = await _sut.GetBook(testBook.Author, testBook.Id);

            // Assert
            _bookContainerMock.Verify(x => x.ReadItemAsync<Book>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal(testBook.Author, response.Author);
            Assert.Equal(testBook.BookName, response.BookName);
            Assert.Equal(testBook.Category, response.Category);
            Assert.Equal(testBook.Price, response.Price);
        }

        [Fact]
        public async Task ReturnNullWhenItemIsNotFound()
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

            _bookContainerMock.SetupReadItemAsyncNotFound<Book>();

            // Act
            var response = await _sut.GetBook("author", "id");

            // Assert
            _bookContainerMock.Verify(x => x.ReadItemAsync<Book>(
               It.IsAny<string>(),
               It.IsAny<PartitionKey>(),
               It.IsAny<ItemRequestOptions>(),
               It.IsAny<CancellationToken>()), Times.Once);

            Assert.Null(response);
        }

        [Fact]
        public async Task GetAllItemsByCategory()
        {
            // Arrange
            var testBooks = new List<Book>();
            var testBook = new Book
            {
                Id = Guid.NewGuid().ToString(),
                BookName = "BookName",
                Author = "Test Author",
                Category = "Test Category",
                Price = 5.99m
            };
            testBooks.Add(testBook);

            _bookContainerMock.SetupItemQueryIteratorMock(testBooks);
            _bookContainerMock.SetupItemQueryIteratorMock(new List<int> { 1 });

            // Act
            var response = await _sut.GetBooks(testBook.Category);

            // Assert
            Assert.Equal(testBooks.Count, response.Count);
        }

        [Fact]
        public async Task GellAllItemsByCategory_NoItemsReturned()
        {
            // Arrange
            var noBooksInList = new List<Book>();

            var getBooks = _bookContainerMock.SetupItemQueryIteratorMock(noBooksInList);
            getBooks.feedIterator.Setup(x => x.HasMoreResults).Returns(false);

            _bookContainerMock.SetupItemQueryIteratorMock(new List<int>() { 0 });

            // Act
            var response = await _sut.GetBooks("test category");

            // Assert
            Assert.Empty(response);
        }

        [Fact]
        public async Task GetAllItems()
        {
            // Arrange
            var testBooks = new List<Book>();
            var testBook = new Book
            {
                Id = Guid.NewGuid().ToString(),
                BookName = "BookName",
                Author = "Test Author",
                Category = "Test Category",
                Price = 5.99m
            };
            testBooks.Add(testBook);

            _bookContainerMock.SetupItemQueryIteratorMock(testBooks);
            _bookContainerMock.SetupItemQueryIteratorMock(new List<int> { 1 });

            // Act
            var response = await _sut.GetBooks();

            // Assert
            Assert.Equal(testBooks.Count, response.Count);
        }

        [Fact]
        public async Task GellAllItems_NoItemsReturned()
        {
            // Arrange
            var noBooksInList = new List<Book>();

            var getBooks = _bookContainerMock.SetupItemQueryIteratorMock(noBooksInList);
            getBooks.feedIterator.Setup(x => x.HasMoreResults).Returns(false);

            _bookContainerMock.SetupItemQueryIteratorMock(new List<int>() { 0 });

            // Act
            var response = await _sut.GetBooks();

            // Assert
            Assert.Empty(response);
        }

        [Fact]
        public async Task ReplaceItemSuccessfully()
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

            _bookContainerMock.SetupReplaceItemAsync<Book>();

            // Act
            await _sut.UpdateBook(testBook.Id, testBook);

            // Assert
            _bookContainerMock.Verify(x => x.ReplaceItemAsync(
                It.IsAny<Book>(),
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
