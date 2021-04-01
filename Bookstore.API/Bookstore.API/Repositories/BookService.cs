using Bookstore.API.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookstore.API.Repositories
{
    public class BookService : IBookService
    {
        private readonly CosmosClient _cosmosClient;
        private Container _bookContainer;

        public BookService(
            IConfiguration configuration,
            CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            _bookContainer = _cosmosClient.GetContainer(configuration["BookDatabaseName"], configuration["BookContainerName"]);
        }

        public async Task AddBook(Book book)
        {
            ItemRequestOptions itemRequestOptions = new ItemRequestOptions
            {
                EnableContentResponseOnWrite = false
            };

            await _bookContainer.CreateItemAsync(book,
                new PartitionKey(book.Category),
                itemRequestOptions);
        }

        public async Task DeleteBook(string category, string bookId)
        {
            await _bookContainer.DeleteItemAsync<Book>(bookId,
                new PartitionKey(category));
        }

        public async Task<Book> GetBook(string category, string bookId)
        {
            try
            {
                ItemResponse<Book> bookResponse = await _bookContainer.ReadItemAsync<Book>(
                bookId,
                new PartitionKey(category));

                return bookResponse.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<Book>> GetBooks(string category)
        {
            QueryDefinition query = new QueryDefinition(
                "SELECT * FROM Books b WHERE b.Category = @category")
                .WithParameter("@category", category);

            List<Book> books = new List<Book>();

            FeedIterator<Book> bookResultSet = _bookContainer.GetItemQueryIterator<Book>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(category)                 
                });

            while (bookResultSet.HasMoreResults)
            {
                FeedResponse<Book> queryResponse = await bookResultSet.ReadNextAsync();
                books.AddRange(queryResponse.Resource);
            }

            return books;
        }

        public async Task UpdateBook(string bookId, Book book)
        {
            await _bookContainer.ReplaceItemAsync(
                book,
                bookId,
                new PartitionKey(book.Category));
        }
    }
}
