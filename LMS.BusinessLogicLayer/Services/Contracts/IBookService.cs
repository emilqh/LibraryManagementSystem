using LMS.BusinessLogicLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.BusinessLogicLayer.Services.Contracts
{
    public interface IBookService
    {
        List<BookDto> GetBooks();
        BookDto GetBookById(int id);
        void AddBook(CreateBookDto createBookDto);
        void UpdateBook(int id, UpdateBookDto updateBookDto);
        void DeleteBook(int id);
        List<BookDto> SearchBook(string keyword);
    }
}
