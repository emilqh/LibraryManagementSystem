using LMS.DataAccessLayer.Entities;
using System.Globalization;

namespace LMS.DataAccessLayer
{
    public class MemberRepository : IRepository<Member>
    {
        //[Id:5][FullName:25][Email:25][PhoneNumber:10][MembershipDate:10][IsActive:1],[Total:76]

        //Lengths
        private const int ID_LENGTH = 5;
        private const int FULLNAME_LENGTH = 25;
        private const int EMAIL_LENGTH = 25;
        private const int PHONE_NUMBER_LENGTH = 10;
        private const int MEMBERSHIP_DATE_LENGTH = 10;
        private const int ISACTIVE_LENGTH = 1;
        private const int TOTAL_LINE_LENGTH = 76;

        //Start indexes
        private const int ID_START = 0;
        private const int FULLNAME_START = ID_START + ID_LENGTH;
        private const int EMAIL_START = FULLNAME_START + FULLNAME_LENGTH;
        private const int PHONE_NUMBER_START = EMAIL_START + EMAIL_LENGTH;
        private const int MEMBERSHIP_DATE_START = PHONE_NUMBER_START + PHONE_NUMBER_LENGTH;
        private const int ISACTIVE_START = MEMBERSHIP_DATE_START + MEMBERSHIP_DATE_LENGTH;

        //Define a member line in members.txt
        private const string DATE_FORMAT = "dd/MM/yyyy";

        private Member ParseLineToMember(string line)
        {
            int id = int.Parse(line.Substring(ID_START, ID_LENGTH).Trim());
            string fullName = line.Substring(FULLNAME_START, FULLNAME_LENGTH).Trim();
            string email = line.Substring(EMAIL_START, EMAIL_LENGTH).Trim();
            string phoneNumber = line.Substring(PHONE_NUMBER_START, PHONE_NUMBER_LENGTH).Trim();
            string dateString = line.Substring(MEMBERSHIP_DATE_START, MEMBERSHIP_DATE_LENGTH).Trim();
            DateTime membershipDate = DateTime.ParseExact(dateString, DATE_FORMAT, CultureInfo.InvariantCulture);

            char isActiveChar = line.Substring(ISACTIVE_START, ISACTIVE_LENGTH)[0];
            bool isActive = isActiveChar == '1';

            Member member = new Member
            {
                Id = id,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                MembershipDate = membershipDate,
                IsActive = isActive
            };

            return member;
        }

        private const string PATH = "Data\\members.txt";

        public MemberRepository()
        {
            //Setup path ("\Data\members.txt")
            string directoryPath = Path.GetDirectoryName(PATH);

            //Create "Data" directory if not exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            //Create "members.txt" file if not exists
            if (!File.Exists(PATH))
            {
                File.Create(PATH).Close();
            }
        }

        public void Add(Member member)
        {
            List<Member> members = GetAll();

            int newId;

            if (members.Count == 0)
            {
                newId = 1;
            }

            else
            {
                newId = members.Max(x => x.Id) + 1;
            }

            member.Id = newId;

            //[Id:5][FullName:25][Email:25][PhoneNumber:10][MembershipDate:10][IsActive:1],[Total:76]
                string line =
                newId.ToString().PadLeft(ID_LENGTH, '0') +
                member.FullName.PadRight(FULLNAME_LENGTH) +
                member.Email.PadRight(EMAIL_LENGTH) +
                member.PhoneNumber.PadRight(PHONE_NUMBER_LENGTH) +
                member.MembershipDate.ToString(DATE_FORMAT) +
                (member.IsActive ? "1" : "0");

            if (line.Length != TOTAL_LINE_LENGTH)
            {
                throw new Exception("Invalid member line legth.");
            }

            File.AppendAllText(PATH, line + Environment.NewLine);

        }

        public void Delete(int id)
        {
            List<Member> members = GetAll();

            bool isRemoved = false;

            for (int i = 0; i < members.Count; i++)
            {
                if (members[i].Id == id)
                {
                    members.RemoveAt(i);
                    isRemoved = true;
                    break;
                }
            }

            if (!isRemoved)
            {
                return; // Member id was not found
            }

            List<string> lines = new List<string>();

            foreach (Member m in members)
            {
                string line =
                    m.Id.ToString().PadLeft(ID_LENGTH, '0') +
                    m.FullName.PadRight(FULLNAME_LENGTH) +
                    m.Email.PadRight(EMAIL_LENGTH) +
                    m.PhoneNumber.PadRight(PHONE_NUMBER_LENGTH) +
                    m.MembershipDate.ToString(DATE_FORMAT) +
                    (m.IsActive ? "1" : "0");

                if (line.Length != TOTAL_LINE_LENGTH)
                {
                    throw new Exception("Invalid member line legth.");
                }

                lines.Add(line);
            }

            File.WriteAllLines(PATH, lines);
        }

        public List<Member> GetAll()
        {
            List<Member> members = new List<Member>();

            string[] lines = File.ReadAllLines(PATH);

            foreach (string line in lines)
            {
                //if line is empty skip
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.Length != TOTAL_LINE_LENGTH)
                    continue;

                Member member = ParseLineToMember(line);
                members.Add(member);
            }
            return members;
        }

        public Member GetById(int id)
        {
            List<Member> members = GetAll();
            foreach (Member member in members)
            {
                if (member.Id == id)
                {
                    return member;
                }
            }

            return null;
        }

        public List<Member> Search(string keyword)
        {
            List<Member> members = GetAll();
            List<Member> result = new List<Member>();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return result;
            }

            keyword = keyword.ToLower();

            foreach (Member member in members)
            {
                if (member.FullName.ToLower().Contains(keyword) ||
                    member.Email.ToLower().Contains(keyword) ||
                    member.PhoneNumber.ToLower().Contains(keyword))
                {
                    result.Add(member);
                }
            }

            return result;

        }

        public void Update(Member member)
        {

            List<Member> members = GetAll();

            bool isUpdated = false;

            for (int i = 0; i < members.Count; i++)
            {
                if (members[i].Id == member.Id)
                {
                    members[i] = member;
                    isUpdated = true;
                    break;
                }
            }

            if (!isUpdated)
            {
                return; //Member was not found
            }

            List<string> lines = new List<string>();

            foreach (Member m in members)
            {
                string line =
                    m.Id.ToString().PadLeft(ID_LENGTH, '0') +
                    m.FullName.PadRight(FULLNAME_LENGTH) +
                    m.Email.PadRight(EMAIL_LENGTH) +
                    m.PhoneNumber.PadRight(PHONE_NUMBER_LENGTH) +
                    m.MembershipDate.ToString(DATE_FORMAT) +
                    (m.IsActive ? "1" : "0");

                if (line.Length != TOTAL_LINE_LENGTH)
                {
                    throw new Exception("Invalid member line legth.");
                }

                lines.Add(line);
            }

            File.WriteAllLines(PATH, lines);
        }
    }
}
