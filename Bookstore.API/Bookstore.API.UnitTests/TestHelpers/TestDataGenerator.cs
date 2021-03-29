using Bogus;
using Bookstore.API.Models;
using System;
using System.Collections.Generic;

namespace Bookstore.API.UnitTests.TestHelpers
{
    public static class TestDataGenerator
    {
        public static List<Book> GenerateBooks(string category, int numberOfBooks)
        {
            var books = new Faker<Book>()
                .RuleFor(i => i.Id, (fake) => Guid.NewGuid().ToString())
                .RuleFor(i => i.BookName, (fake) => "TestBook")
                .RuleFor(i => i.Author, (fake) => "TestAuthor")
                .RuleFor(i => i.Price, (fake) => fake.Random.Decimal(10.99m, 19.99m))
                .RuleFor(i => i.Category, (fake) => category)
                .Generate(numberOfBooks);

            return books;
        }
    }
}
