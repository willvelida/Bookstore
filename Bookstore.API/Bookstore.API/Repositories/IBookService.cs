using Bookstore.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookstore.API.Repositories
{
    public interface IBookService
    {
        Task AddBook(Book book);
        Task<Book> GetBook(string category, string bookId);
        Task UpdateBook(string bookId, Book book);
        Task DeleteBook(string category, string bookId);
        Task<List<Book>> GetBooks(string category);
    }
}
