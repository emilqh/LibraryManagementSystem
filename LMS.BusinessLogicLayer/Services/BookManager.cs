using LMS.BusinessLogicLayer.Dtos;
using LMS.BusinessLogicLayer.Services.Contracts;
using LMS.DataAccessLayer.Entities;
using LMS.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.BusinessLogicLayer.Services
{
    public class BookManager : IBookService
    {
        private readonly BookRepository _bookRepository;

        public BookManager()
        {
            _bookRepository = new BookRepository();
        }

        public List<BookDto> GetBooks()
        {
            List<Book> books = _bookRepository.GetAll();
            List<BookDto> result = new List<BookDto>();

            foreach (Book book in books)
            {
                result.Add(MapToDto(book));
            }

            return result;
        }

        public BookDto? GetBookById(int id)
        {
            Book book = _bookRepository.GetById(id);

            if (book is null)
            { 
                return null;
            }

            return MapToDto(book);            
        }

        public void AddBook(CreateBookDto createBookDto)
        {
            _bookRepository.Add(new Book
            {
                Title = createBookDto.Title,
                Author = createBookDto.Author,
                ISBN = createBookDto.ISBN,
                PublishedYear = createBookDto.PublishedYear,
                CategoryId = createBookDto.CategoryId            
            });
        }

        public void UpdateBook(int id, UpdateBookDto updateBookDto)
        {
            ArgumentNullException.ThrowIfNull(updateBookDto);

            if (string.IsNullOrWhiteSpace(updateBookDto.Title))
                throw new Exception("Book title is required.");

            if (updateBookDto.Title.Length > 30)
                throw new Exception("Book title cannot exceed 30 characters.");

            if (updateBookDto.Author.Length > 25)
                throw new Exception("Author name cannot exceed 25 characters.");

            Book book = _bookRepository.GetById(id);

            if (book is null)
            {
                return;
            }

            book.Title = updateBookDto.Title.Trim();
            book.Author = updateBookDto.Author.Trim();
            book.CategoryId = updateBookDto.CategoryId;
            book.IsAvailable = updateBookDto.IsAvailable;

            _bookRepository.Update(book);
        }

        public void DeleteBook(int id)
        {
            Book book = _bookRepository.GetById(id);

            if (book is null)
            {
                return;
            }

            _bookRepository.Delete(id);
        }

        public List<BookDto> SearchBook(string keyword)
        {            
            List<BookDto> result = new List<BookDto>();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return result;
            }

            List<Book> books = _bookRepository.Search(keyword);

            foreach (Book book in books)
            {
                result.Add(MapToDto(book));
            }

            return result;
        }

        //One centralized mapping method to use in other methods
        private BookDto MapToDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublishedYear = book.PublishedYear,
                CategoryId = book.CategoryId,
                IsAvailable = book.IsAvailable
            };
        }
    }
}
