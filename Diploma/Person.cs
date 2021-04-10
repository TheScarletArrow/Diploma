using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Diploma
{
    [DisplayName("person")]
    public class Person
    {
        private string _id;
        private string _name;
        private string _surname;
        private string _password;
        private string _mail;
        private string _phone;
        private DateTime _birthdate;
        private string _leader1;
        private string _leader2;
        private string _stage;
        private string _knowledge;
     
        
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
            get => _leader1;
            set => _leader1 = value;
        }

        public string Leader2
        {
            get => _leader2;
            set => _leader2 = value;
        }

        public string Stage
        {
            get => _stage;
            set => _stage = value;
        }

        public string Knowledge
        {
            get => _knowledge;
            set => _knowledge = value;
        }
        
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

        private List<Person> _persons = new List<Person>();

        public List<Person> GetPersons()
        {
            return _persons;
        }

        public void ClearAllPersons()
        {
            _persons.Clear();
            
        }
    }
}
