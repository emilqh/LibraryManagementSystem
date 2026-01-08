using LMS.DataAccessLayer.Entities;

namespace LMS.DataAccessLayer
{
    public class BookRepository : IRepository<Book>
    {
        //[Id:5][Title:30][Author:25][ISBN:13][Year:4][CategoryId:5][IsAvailable:1],[Total:83]

        //Lengths
        private const int ID_LENGTH = 5;
        private const int TITLE_LENGTH = 30;
        private const int AUTHOR_LENGTH = 25;
        private const int ISBN_LENGTH = 13;
        private const int YEAR_LENGTH = 4;
        private const int CATEGORY_ID_LENGTH = 5;
        private const int ISAVAILABLE_LENGTH = 1;
        private const int TOTAL_LINE_LENGTH = 83;

        //Start indexes
        private const int ID_START = 0;
        private const int TITLE_START = ID_START + ID_LENGTH;
        private const int AUTHOR_START = TITLE_START + TITLE_LENGTH;
        private const int ISBN_START = AUTHOR_START + AUTHOR_LENGTH;
        private const int YEAR_START = ISBN_START + ISBN_LENGTH;
        private const int CATEGORY_ID_START = YEAR_START + YEAR_LENGTH;
        private const int ISAVAILABLE_START = CATEGORY_ID_START + CATEGORY_ID_LENGTH;

        //Define a book line in books.txt
        private Book ParseLineToBook(string line)        
        {         
            int id = int.Parse(line.Substring(ID_START, ID_LENGTH).Trim());
            string title = line.Substring(TITLE_START, TITLE_LENGTH).Trim();
            string author = line.Substring(AUTHOR_START, AUTHOR_LENGTH).Trim();
            string isbn = line.Substring(ISBN_START, ISBN_LENGTH).Trim();
            int publishedYear = int.Parse(line.Substring(YEAR_START, YEAR_LENGTH).Trim());
            int categoryId = int.Parse(line.Substring(CATEGORY_ID_START, CATEGORY_ID_LENGTH).Trim());

            char isAvailableChar = line.Substring(ISAVAILABLE_START, ISAVAILABLE_LENGTH)[0];
            bool isAvailable = isAvailableChar == '1';

            Book book = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                ISBN = isbn,
                PublishedYear = publishedYear,
                CategoryId = categoryId,
                IsAvailable = isAvailable
            };

            return book;
        }

        private const string PATH = "Data\\books.txt";              

        public BookRepository() 
        {
            //Setup path ("\Data\books.txt")
            string directoryPath = Path.GetDirectoryName(PATH);

            //Create "Data" directory if not exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            //Create "books.txt" file if not exists
            if (!File.Exists(PATH)) 
            { 
                File.Create(PATH).Close();
            }
        }

        public void Add(Book book)
        {
            List<Book> books = GetAll();

            int newId;

            if (books.Count == 0)
            {
                newId = 1;
            }

            else
            {
                newId = books.Max(x => x.Id) + 1;
            }

            book.Id = newId;

            //[Id:5][Title:30][Author:25][ISBN:13][Year:4][CategoryId:5][IsAvailable:1],[Total:83]
            string line =
                newId.ToString().PadLeft(ID_LENGTH, '0') +
                book.Title.PadRight(TITLE_LENGTH) +
                book.Author.PadRight(AUTHOR_LENGTH) +
                book.ISBN.PadRight(ISBN_LENGTH) +
                book.PublishedYear.ToString().PadLeft(YEAR_LENGTH, '0') +
                book.CategoryId.ToString().PadLeft(CATEGORY_ID_LENGTH, '0') +
                (book.IsAvailable ? "1" : "0");

            if (line.Length != TOTAL_LINE_LENGTH)
            {
                throw new Exception("Invalid book line legth.");
            }

            File.AppendAllText(PATH, line + Environment.NewLine);

        }

        public void Delete(int id)
        {
            List<Book> books = GetAll();

            bool isRemoved = false;
                              
            for (int i = 0; i < books.Count; i++) 
            {
                if (books[i].Id == id)
                { 
                    books.RemoveAt(i);
                    isRemoved = true;
                    break;
                }
            }

            if (!isRemoved)
            {
                return; // Book id was not found
            }

            List<string> lines = new List<string>();

            foreach (Book b in books)
            {
                string line =
                    b.Id.ToString().PadLeft(ID_LENGTH, '0') +
                    b.Title.PadRight(TITLE_LENGTH) +
                    b.Author.PadRight(AUTHOR_LENGTH) +
                    b.ISBN.PadRight(ISBN_LENGTH) +
                    b.PublishedYear.ToString().PadLeft(YEAR_LENGTH, '0') +
                    b.CategoryId.ToString().PadLeft(CATEGORY_ID_LENGTH, '0') +
                    (b.IsAvailable ? "1" : "0");

                if (line.Length != TOTAL_LINE_LENGTH)
                {
                    throw new Exception("Invalid book line legth.");
                }

                lines.Add(line);
            }

            File.WriteAllLines(PATH, lines);
        }

        public List<Book> GetAll()
        {
            List<Book> books = new List<Book>();

            string[] lines = File.ReadAllLines(PATH);

            foreach (string line in lines)
            {
                //if line is empty skip
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.Length != TOTAL_LINE_LENGTH)
                    continue;
                
                Book book = ParseLineToBook(line);
                books.Add(book);                
            }
            return books;
        }

        public Book GetById(int id)
        {
            List<Book> books = GetAll();
            foreach (Book book in books)
            {
                if (book.Id == id)
                {
                    return book;                    
                }
            }

            return null;
        }

        public List<Book> Search(string keyword)
        {
            List<Book> books = GetAll();
            List<Book> result = new List<Book>();

            if (string.IsNullOrWhiteSpace(keyword)) 
            {
                return result;
            }

            keyword = keyword.ToLower();

            foreach (Book book in books) 
            {
                if (book.Title.ToLower().Contains(keyword) ||
                    book.Author.ToLower().Contains(keyword) ||
                    book.ISBN.ToLower().Contains(keyword))
                {
                    result.Add(book);
                }
            }

            return result;

        }

        public void Update(Book book)
        {
            
            List<Book> books = GetAll();

            bool isUpdated = false;

            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].Id == book.Id)
                {
                    books[i] = book;
                    isUpdated = true;
                    break;
                }
            }

            if (!isUpdated)
            {
                return; //Book was not found
            }

            List<string> lines = new List<string>();

            foreach (Book b in books)
            {
                string line =
                    b.Id.ToString().PadLeft(ID_LENGTH, '0') +
                    b.Title.PadRight(TITLE_LENGTH) +
                    b.Author.PadRight(AUTHOR_LENGTH) +
                    b.ISBN.PadRight(ISBN_LENGTH) +
                    b.PublishedYear.ToString().PadLeft(YEAR_LENGTH, '0') +
                    b.CategoryId.ToString().PadLeft(CATEGORY_ID_LENGTH, '0') +
                    (b.IsAvailable ? "1" : "0");

                if (line.Length != TOTAL_LINE_LENGTH)
                {
                    throw new Exception("Invalid book line legth.");
                }

                lines.Add(line);
            }

            File.WriteAllLines(PATH, lines);
        }
    }
}
