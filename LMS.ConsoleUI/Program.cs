using LMS.BusinessLogicLayer.Dtos;
using LMS.BusinessLogicLayer.Services;
using LMS.DataAccessLayer.Entities;

namespace LMS.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookManager bookManager = new BookManager();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== LIBRARY MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. List all books");
                Console.WriteLine("2. Get book by ID");
                Console.WriteLine("3. Add new book");
                Console.WriteLine("4. Update book");
                Console.WriteLine("5. Delete book");
                Console.WriteLine("6. Search book");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListBooks(bookManager);
                        break;
                    case "2":
                        GetBookById(bookManager);
                        break;
                    case "3":
                        AddBook(bookManager);
                        break;
                    case "4":
                        UpdateBook(bookManager);
                        break;
                    case "5":
                        DeleteBook(bookManager);
                        break;
                    case "6":
                        SearchBook(bookManager);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }
        }

        // ===== BOOK OPERATIONS =====

        static void ListBooks(BookManager bookManager)
        {
            Console.Clear();
            var books = bookManager.GetBooks();

            if (books.Count == 0)
            {
                Console.WriteLine("No books found.");
            }
            else
            {               
                PrintBooksTable(books);                
            }

            Pause();
        }

        static void GetBookById(BookManager bookManager)
        {
            Console.Clear();
            Console.Write("Enter book ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Pause();
                return;
            }

            var book = bookManager.GetBookById(id);

            if (book == null)
            {
                Console.WriteLine("Book not found.");
            }
            else
            {
                PrintBooksTable(new List<BookDto> { book });
            }

            Pause();
        }

        static void AddBook(BookManager bookManager)
        {
            Console.Clear();

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Author: ");
            string author = Console.ReadLine();

            Console.Write("ISBN: ");
            string isbn = Console.ReadLine();

            Console.Write("Published Year: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Invalid year.");
                Pause();
                return;
            }

            Console.Write("Category ID: ");
            if (!int.TryParse(Console.ReadLine(), out int categoryId))
            {
                Console.WriteLine("Invalid category ID.");
                Pause();
                return;
            }

            CreateBookDto dto = new CreateBookDto
            {
                Title = title,
                Author = author,
                ISBN = isbn,
                PublishedYear = year,
                CategoryId = categoryId
            };

            bookManager.AddBook(dto);

            Console.WriteLine("Book added successfully.");
            Pause();
        }

        static void UpdateBook(BookManager bookManager)
        {
            Console.Clear();
            Console.Write("Enter book ID to update: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Pause();
                return;
            }

            var existingBook = bookManager.GetBookById(id);

            if (existingBook == null)
            {
                Console.WriteLine("Book not found.");
                Pause();
                return;
            }

            Console.WriteLine("Press ENTER to keep existing value.");

            Console.Write($"New Title ({existingBook.Title}): ");
            string titleInput = Console.ReadLine();
            string title = string.IsNullOrWhiteSpace(titleInput)
                ? existingBook.Title
                : titleInput;

            Console.Write($"New Author ({existingBook.Author}): ");
            string authorInput = Console.ReadLine();
            string author = string.IsNullOrWhiteSpace(authorInput)
                ? existingBook.Author
                : authorInput;

            Console.Write($"New Category ID ({existingBook.CategoryId}): ");
            string categoryInput = Console.ReadLine();
            int categoryId = string.IsNullOrWhiteSpace(categoryInput)
                ? existingBook.CategoryId
                : int.Parse(categoryInput);

            Console.Write($"Is Available (1=Yes, 0=No) ({(existingBook.IsAvailable ? "1" : "0")}): ");
            string availabilityInput = Console.ReadLine();
            bool isAvailable = string.IsNullOrWhiteSpace(availabilityInput)
                ? existingBook.IsAvailable
                : availabilityInput == "1";

            UpdateBookDto dto = new UpdateBookDto
            {
                Title = title,
                Author = author,
                CategoryId = categoryId,
                IsAvailable = isAvailable
            };

            bookManager.UpdateBook(id, dto);

            Console.WriteLine("Book updated successfully.");
            Pause();
        }

        static void DeleteBook(BookManager bookManager)
        {
            Console.Clear();
            Console.Write("Enter book ID to delete: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                Pause();
                return;
            }

            bookManager.DeleteBook(id);
            Console.WriteLine("Book deleted (if it existed).");
            Pause();
        }

        static void SearchBook(BookManager bookManager)
        {
            Console.Clear();
            Console.Write("Enter search keyword: ");
            string keyword = Console.ReadLine();

            var books = bookManager.SearchBook(keyword);

            if (books.Count == 0)
            {
                Console.WriteLine("No matching books found.");
            }
            else
            {
                PrintBooksTable(books);               
            }

            Pause();
        }

        // ===== HELPERS =====

        static void PrintBooksTable(List<BookDto> books)
        {
            Console.WriteLine();
            Console.WriteLine(
                $"{"ID",-4} | {"Title",-25} | {"Author",-22} | {"Year",-4} | {"Cat",-3} | {"Avl",-3}");
            Console.WriteLine(new string('-', 72));

            foreach (var book in books)
            {
                Console.WriteLine(
                    $"{book.Id,-4} | " +
                    $"{Truncate(book.Title, 25),-25} | " +
                    $"{Truncate(book.Author, 22),-22} | " +
                    $"{book.PublishedYear,-4} | " +
                    $"{book.CategoryId,-3} | " +
                    $"{(book.IsAvailable ? "Yes" : "No"),-3}");
            }

            Console.WriteLine();
        }
        static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Length <= maxLength
                ? value
                : value.Substring(0, maxLength - 3) + "...";
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
