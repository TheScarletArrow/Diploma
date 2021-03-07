using System;
using System.Text.RegularExpressions;

namespace Diploma
{
    class Person
    {
        private int _id;
        private string _name;
        private string _surname;
        private string _mail;
        private string _phone;
        private DateTime _birthdate;
        //Regex regex = new Regex( @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");

        public Person(int id, string name, string surname, string mail, string phone, DateTime dateTime) {
            Id = id;
            Name = name;
            Surname = surname;
            Mail = mail;
            Phone = phone;
            Birthdate = dateTime;
        }
        public int Id { 
            get { return _id; } 
            set { _id = value; } 
        }
        public string Name { 
            get { return _name; } 
            set { _name = value; } 
        }

        public string Surname { 
            get { return _surname; } 
            set { _surname = value; } 
        }

        public string Mail
        {
            get { return _mail; }
            set { _mail = value; }
        }

        
        public string Phone
        {
                        
            get { return _phone; }
            set { _phone = value; }
        }

        public DateTime Birthdate
        {
            get { return _birthdate; }
            set { _birthdate = value; }
        }

       
    }
}
