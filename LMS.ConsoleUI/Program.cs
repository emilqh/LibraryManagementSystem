using LMS.BusinessLogicLayer.Dtos;
using LMS.BusinessLogicLayer.Services;
using LMS.DataAccessLayer.Entities;

namespace LMS.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== LIBRARY MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. Books");
                Console.WriteLine("2. Categories");
                Console.WriteLine("3. Members");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BookMenu();
                        break;
                    case "2":
                        CategoryMenu();
                        break;
                    case "3":
                        MemberMenu();
                        Pause();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }

            //Book Menu
            static void BookMenu()
            {
                BookManager bookManager = new BookManager();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("===== BOOK MENU =====");
                    Console.WriteLine("1. List all books");
                    Console.WriteLine("2. Get book by ID");
                    Console.WriteLine("3. Add new book");
                    Console.WriteLine("4. Update book");
                    Console.WriteLine("5. Delete book");
                    Console.WriteLine("6. Search book");
                    Console.WriteLine("0. Back");
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

            //CATEGORY MENU

            static void CategoryMenu()
            {
                CategoryManager categoryManager = new CategoryManager();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("===== CATEGORY MENU =====");
                    Console.WriteLine("1. List all categories");
                    Console.WriteLine("2. Get category by ID");
                    Console.WriteLine("3. Add new category");
                    Console.WriteLine("4. Update category");
                    Console.WriteLine("5. Delete category");
                    Console.WriteLine("6. Search category");
                    Console.WriteLine("0. Back");
                    Console.Write("Select an option: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ListCategories(categoryManager);
                            break;
                        case "2":
                            GetCategoryById(categoryManager);
                            break;
                        case "3":
                            AddCategory(categoryManager);
                            break;
                        case "4":
                            UpdateCategory(categoryManager);
                            break;
                        case "5":
                            DeleteCategory(categoryManager);
                            break;
                        case "6":
                            SearchCategory(categoryManager);
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

            // ===== CATEGORY OPERATIONS =====

            static void ListCategories(CategoryManager categoryManager)
            {
                Console.Clear();
                var categories = categoryManager.GetCategories();

                if (categories.Count == 0)
                {
                    Console.WriteLine("No categories found.");
                }
                else
                {
                    PrintCategoriesTable(categories);
                }

                Pause();
            }

            static void GetCategoryById(CategoryManager categoryManager)
            {
                Console.Clear();
                Console.Write("Enter category ID: ");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid ID.");
                    Pause();
                    return;
                }

                var category = categoryManager.GetCategoryById(id);

                if (category == null)
                {
                    Console.WriteLine("Category not found.");
                }
                else
                {
                    PrintCategoriesTable(new List<CategoryDto> { category });
                }

                Pause();
            }

            static void AddCategory(CategoryManager categoryManager)
            {
                Console.Clear();

                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Description: ");
                string description = Console.ReadLine();

                try
                {
                    CreateCategoryDto dto = new CreateCategoryDto
                    {
                        Name = name,
                        Description = description
                    };

                    categoryManager.AddCategory(dto);
                    Console.WriteLine("Category added successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Pause();
            }

            static void UpdateCategory(CategoryManager categoryManager)
            {
                Console.Clear();
                Console.Write("Enter category ID to update: ");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid ID.");
                    Pause();
                    return;
                }

                var existingCategory = categoryManager.GetCategoryById(id);

                if (existingCategory == null)
                {
                    Console.WriteLine("Category not found.");
                    Pause();
                    return;
                }

                Console.WriteLine("Press ENTER to keep existing value.");

                Console.Write($"New Name ({existingCategory.Name}): ");
                string nameInput = Console.ReadLine();
                string name = string.IsNullOrWhiteSpace(nameInput)
                    ? existingCategory.Name
                    : nameInput;

                Console.Write($"New Description ({existingCategory.Description}): ");
                string descInput = Console.ReadLine();
                string description = string.IsNullOrWhiteSpace(descInput)
                    ? existingCategory.Description
                    : descInput;

                try
                {
                    UpdateCategoryDto dto = new UpdateCategoryDto
                    {
                        Name = name,
                        Description = description
                    };

                    categoryManager.UpdateCategory(id, dto);
                    Console.WriteLine("Category updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Pause();
            }

            static void DeleteCategory(CategoryManager categoryManager)
            {
                Console.Clear();
                Console.Write("Enter category ID to delete: ");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid ID.");
                    Pause();
                    return;
                }

                try
                {
                    categoryManager.DeleteCategory(id);
                    Console.WriteLine("Category deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Pause();
            }

            static void SearchCategory(CategoryManager categoryManager)
            {
                Console.Clear();
                Console.Write("Enter search keyword: ");
                string keyword = Console.ReadLine();

                var categories = categoryManager.SearchCategory(keyword);

                if (categories.Count == 0)
                {
                    Console.WriteLine("No matching categories found.");
                }
                else
                {
                    PrintCategoriesTable(categories);
                }

                Pause();
            }

            // ===== HELPERS =====            

            static void PrintCategoriesTable(List<CategoryDto> categories)
            {
                Console.WriteLine();
                Console.WriteLine($"{"ID",-4} | {"Name",-25} | {"Description",-40}");
                Console.WriteLine(new string('-', 75));

                foreach (var category in categories)
                {
                    Console.WriteLine(
                        $"{category.Id,-4} | " +
                        $"{Truncate(category.Name, 25),-25} | " +
                        $"{Truncate(category.Description, 40),-40}");
                }

                Console.WriteLine();
            }

            //MEMBER Menu

            static void MemberMenu()
            {
                MemberManager memberManager = new MemberManager();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("===== MEMBER MENU =====");
                    Console.WriteLine("1. List all members");
                    Console.WriteLine("2. Get member by ID");
                    Console.WriteLine("3. Add new member");
                    Console.WriteLine("4. Update member");
                    Console.WriteLine("5. Delete member");
                    Console.WriteLine("6. Search member");
                    Console.WriteLine("0. Back");
                    Console.Write("Select an option: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ListMembers(memberManager);
                            break;
                        case "2":
                            GetMemberById(memberManager);
                            break;
                        case "3":
                            AddMember(memberManager);
                            break;
                        case "4":
                            UpdateMember(memberManager);
                            break;
                        case "5":
                            DeleteMember(memberManager);
                            break;
                        case "6":
                            SearchMember(memberManager);
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

            static void ListMembers(MemberManager memberManager)
            {
                Console.Clear();
                var members = memberManager.GetMembers();

                if (members.Count == 0)
                {
                    Console.WriteLine("No members found.");
                }
                else
                {
                    PrintMembersTable(members);
                }

                Pause();
            }

            static void GetMemberById(MemberManager memberManager)
            {
                Console.Clear();
                Console.Write("Enter member ID: ");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid ID.");
                    Pause();
                    return;
                }

                var member = memberManager.GetMemberById(id);

                if (member == null)
                {
                    Console.WriteLine("Member not found.");
                }
                else
                {
                    PrintMembersTable(new List<MemberDto> { member });
                }

                Pause();
            }

            static void AddMember(MemberManager memberManager)
            {
                while (true)
                {
                    Console.Clear();

                    Console.Write("Full Name: ");
                    string fullName = Console.ReadLine();

                    Console.Write("Email: ");
                    string email = Console.ReadLine();

                    Console.Write("Phone Number (10 digits): ");
                    string phone = Console.ReadLine();

                    try
                    {
                        CreateMemberDto dto = new CreateMemberDto
                        {
                            FullName = fullName,
                            Email = email,
                            PhoneNumber = phone
                        };

                        memberManager.AddMember(dto);
                        Console.WriteLine("Member added successfully.");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine("Please re-enter the member details.");
                        Pause();
                    }
                }
            }

            static void UpdateMember(MemberManager memberManager)
            {
                Console.Clear();
                Console.Write("Enter member ID to update: ");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid ID.");
                    Pause();
                    return;
                }

                var existing = memberManager.GetMemberById(id);
                if (existing == null)
                {
                    Console.WriteLine("Member not found.");
                    Pause();
                    return;
                }

                Console.WriteLine("Press ENTER to keep existing value.");

                Console.Write($"Full Name ({existing.FullName}): ");
                string nameInput = Console.ReadLine();
                string name = string.IsNullOrWhiteSpace(nameInput) ? existing.FullName : nameInput;

                Console.Write($"Email ({existing.Email}): ");
                string emailInput = Console.ReadLine();
                string email = string.IsNullOrWhiteSpace(emailInput) ? existing.Email : emailInput;

                Console.Write($"Phone ({existing.PhoneNumber}): ");
                string phoneInput = Console.ReadLine();
                string phone = string.IsNullOrWhiteSpace(phoneInput) ? existing.PhoneNumber : phoneInput;

                Console.Write($"Is Active (1=Yes, 0=No) ({(existing.IsActive ? "1" : "0")}): ");
                string activeInput = Console.ReadLine();
                bool isActive = string.IsNullOrWhiteSpace(activeInput)
                    ? existing.IsActive
                    : activeInput == "1";

                try
                {
                    UpdateMemberDto dto = new UpdateMemberDto
                    {
                        FullName = name,
                        Email = email,
                        PhoneNumber = phone,
                        IsActive = isActive
                    };

                    memberManager.UpdateMember(id, dto);
                    Console.WriteLine("Member updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Pause();
            }

            static void DeleteMember(MemberManager memberManager)
            {
                Console.Clear();
                Console.Write("Enter member ID to delete: ");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid ID.");
                    Pause();
                    return;
                }

                try
                {
                    memberManager.DeleteMember(id);
                    Console.WriteLine("Member deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Pause();
            }

            static void SearchMember(MemberManager memberManager)
            {
                Console.Clear();
                Console.Write("Enter search keyword: ");
                string keyword = Console.ReadLine();

                var members = memberManager.SearchMember(keyword);

                if (members.Count == 0)
                {
                    Console.WriteLine("No matching members found.");
                }
                else
                {
                    PrintMembersTable(members);
                }

                Pause();
            }

            // ===== HELPERS =====

            static void PrintMembersTable(List<MemberDto> members)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"{"ID",-4} | {"Full Name",-25} | {"Email",-25} | {"Phone",-15} | {"Active",-6}");
                Console.WriteLine(new string('-', 86));

                foreach (var m in members)
                {
                    Console.WriteLine(
                        $"{m.Id,-4} | " +
                        $"{Truncate(m.FullName, 25),-25} | " +
                        $"{Truncate(m.Email, 25),-25} | " +
                        $"{Truncate(m.PhoneNumber, 15),-15} | " +
                        $"{(m.IsActive ? "Yes" : "No"),-6}");
                }

                Console.WriteLine();
            }
        }
    }
}
