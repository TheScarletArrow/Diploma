using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using System.IO;

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
            if (string.IsNullOrEmpty(name)) { MessageBox.Show(@"Имя не введено"); return false; }
            else
            {
                var name_is_good = name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
                
                if (!name_is_good)
                {
                    MessageBox.Show(@"Имя сожержит неверные символы");
                    return false;
                }

            }

            //фамилия
            if (string.IsNullOrEmpty(surname)) { MessageBox.Show(@"Фамилия не введена"); return false; }
            else
            {
                var surname_is_good = surname.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
                if (!surname_is_good) { MessageBox.Show(@"Фамилия сожержит неверные символы"); return false; }
            }
            //почта
            if (string.IsNullOrEmpty(mail)) { MessageBox.Show(@"Почта не введена"); return false; }
           
            if (!mail.Contains("@")) { MessageBox.Show(@"Неправильно введена почта"); return false; }

            if (String.IsNullOrEmpty(phone)) { MessageBox.Show(@"Телефон не введен"); return false; }
            
            {
                var phone_is_good = phone.All(c => char.IsDigit(c) || c.Equals('-') || c.Equals('+') || c.Equals('(')|| c.Equals(')'));
                if (!phone_is_good) { MessageBox.Show(@"Телефон содержит неверные символы"); return false; }
            }
            if (string.IsNullOrEmpty(PasswordField.Text)) { MessageBox.Show(@"Пароль не задан"); return false; }

            if (birthdate > DateTime.Now)
            {
                MessageBox.Show(@"Некорреткная дата рождения");
                return false;
            }
            return true;
        }

        private static string getPassword(int length)
        {
            
            var pass = "";
            var random = new Random();
            var password_length = length;

            while (pass.Length < password_length)
            {
                Char c = (char)random.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    pass += c;
            }

            return pass;
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String enc_pass = "";
                DataBase dataBase = new DataBase();
                DataTable dataTable = new DataTable();



                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlDataAdapter adapter2 = new MySqlDataAdapter();

                dataBase.OpenConnection();

                string name = NameField.Text;
                int id = 0;
                string surname = SurnameField.Text;
                string mail = MailField.Text;
                string phone = PhoneField.Text;
                DateTime birthdate = BirthDatePicker.Value.Date;
                bool all_is_ok = check_fields();
                if (all_is_ok)
                    //for(int i=0;i<100;i++)
                    try
                    {
                        dataBase.OpenConnection();
                        MySqlCommand insertToUserInfo = new MySqlCommand
                        ("insert into user_info (id, name, surname, mail, phone, birth_date,login_id) values (NULL, @un, @us, @um, @up, @ub, @ul);",
                            dataBase.GetConnection());
                        insertToUserInfo.Parameters.Add("@un", MySqlDbType.VarChar).Value = name;
                        insertToUserInfo.Parameters.Add("@us", MySqlDbType.VarChar).Value = surname;
                        insertToUserInfo.Parameters.Add("@um", MySqlDbType.VarChar).Value = mail;
                        insertToUserInfo.Parameters.Add("@up", MySqlDbType.VarChar).Value = phone;
                        insertToUserInfo.Parameters.Add("@ub", MySqlDbType.DateTime).Value = birthdate;

                        MySqlCommand countRows = new MySqlCommand("select * from user_info;", dataBase.GetConnection());

                        adapter2.SelectCommand = countRows;
                        adapter.SelectCommand = insertToUserInfo;

                        MySqlCommand findID = new MySqlCommand("select id from user_info order by id desc limit 1;",
                            dataBase.GetConnection());
                        MySqlDataReader dataReader = null;
                        try
                        {
                            dataReader = findID.ExecuteReader();
                        }
                        catch (NullReferenceException)
                        {
                            id = 0;
                        }

                        while (dataReader.Read())
                        {
                            id = int.Parse(dataReader.GetValue(0).ToString());
                        }

                        dataReader.Close();
                        MessageBox.Show(@"Ваш логин user" + (id.ToString()) + "\nВаш пароль " + PasswordField.Text +
                                        "\n ЗАПИШИТЕ ИЛИ ЗАПОМНИТЕ ЕГО!");
                        adapter2.Fill(dataTable);
                        insertToUserInfo.Parameters.Add("@ul", MySqlDbType.VarChar).Value =
                            "user" + (id.ToString()).ToString();

                        adapter.Fill(dataTable);


                        MySqlCommand insertToUserList = new MySqlCommand(
                            "insert into user_list (id, login, password) values (NULL, @ul, @up);",
                            dataBase.GetConnection());

                        insertToUserList.Parameters.AddWithValue("@ul", "user" + (id.ToString()));
                        enc_pass = Convert.ToBase64String(
                            sha512.ComputeHash(Encoding.UTF8.GetBytes(PasswordField.Text)));
                        insertToUserList.Parameters.AddWithValue("@up", enc_pass);
                        //adapterToList.SelectCommand = insertToUserList;
                        insertToUserInfo.ExecuteNonQuery();

                        insertToUserList.ExecuteNonQuery();

                        dataBase.OpenConnection();
                        //adapterToList.Fill(dataTable1);

                        MySqlCommand insertToWork = new MySqlCommand
                        ("insert into user_dep (id, user_id, worker_type, department_type) values (NULL, @ul, @uwt, @udt);",
                            dataBase.GetConnection());
                        insertToWork.Parameters.AddWithValue("@ul", "user" + (id.ToString()));
                        insertToWork.Parameters.AddWithValue("@uwt", workingXPComboBox.SelectedItem);
                        insertToWork.Parameters.AddWithValue("@udt", KnowledgeComboBox.SelectedItem);
                        insertToWork.ExecuteNonQuery();

                        //adapterToWork.SelectCommand = insertToWork;
                        //adapterToWork.Fill(dataTable2);

                        MySqlCommand insertToXP = new MySqlCommand
                            ("insert into user_xp (login) values (@UL);", dataBase.GetConnection());
                        insertToXP.Parameters.AddWithValue("@UL", "user" + (id.ToString()));
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
                        insertToXP.ExecuteNonQuery();

                        // dataAdapter.SelectCommand = insertToXP;
                        // dataAdapter.Fill(dataTable3);

                        clearAllTextBox();
                    }
                    catch (
                        MySqlException mysqlexception
                    )
                    {

                        MessageBox.Show(mysqlexception.StackTrace);
                    }

                dataBase.CloseConnection();
            }
            catch (Exception exception)
            {
                using (FileStream fstream =
                    new FileStream(
                        $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log",
                        FileMode.Append))
                {

                    byte[] array = Encoding.Default.GetBytes(exception.StackTrace);
                    // асинхронная запись массива байтов в файл
                    await fstream.WriteAsync(array, 0, array.Length);
                    await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                }
            }


        }

        private void PasswordField_DoubleClick(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(PasswordField.Text)) 
            
            {
                Clipboard.SetText(PasswordField.Text);
                PasswordLabel.Text = @"Скопировано в буфер обмена!";
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PasswordField.Text = getPassword(Decimal.ToInt32( numericUpDown1.Value));

        }

        private void CloseHover(object sender, EventArgs e)
        {
            CloseLabel.ForeColor = Color.Red;
        }

        private void CloseLeave(object sender, EventArgs e)
        {
            CloseLabel.ForeColor = Color.Black;

        }

        private void CloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            new Auth().Show();
            Hide();
        }
        Point _lastPoint;
        private void formMove(object sender, MouseEventArgs e)
        {
            int dx = e.X - _lastPoint.X;
            int dy = e.Y - _lastPoint.Y;
            if (e.Button.Equals(MouseButtons.Left))
            {
                Left += dx;
                Top += dy;
            }
        }

        private void formDown(object sender, MouseEventArgs e)
        {
            _lastPoint = new Point(e.X, e.Y);
        }
        private void MimimzeClick(object sender, EventArgs e)
        {
            if (WindowState.Equals(FormWindowState.Normal))
            {
                WindowState = FormWindowState.Minimized;
            }
        }
        private void MinimizeHover(object sender, EventArgs e)
        {
            MinimizeLabel.ForeColor = Color.FromArgb(70, 63, 72, 204);
        }
        private void MinimizeLeave(object sender, EventArgs e)
        {
            MinimizeLabel.ForeColor = Color.Black;

        }
    }
}
