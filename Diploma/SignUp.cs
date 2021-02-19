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
        public readonly SHA512 sha512 = SHA512.Create();


        public SignUp()
        {
            InitializeComponent();
            BirthDatePicker.Value = DateTime.Parse("2020-01-01");
        }

        private void clearAllTextBox()
        {
            NameField.Text = "";
            SurnameField.Text = "";
            MailField.Text = "";
            PhoneField.Text = "";
            PasswordField.Text = "";
            workingXPComboBox.SelectedItem = null;
            KnowledgeComboBox.SelectedItem = null;
            BirthDatePicker.Value = DateTime.Now;
        }
        private bool check_fields()
        {
            string name = NameField.Text;

            string surname = SurnameField.Text;
            string mail = MailField.Text;
            string phone = PhoneField.Text;
            DateTime birthdate = BirthDatePicker.Value;

            //имя
            if (String.IsNullOrEmpty(name)) { MessageBox.Show("Имя не введено"); return false; }
            else
            {
                bool name_is_good = name.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c));
                if (!name_is_good)
                {
                    MessageBox.Show("Имя сожержит неверные символы");
                    return false;
                }

            }

            //фамилия
            if (String.IsNullOrEmpty(surname)) { MessageBox.Show("Фамилия не введена"); return false; }
            else
            {
                bool surname_is_good = surname.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c));
                if (!surname_is_good) { MessageBox.Show("Фамилия сожержит неверные символы"); return false; }
            }
            //почта
            if (String.IsNullOrEmpty(mail)) { MessageBox.Show("Почта не введена"); return false; }
            else
            if (!mail.Contains("@")) { MessageBox.Show("Неправильно введена почта"); return false; }

            if (String.IsNullOrEmpty(phone)) { MessageBox.Show("Телефон не введен"); return false; }
            else
            {
                bool phone_is_good = phone.All(c => Char.IsDigit(c) || c.Equals('-') || c.Equals('+') || c.Equals('(')|| c.Equals(')'));
                if (!phone_is_good) { MessageBox.Show("телефон содержит неверные символы"); return false; }
            }
            if (String.IsNullOrEmpty(PasswordField.Text)) { MessageBox.Show("Пароль не задан"); return false; }

            if (birthdate > DateTime.Now)
            {
                MessageBox.Show("Некорреткная дата рождения");
                return false;
            }
            return true;
        }

        private static String getPassword()
        {
            String pass = "";
            var random = new Random();
            const int password_length = 10;

            while (pass.Length < password_length)
            {
                Char c = (char)random.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    pass += c;
            }

            return pass;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            String enc_pass = "";
            DataBase dataBase = new DataBase();
            DataTable dataTable = new DataTable();
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            DataTable dataTable3 = new DataTable();


            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlDataAdapter adapter2 = new MySqlDataAdapter();
            MySqlDataAdapter adapterToList = new MySqlDataAdapter();
            MySqlDataAdapter adapterToWork = new MySqlDataAdapter();
            dataBase.OpenConnection();

            string name = NameField.Text;
            int id = 0;
            string surname = SurnameField.Text;
            string mail = MailField.Text;
            string phone = PhoneField.Text;
            DateTime birthdate = BirthDatePicker.Value.Date;
            bool all_is_ok = check_fields();
            if (all_is_ok)
                try
                {
                        dataBase.OpenConnection();
                    MySqlCommand insertToUserInfo = new MySqlCommand
                    //    ("insert into user_info (id, name, surname, mail, phone, birth_date,login_id) values (NULL, @un, @us, @um, @up, @ub, @ul);", dataBase.GetConnection());

                    ("insert into user_info (id, name, surname, mail, phone, birth_date,login_id) values (NULL, @un, @us, @um, @up, @ub, @ul);", dataBase.GetConnection());
                    insertToUserInfo.Parameters.Add("@un", MySqlDbType.VarChar).Value = name;
                    insertToUserInfo.Parameters.Add("@us", MySqlDbType.VarChar).Value = surname;
                    insertToUserInfo.Parameters.Add("@um", MySqlDbType.VarChar).Value = mail;
                    insertToUserInfo.Parameters.Add("@up", MySqlDbType.VarChar).Value = phone;
                    insertToUserInfo.Parameters.Add("@ub", MySqlDbType.DateTime).Value = birthdate;
                    
                    
                    MySqlCommand countRows = new MySqlCommand("select * from user_info;", dataBase.GetConnection());
                    adapter2.SelectCommand = countRows;
                    adapter.SelectCommand = insertToUserInfo;

                    MySqlCommand findID = new MySqlCommand("select id from user_info order by id desc limit 1;", dataBase.GetConnection());
                    MySqlDataReader dataReader = null;
                    try { dataReader = findID.ExecuteReader(); }
                    catch (MySqlException) { id = 0; }
                    while (dataReader.Read())
                    {
                        id = int.Parse(dataReader.GetValue(0).ToString());
                    }
                    dataReader.Close();
                    MessageBox.Show("Ваш логин " + "user" + (id.ToString()) + "\nВаш пароль " + PasswordField.Text + "\n ЗАПИШИТЕ ИЛИ ЗАПОМНИТЕ ЕГО!");
                    adapter2.Fill(dataTable);
                    insertToUserInfo.Parameters.Add("@ul", MySqlDbType.VarChar).Value = "user" + (id.ToString()).ToString();

                    adapter.Fill(dataTable);


                    MySqlCommand insertToUserList = new MySqlCommand("insert into user_list (id, login, password) values (NULL, @ul, @up);", dataBase.GetConnection());

                    insertToUserList.Parameters.AddWithValue("@ul", "user" + (id.ToString()));
                    enc_pass = Convert.ToBase64String(sha512.ComputeHash(Encoding.UTF8.GetBytes(PasswordField.Text)));
                    insertToUserList.Parameters.AddWithValue("@up", enc_pass);
                    adapterToList.SelectCommand = insertToUserList;
                    dataBase.OpenConnection();
                    adapterToList.Fill(dataTable1);

                    MySqlCommand insertToWork = new MySqlCommand
                        ("insert into user_dep (id, user_id, worker_type, department_type) values (NULL, @ul, @uwt, @udt);", dataBase.GetConnection());
                    insertToWork.Parameters.AddWithValue("@ul", "user" + (id.ToString()));
                    insertToWork.Parameters.AddWithValue("@uwt", workingXPComboBox.SelectedItem);
                    insertToWork.Parameters.AddWithValue("@udt", KnowledgeComboBox.SelectedItem);
                    adapterToWork.SelectCommand = insertToWork;
                    adapterToWork.Fill(dataTable2);

                    MySqlCommand insertToXP = new MySqlCommand
                        ("insert into user_xp (login) values (@UL);", dataBase.GetConnection());
                    insertToXP.Parameters.AddWithValue("@UL", "user" + (id.ToString()));
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
                    dataAdapter.SelectCommand = insertToXP;
                    dataAdapter.Fill(dataTable3);

                    clearAllTextBox();
                }
                catch (
                    MySqlException mysqlexception
                )
                {

                    MessageBox.Show(mysqlexception.StackTrace);
                }
            else { }

        }

        private void PasswordField_DoubleClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(PasswordField.Text)) return;
            else
            {
                Clipboard.SetText(PasswordField.Text);
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
            this.Close();
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
            if (e.Button.Equals(MouseButtons.Left))
            {
                Left += dx;
                Top += dy;
            }
        }

        private void formDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        private void MimimzeClick(object sender, EventArgs e)
        {
            if (this.WindowState.Equals(FormWindowState.Normal))
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }
        private void MinimizeHover(object sender, EventArgs e)
        {
            MinimizeLabel.ForeColor = System.Drawing.Color.FromArgb(70, 63, 72, 204);
        }
        private void MinimizeLeave(object sender, EventArgs e)
        {
            MinimizeLabel.ForeColor = System.Drawing.Color.Black;

        }
    }
}
