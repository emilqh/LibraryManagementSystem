using LMS.DataAccessLayer.Entities;
using System.IO;
using System.Linq;

namespace LMS.DataAccessLayer
{
    public class CategoryRepository : IRepository<Category>
    {     
        //[Id:5][Name:25][Description:50]

        //Length
        private const int ID_LENGTH = 5;
        private const int NAME_LENGTH = 25;
        private const int DESCRIPTION_LENGTH = 50;
        private const int TOTAL_LENGTH = 80;

        //Start indexes
        private const int ID_START = 0;
        private const int NAME_START = ID_START + ID_LENGTH;
        private const int DESCRIPTION_START = NAME_START + NAME_LENGTH;

        //Define a category line in "categories.txt"
        private Category ParseLineToCategory(string line)
        {
            int id = int.Parse(line.Substring(ID_START, ID_LENGTH).Trim());
            string name = line.Substring(NAME_START, NAME_LENGTH).Trim();
            string description = line.Substring(DESCRIPTION_START, DESCRIPTION_LENGTH).Trim();


            Category category = new Category
            {
                Id = id,
                Name = name,
                Description = description
            };

            return category;
        }

        private const string PATH = "Data\\categories.txt";
        public CategoryRepository()
        {
            //Setup path ("\Data\categories.txt")
            string directoryPath = Path.GetDirectoryName(PATH);

            //Create "Data" directory if not exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            //Create "categories.txt" file if not exists
            if (!File.Exists(PATH))
            {
                File.Create(PATH).Close();
            }
        }

        public void Add(Category category)
        {
            List<Category> categories = GetAll();

            int newId;

            if (categories.Count == 0)
            {
                newId = 1;
            }

            else
            {
                newId = categories.Max(x => x.Id) + 1;
            }

            category.Id = newId;

            //[Id:5][Name:25][Description:50]
            string line =
                newId.ToString().PadLeft(ID_LENGTH, '0') +
                category.Name.PadRight(NAME_LENGTH) +
                category.Description.PadRight(DESCRIPTION_LENGTH);

            if (line.Length != TOTAL_LENGTH)
            {
                throw new Exception("Invalid category line length.");
            }

            File.AppendAllText(PATH, line + Environment.NewLine);
        }

        public void Delete(int id)
        {
            List<Category> categories = GetAll();

            bool isRemoved = false;

            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i].Id == id)
                {
                    categories.RemoveAt(i);
                    isRemoved = true;
                    break;
                }
            }

            if (!isRemoved)
            {
                return; // Category id was not found
            }

            List<string> lines = new List<string>();

            foreach (Category c in categories)
            {
                string line =
                    c.Id.ToString().PadLeft(ID_LENGTH, '0') +
                    c.Name.PadRight(NAME_LENGTH) +
                    c.Description.PadRight(DESCRIPTION_LENGTH);
                    

                if (line.Length != TOTAL_LENGTH)
                {
                    throw new Exception("Invalid category line legth.");
                }

                lines.Add(line);
            }

            File.WriteAllLines(PATH, lines);
        }

        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();

            string[] lines = File.ReadAllLines(PATH);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.Length != TOTAL_LENGTH)
                    continue;

                Category category = ParseLineToCategory(line);
                categories.Add(category);                
            }

            return categories;
        }

        public Category GetById(int id)
        {
            List<Category> categories = GetAll();

            foreach (Category category in categories)
            {
                if (category.Id == id)
                {
                    return category;
                }
            }

            return null;
        }

        public List<Category> Search(string keyword)
        {
            List<Category> categories = GetAll();
            List<Category> result = new List<Category>();

            if (string.IsNullOrWhiteSpace(keyword)) 
            {
                return result;
            }

            keyword = keyword.ToLower();

            foreach (Category category in categories) 
            {
                if (category.Name.ToLower().Contains(keyword) ||
                    category.Description.ToLower().Contains(keyword))                 
                {
                    result.Add(category);
                }
            }

            return result;
        }

        public void Update(Category category)
        {

            List<Category> categories = GetAll();

            bool isUpdated = false;

            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i].Id == category.Id)
                {
                    categories[i] = category;
                    isUpdated = true;
                    break;
                }
            }

            if (!isUpdated)
            {
                return; //Category was not found
            }

            List<string> lines = new List<string>();

            foreach (Category b in categories)
            {
                string line =
                    b.Id.ToString().PadLeft(ID_LENGTH, '0') +
                    b.Name.PadRight(NAME_LENGTH) +
                    b.Description.PadRight(DESCRIPTION_LENGTH);

                if (line.Length != TOTAL_LENGTH)
                {
                    throw new Exception("Invalid category line legth.");
                }

                lines.Add(line);
            }

            File.WriteAllLines(PATH, lines);
        }
    }
}
