using BookApi.Controllers;
using BookApi.Models;
using BookApi.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BookApi.Tests
{
    public class BookControllerTests
    {
        private readonly BookController _sut;
        private readonly Mock<IBookRepository> bookRepositoryStub = new Mock<IBookRepository>();
        private readonly Random rand = new Random();

        public BookControllerTests()
        {
            _sut = new BookController(bookRepositoryStub.Object);
        }
        [Fact]
        public async Task Get_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            bookRepositoryStub.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((Book)null);

            // Act
            var result = await _sut.GetBook(3) as NotFoundResult;

            // Assert
            //Assert.Equal(404, result.StatusCode); xUnit assertion
            result.StatusCode.Should().Be(404); // fluent assertions
        }

        [Fact]
        public async Task Get_WithValidId_ShouldReturnBook()
        {
            // Arrange
            var expectedBook = CreateRandomBook();
            bookRepositoryStub.Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(expectedBook);

            // Act
            var result = await _sut.GetBook(rand.Next(500)) as OkObjectResult;

            // Assert
            Assert.IsType<Book>(result.Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Get_ShouldReturnAllBooks()
        {
            // Arrange
            var expectedBooks = new[] { CreateRandomBook(), CreateRandomBook(), CreateRandomBook() };

            bookRepositoryStub.Setup(repo => repo.Get()).ReturnsAsync(expectedBooks);
            // Act
            var actualBooks = await _sut.GetBooks();
            // Assert
            actualBooks.Should().BeEquivalentTo(expectedBooks);
        }

        [Fact]
        public async Task CreateBook_ShouldReturnCreatedBook()
        {
            // Arrange
            var bookToCreate = new Book()
            {
                //Id = rand.Next(200),
                Author = "JK Rowling",
                Title = "Harry Potter and the Goblet of Fire",
                Description = "Harry unwittingly joins the tri-wizard tournament"
            };
            bookRepositoryStub.Setup(repo => repo.Create(bookToCreate)).ReturnsAsync(bookToCreate);
            // Act
            var result = await _sut.CreateBook(bookToCreate) as CreatedAtActionResult;
            // Assert
            //Assert.Equal(201, result.StatusCode);
            var createdBook = result.Value as Book;
            createdBook.Should().BeEquivalentTo(bookToCreate);

        }

        private Book CreateRandomBook()
        {
            return new Book
            {
                Id = rand.Next(1000),
                Title = Guid.NewGuid().ToString(),
                Author = "J.K Rowling",
                Description = Guid.NewGuid().ToString()
            };
        }
    }
}
