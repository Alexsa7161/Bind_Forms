using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bind_Forms
{

    public enum Gender { male, female };
    [Serializable]
    public class Writer
    {
        public string name { get; set; }
        public string surName { get; set; }
        public string author_photo { get; set; }
        public DateTime birthdate { get; set; }
        public DateTime? deathdate { get; set; }
        public int num_books { get; set; }
        public Gender gender { get; set; }
        public string StrEnum
        {
            get => gender.ToString();
            set => gender.ToString();
        }
        public Writer(string Name, string SurName, string author_photo, DateTime birthdate, DateTime? deathdate, int num_books, Gender gender)
        {
            this.name = Name;
            this.surName = SurName;
            this.author_photo = author_photo;
            this.birthdate = birthdate;
            this.deathdate = deathdate;
            this.num_books = num_books;
            this.gender = gender;
        }
        public Writer()
        {
            this.name = "Алексей";
            this.surName = "Саньков";
            this.author_photo = "C:\\Users\\Alexsa7161\\source\\repos\\Bind Forms\\Images\\1638840756_2.jpg";
            this.birthdate = new DateTime(2002, 5, 15);
            this.deathdate = null;
            this.num_books = 5;
            this.gender = gender;
        }
    }
}