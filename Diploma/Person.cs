using System;
using System.Text.RegularExpressions;

namespace Diploma
{
    public class Person
    {
        private string _id;
        private string _name;
        private string _surname;
        private string _password;
        private string _mail;
        private string _phone;
        private DateTime _birthdate;
        private string leader1;
        private string leader2;
        private string stage;
        private string knowledge;
        public Person(string ID, string name, string surname, string mail, string phone, DateTime dateTime, string leader1, string leader2, string stage, string knowledge) {
            Id = ID;
            Name = name;
            Surname = surname;
            Mail = mail;
            Phone = phone;
            Birthdate = dateTime;
            Leader1 = leader1;
            Leader2 = leader2;
            Stage = stage;
            Knowledge = knowledge;

        }
        public Person(string ID, string name, string surname, string mail, string phone, DateTime dateTime, string leader1,  string stage, string knowledge) {
            Id = ID;
            Name = name;
            Surname = surname;
            Mail = mail;
            Phone = phone;
            Birthdate = dateTime;
            Leader1 = leader1;
            Leader2 = " ";
            Stage = stage;
            Knowledge = knowledge;

        }

        public override string ToString()
        {
            return  Id;
        }

        public Person()
        {
        }

        public Person(string id)
        {
            _id = id;
        }

        public string Id { 
            get => _id;
            set => _id = value;
        }

        public string Password
        {
            get => _password;
            set => _password = value;
        }

        public string Name { 
            get => _name;
            set => _name = value;
        }

        public string Surname { 
            get => _surname;
            set => _surname = value;
        }

        public string Mail
        {
            get => _mail;
            set => _mail = value;
        }

        
        public string Phone
        {
                        
            get => _phone;
            set => _phone = value;
        }

        public DateTime Birthdate
        {
            get => _birthdate;
            set => _birthdate = value;
        }

        public string Leader1
        {
            get => leader1;
            set => leader1 = value;
        }

        public string Leader2
        {
            get => leader1;
            set => leader1 = value;
        }

        public string Stage
        {
            get => stage;
            set => stage = value;
        }

        public string Knowledge
        {
            get => knowledge;
            set => knowledge = value;
        }
    }
}
