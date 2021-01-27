using System;
using System.Text.RegularExpressions;

namespace Diploma
{
    class Person
    {
        private int id;
        private string name;
        private string surname;
        private string mail;
        private string phone;
        private DateTime birthdate;
        //Regex regex = new Regex( @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");

        public Person(int ID, string name, string surname, string mail, string phone, DateTime dateTime) {
            Id = ID;
            Name = name;
            Surname = surname;
            Mail = mail;
            Phone = phone;
            Birthdate = dateTime;
        }
        public int Id { 
            get { return id; } 
            set { id = value; } 
        }
        public string Name { 
            get { return name; } 
            set { name = value; } 
        }

        public string Surname { 
            get { return surname; } 
            set { surname = value; } 
        }

        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        
        public string Phone
        {
                        
            get { return phone; }
            set { phone = value; }
        }

        public DateTime Birthdate
        {
            get { return birthdate; }
            set { birthdate = value; }
        }

       
    }
}
