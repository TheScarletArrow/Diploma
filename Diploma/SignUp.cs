using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;

namespace Diploma
{
    public partial class SignUp : Form
    {
       
        public SignUp()
        {
            InitializeComponent();
        }

        private void clearAllTextBox() {
            NameField.Text = "";
            SurnameField.Text = "";
            MailField.Text = "";
            PhoneField.Text = "";
            PasswordField.Text = "";
        }
        private bool check_fields() {
            string name = NameField.Text;

            string surname = SurnameField.Text;
            string mail = MailField.Text;
            string phone = PhoneField.Text;
            DateTime birthdate = BirthDatePicker.Value;
            //имя
            if (String.IsNullOrEmpty(name)) { MessageBox.Show("Имя не введено"); }
            else
            {
                bool name_is_good = name.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c));
                if (!name_is_good) MessageBox.Show("Имя сожержит неверные символы");
            }

            //фамилия
            if (String.IsNullOrEmpty(surname)) { MessageBox.Show("Фамилия не введена"); }
            else
            {
                bool surname_is_good = surname.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c));
                if (!surname_is_good) MessageBox.Show("Фамилия сожержит неверные символы");
            }
            //почта
            if (String.IsNullOrEmpty(mail)) { MessageBox.Show("Почта не введена"); }
            else
            if (!mail.Contains("@")) { MessageBox.Show("Неправильно введена почта"); }

            if (String.IsNullOrEmpty(phone)) { MessageBox.Show("Телефон не введен"); }
            else
            {
                bool phone_is_good = phone.All(c => Char.IsDigit(c) || c.Equals('-')||c.Equals('+'));
                if (!phone_is_good) MessageBox.Show("телефон содержит неверные символы");
            }
            if (String.IsNullOrEmpty(PasswordField.Text)) { MessageBox.Show("Пароль не задан"); }

          //  if (birthdate.Date.CompareTo(DateTime.Today) > 0)
           // {
            //    MessageBox.Show("Введенная дата позже сегодняшней");
           // }
            return true;
        }

        private static String getPassword() {
            String pass = "";
            var random = new Random();
            const int password_length = 10;

            while (pass.Length < password_length) {
                Char c = (char)random.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    pass+=c;
            }
            
            return pass;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            DataBase dataBase = new DataBase();
            DataTable dataTable = new DataTable();
            DataTable dataTable1 = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlDataAdapter adapter2 = new MySqlDataAdapter();
            MySqlDataAdapter adapterToList = new MySqlDataAdapter();
            dataBase.OpenConnection();

            string name = NameField.Text;
       
            string surname = SurnameField.Text;
            string mail = MailField.Text;
            string phone = PhoneField.Text;
            DateTime birthdate = BirthDatePicker.Value;
            bool all_is_ok = check_fields();
            if (all_is_ok)
                try
                {
                    MySqlCommand insertToUserInfo = new MySqlCommand
                        ("insert into user_info (id, name, surname, mail, phone, birth_date,login) values (NULL, @un, @us, @um, @up, @ub, @ul);", dataBase.GetConnection());
                    // ("select * from user_info;",  dataBase.GetConnection());
                    insertToUserInfo.Parameters.Add("@un", MySqlDbType.VarChar).Value = name;
                    insertToUserInfo.Parameters.Add("@us", MySqlDbType.VarChar).Value = surname;
                    insertToUserInfo.Parameters.Add("@um", MySqlDbType.VarChar).Value = mail;
                    insertToUserInfo.Parameters.Add("@up", MySqlDbType.VarChar).Value = phone;
                    insertToUserInfo.Parameters.Add("@ub", MySqlDbType.DateTime).Value = birthdate;
                    MySqlCommand countRows = new MySqlCommand("select * from user_info;", dataBase.GetConnection());
                    
                    adapter2.SelectCommand = countRows;
                    
                    adapter.SelectCommand = insertToUserInfo;

                    adapter2.Fill(dataTable);
                    insertToUserInfo.Parameters.Add("@ul", MySqlDbType.VarChar).Value = "user" + (dataTable.Rows.Count+1).ToString();

                    adapter.Fill(dataTable);
                    MessageBox.Show("Ваш логин " + "user"+ (dataTable.Rows.Count+1) +"\nВаш пароль "+PasswordField.Text + "\n ЗАПИШИТЕ ИЛИ ЗАПОМНИТЕ ЕГО!");

                    MySqlCommand insertToUserList = new MySqlCommand("insert into user_list (id, login, password) values (NULL, @ul, @up)", dataBase.GetConnection());

                    SHA512 sha512 = SHA512.Create();

                    insertToUserList.Parameters.AddWithValue("@ul", "user" + (dataTable.Rows.Count + 1));
                    insertToUserList.Parameters.AddWithValue("@up", Convert.ToBase64String
                     (sha512.ComputeHash(Encoding.UTF8.GetBytes(getPassword()))));
                    adapterToList.SelectCommand = insertToUserList;
                    adapterToList.Fill(dataTable1);


                    clearAllTextBox();
                }
                catch (
                    MySqlException mysqlexception
                )
                {

                    MessageBox.Show(mysqlexception.StackTrace);
                }
                

        }

        private void PasswordField_DoubleClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(PasswordField.Text)) return;
            else
            { Clipboard.SetText(PasswordField.Text);
                PasswordLabel.Text = "Скопировано в буфер обмена!";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            PasswordField.Text = getPassword();

        }

        private void CloseHover(object sender, EventArgs e)
        {
            CloseLabel.ForeColor = System.Drawing.Color.Red;
        }

        private void CloseLeave(object sender, EventArgs e)
        {
            CloseLabel.ForeColor = System.Drawing.Color.Black;

        }

        private void CloseClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            new Auth().Show();
            this.Hide();
        }
        Point lastPoint;
        private void formMove(object sender, MouseEventArgs e)
        {
            int dx = e.X - lastPoint.X;
            int dy = e.Y - lastPoint.Y;
            if (e.Button.Equals(MouseButtons.Left)) {
                this.Left += dx;
                this.Top += dy;
                }
        }

        private void formDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
    }
}
