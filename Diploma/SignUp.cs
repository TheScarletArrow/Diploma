using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Diploma
{
    public partial class SignUp : Form
    {
        static  string expression = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
        Regex regex = new Regex(expression);
        readonly Regex testregex = new Regex(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");
        public SignUp()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {

            DataBase dataBase = new DataBase();
            DataTable dataTable = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            if (Regex.IsMatch(PhoneField.Text, expression, RegexOptions.IgnoreCase)) {
                MessageBox.Show("WEEE");
            }
            else {
                    
                  }

            string name = NameField.Text;
            string surname = SurnameField.Text;
            string mail = MailField.Text;
            string phone = PhoneField.Text;
            DateTime birthdate = BirthDatePicker.Value;
            MySqlCommand insertToUserInfo = new MySqlCommand("insert into `user_info` (id, name, surname, mail, phone, birth_date) values (NULL, @un, @us, @um, @up, @ub);", 
                dataBase.GetConnection());
            insertToUserInfo.Parameters.Add("@un", MySqlDbType.VarChar).Value = name;
            insertToUserInfo.Parameters.Add("@us", MySqlDbType.VarChar).Value = surname;
            insertToUserInfo.Parameters.Add("@um", MySqlDbType.VarChar).Value = mail;
            insertToUserInfo.Parameters.Add("@up", MySqlDbType.VarChar).Value = phone;
            insertToUserInfo.Parameters.Add("@ub", MySqlDbType.DateTime).Value = birthdate;

            adapter.SelectCommand=insertToUserInfo;
            adapter.Fill(dataTable);



        }


    }
}
