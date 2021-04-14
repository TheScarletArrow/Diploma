using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using System.IO;
using Diploma.Entities;

namespace Diploma
{
    public partial class SignUp : Form
    {
        public readonly SHA512 Sha512 = SHA512.Create();


        public SignUp()
        {
            InitializeComponent();
            BirthDatePicker.Value = DateTime.Parse("2020-01-01");
        }

        private void ClearAllTextBox()
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
            
            var name = NameField.Text;

            var surname = SurnameField.Text;
            var mail = MailField.Text;
            var phone = PhoneField.Text;
            var birthdate = BirthDatePicker.Value;

            //имя
            if (string.IsNullOrEmpty(name)) { MessageBox.Show(@"Имя не введено"); return false; }
            else
            {
                var nameIsGood = name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
                
                if (!nameIsGood)
                {
                    MessageBox.Show(@"Имя сожержит неверные символы");
                    return false;
                }

            }

            //фамилия
            if (string.IsNullOrEmpty(surname)) { MessageBox.Show(@"Фамилия не введена"); return false; }
            else
            {
                var surnameIsGood = surname.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
                if (!surnameIsGood) { MessageBox.Show(@"Фамилия сожержит неверные символы"); return false; }
            }
            //почта
            if (string.IsNullOrEmpty(mail)) { MessageBox.Show(@"Почта не введена"); return false; }
           
            if (!mail.Contains("@")) { MessageBox.Show(@"Неправильно введена почта"); return false; }

            if (string.IsNullOrEmpty(phone)) { MessageBox.Show(@"Телефон не введен"); return false; }
            
            {
                var phoneIsGood = phone.All(c => char.IsDigit(c) || c.Equals('-') || c.Equals('+') || c.Equals('(')|| c.Equals(')'));
                if (!phoneIsGood) { MessageBox.Show(@"Телефон содержит неверные символы"); return false; }
            }
            if (string.IsNullOrEmpty(PasswordField.Text)) { MessageBox.Show(@"Пароль не задан"); return false; }

            if (birthdate > DateTime.Now)
            {
                MessageBox.Show(@"Некорреткная дата рождения");
                return false;
            }
            return true;
        }

        private static string GetPassword(int length)
        {
            
            var pass = "";
            var random = new Random();
            var passwordLength = length;

            while (pass.Length < passwordLength)
            {
                var c = (char)random.Next(33, 125);
                if (char.IsLetterOrDigit(c))
                    pass += c;
            }

            return pass;
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var encPass = "";
                var dataBase = new DataBase();
                var dataTable = new DataTable();



                var adapter = new MySqlDataAdapter();
                var adapter2 = new MySqlDataAdapter();

               

                var name = NameField.Text;
                var id = 0;
                var surname = SurnameField.Text;
                var mail = MailField.Text;
                var phone = PhoneField.Text;
                var birthdate = BirthDatePicker.Value.Date;
                var allIsOk = check_fields();
                if (allIsOk)
                   //for (int i=0;i<100;i++)
                    try
                    {
                        dataBase.OpenConnection();
                        var insertToUserInfo = new MySqlCommand
                        ("insert into user_info (id, name, surname, mail, phone, birth_date,login_id) values (NULL, @un, @us, @um, @up, @ub, @ul);",
                            dataBase.GetConnection());
                        insertToUserInfo.Parameters.Add("@un", MySqlDbType.VarChar).Value = name;
                        insertToUserInfo.Parameters.Add("@us", MySqlDbType.VarChar).Value = surname;
                        insertToUserInfo.Parameters.Add("@um", MySqlDbType.VarChar).Value = mail;
                        insertToUserInfo.Parameters.Add("@up", MySqlDbType.VarChar).Value = phone;
                        insertToUserInfo.Parameters.Add("@ub", MySqlDbType.DateTime).Value = birthdate;

                        var countRows = new MySqlCommand("select * from user_info;", dataBase.GetConnection());

                        adapter2.SelectCommand = countRows;
                        adapter.SelectCommand = insertToUserInfo;

                        var findId = new MySqlCommand("select id from user_info order by id desc limit 1;",
                            dataBase.GetConnection());
                        MySqlDataReader dataReader = null;
                        try
                        {
                            dataReader = findId.ExecuteReader();
                        }
                        catch (NullReferenceException)
                        {
                            id = 0;
                        }

                        while (dataReader != null && dataReader.Read())
                        {
                            id = int.Parse(dataReader.GetValue(0).ToString());
                        }

                        dataReader?.Close();
                        MessageBox.Show(@"Ваш логин user" + id + "\nВаш пароль " + PasswordField.Text +
                                        "\n ЗАПИШИТЕ ИЛИ ЗАПОМНИТЕ ЕГО!");
                        adapter2.Fill(dataTable);
                        insertToUserInfo.Parameters.Add("@ul", MySqlDbType.VarChar).Value =
                            "user" + (id.ToString()).ToString();

                        //adapter.Fill(dataTable);


                        var insertToUserList = new MySqlCommand(
                            "insert into user_list (id, login, password) values (NULL, @ul, @up);",
                            dataBase.GetConnection());

                        insertToUserList.Parameters.AddWithValue("@ul", "user" + (id));
                        encPass = Convert.ToBase64String(
                            Sha512.ComputeHash(Encoding.UTF8.GetBytes(PasswordField.Text)));
                        insertToUserList.Parameters.AddWithValue("@up", encPass);
                        //adapterToList.SelectCommand = insertToUserList;
                        insertToUserInfo.ExecuteNonQuery();

                        insertToUserList.ExecuteNonQuery();

                   //     dataBase.OpenConnection();
                        //adapterToList.Fill(dataTable1);

                        var insertToWork = new MySqlCommand
                        ("insert into user_dep (id, user_id, worker_type, department_type) values (NULL, @ul, @uwt, @udt);",
                            dataBase.GetConnection());
                        insertToWork.Parameters.AddWithValue("@ul", "user" + (id.ToString()));
                        insertToWork.Parameters.AddWithValue("@uwt", workingXPComboBox.SelectedItem);
                        insertToWork.Parameters.AddWithValue("@udt", KnowledgeComboBox.SelectedItem);
                        insertToWork.ExecuteNonQuery();

                        //adapterToWork.SelectCommand = insertToWork;
                        //adapterToWork.Fill(dataTable2);

                        var insertToXp = new MySqlCommand
                            ("insert into user_xp (login) values (@UL);", dataBase.GetConnection());
                        insertToXp.Parameters.AddWithValue("@UL", "user" + (id));
                       // var dataAdapter = new MySqlDataAdapter();
                        insertToXp.ExecuteNonQuery();

                        // dataAdapter.SelectCommand = insertToXP;
                        // dataAdapter.Fill(dataTable3);

                        ClearAllTextBox();
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
                using (var fstream =
                    new FileStream(
                        $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log",
                        FileMode.Append))
                {

                    var array = Encoding.Default.GetBytes(exception.StackTrace);
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
            PasswordField.Text = GetPassword(Decimal.ToInt32( numericUpDown1.Value));

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
        private void FormMove(object sender, MouseEventArgs e)
        {
            var dx = e.X - _lastPoint.X;
            var dy = e.Y - _lastPoint.Y;
            if (e.Button.Equals(MouseButtons.Left))
            {
                Left += dx;
                Top += dy;
            }
        }

        private void FormDown(object sender, MouseEventArgs e)
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
