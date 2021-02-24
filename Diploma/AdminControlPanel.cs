using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IO;


namespace Diploma
{
    /// <summary>
    /// Панель администратора
    /// </summary>
    public partial class AdminControlPanel : Form
    {
        
        SignUp signUp = new SignUp();
        public AdminControlPanel()
        {
            InitializeComponent();
            label8.Visible = false;
           
        }
        

        private void EmptyFields()
        {
            NameField.Text = "";
            SurnameField.Text = "";
            PasswordField.Text = "";
            MailField.Text = "";
            PhoneField.Text = "";
            DateField.Text = "";
            KnowledgeComboBox.SelectedItem = null;
            workingXPComboBox.SelectedItem = null;
            HeadOfficer.Text = null;
            scienceLeader.SelectedItem = null;
            scienceLeader.SelectedItem = null;
        }
        private async void textBox7_TextChanged(object sender, System.EventArgs e)
        {
            try
            {

                DataBase dataBase = new DataBase();
                dataBase.OpenConnection();
                EmptyFields();
                MySqlCommand cmd = new MySqlCommand(
                    @"select ui.login_id, ui.name, ui.surname, ul.password,
                ui.mail, ui.phone, ui.birth_date from user_list ul left join
                user_info ui on ((ul.login=ui.login_id)) where ui.login_id=@id;"
                    , dataBase.GetConnection());
                cmd.Parameters.AddWithValue("@id", searchField.Text);

                MySqlDataReader dataReader = cmd.ExecuteReader();


                String password_cached = PasswordField.Text;

                while (dataReader.Read())
                {
                    NameField.Text = dataReader.GetValue(1).ToString();
                    SurnameField.Text = dataReader.GetValue(2).ToString();
                    password_cached = dataReader.GetValue(3).ToString();
                    PasswordField.Text = password_cached;
                    MailField.Text = dataReader.GetValue(4).ToString();
                    PhoneField.Text = dataReader.GetValue(5).ToString();
                    DateField.Text = dataReader.GetValue(6).ToString();
                }

                dataReader.Close();

                MySqlCommand cmd2 = new MySqlCommand(
                    "select ud.user_id, ud.worker_type, ud.department_type, eho.HeadOfficer from user_dep ud left join engineer_head_officer eho on ud.department_type=Department where  user_id=@ui;"
                    , dataBase.GetConnection());
                cmd2.Parameters.AddWithValue("@ui", searchField.Text);
                MySqlDataReader dataReader1 = cmd2.ExecuteReader();

                while (dataReader1.Read())
                {
                    workingXPComboBox.SelectedItem = dataReader1.GetValue(1).ToString();
                    KnowledgeComboBox.SelectedItem = dataReader1.GetValue(2).ToString();
                    HeadOfficer.Text = dataReader1.GetValue(3).ToString();
                }

                dataReader1.Close();

                /* MySqlCommand officer = new MySqlCommand("select eho.HeadOfficer from user_dep ud left join engineer_head_officer eho on ud.department_type=Department where user_id=@id;", dataBase.GetConnection());
                 officer.Parameters.AddWithValue("@id", searchField.Text);
                 MySqlDataReader dataReader2 = officer.ExecuteReader();
                 while (dataReader2.Read())
                 {
                     HeadOfficer.Text = dataReader2.GetValue(0).ToString();
                 }
                 dataReader2.Close();*/

                MySqlCommand SelectScienceLeader = new MySqlCommand
                ("select ux.leader_name from user_xp ux left join worker_table_officer wo on wo.worker_type_officer=ux.leader_name where login=@ul;"
                    , dataBase.GetConnection());
                SelectScienceLeader.Parameters.AddWithValue("@ul", searchField.Text);
                MySqlDataReader sceinceLeaderReader = SelectScienceLeader.ExecuteReader();
                while (sceinceLeaderReader.Read())
                {
                    scienceLeader.SelectedItem = sceinceLeaderReader.GetValue(0).ToString();
                }

                sceinceLeaderReader.Close();


                dataBase.CloseConnection();
            }
            catch (InvalidOperationException exception)
            {
                using (FileStream fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                {

                    byte[] array = Encoding.Default.GetBytes(exception.StackTrace);
                    // асинхронная запись массива байтов в файл
                    await fstream.WriteAsync(array,  0, array.Length);
                    await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                }

            }


        }

        //удалить
        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DataBase dataBase = new DataBase();
                dataBase.OpenConnection();

                MySqlCommand dropFromUserInfo = new MySqlCommand("delete from user_info where login_id=@login;",
                    dataBase.GetConnection());
                MySqlCommand dropFromUserList =
                    new MySqlCommand("delete from user_list where login=@login;", dataBase.GetConnection());
                MySqlCommand dropFromUserDep =
                    new MySqlCommand("delete from user_dep where user_id=@login", dataBase.GetConnection());
                MySqlCommand dropFromUserXP =
                    new MySqlCommand("delete from user_xp where login=@login", dataBase.GetConnection());

                dropFromUserInfo.Parameters.AddWithValue("@login", searchField.Text);
                dropFromUserList.Parameters.AddWithValue("@login", searchField.Text);
                dropFromUserDep.Parameters.AddWithValue("@login", searchField.Text);
                dropFromUserXP.Parameters.AddWithValue("@login", searchField.Text);

                dropFromUserList.ExecuteNonQuery();
                dropFromUserDep.ExecuteNonQuery();
                dropFromUserInfo.ExecuteNonQuery();
                dropFromUserXP.ExecuteNonQuery();

                dataBase.CloseConnection();

                EmptyFields();
                searchField.Text = "";

                MessageBox.Show("Пользователь удален");
            }
            catch (InvalidOperationException exception)
            {
                using (FileStream fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                {

                    byte[] array = Encoding.Default.GetBytes(exception.StackTrace);
                    // асинхронная запись массива байтов в файл
                    await fstream.WriteAsync(array,  0, array.Length);
                    await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                }
            }

        }

        //изменить
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                String password = PasswordField.Text;
                String password1 = "";
                DataBase database = new DataBase();
                database.OpenConnection();
                MySqlCommand getPasswordFromDB = new MySqlCommand("select password from user_list where login=@ul",
                    database.GetConnection());
                getPasswordFromDB.Parameters.AddWithValue("@ul", searchField.Text);
                MySqlDataReader readPassword = getPasswordFromDB.ExecuteReader();
                while (readPassword.Read())
                {
                    password1 = readPassword.GetValue(0).ToString();
                }

                readPassword.Close();


                MySqlCommand change = new MySqlCommand
                ("update user_info set name=@name,surname=@surname,mail=@mail,phone=@phone,birth_date=@birthdate where login_id=@login;"
                    , database.GetConnection());

                change.Parameters.AddWithValue("@name", NameField.Text);
                change.Parameters.AddWithValue("@surname", SurnameField.Text);
                change.Parameters.AddWithValue("@mail", MailField.Text);
                change.Parameters.AddWithValue("@phone", PhoneField.Text);
                change.Parameters.AddWithValue("@birthdate", DateTime.Parse(DateField.Text));
                change.Parameters.AddWithValue("@login", searchField.Text);
                change.ExecuteNonQuery();


                MySqlCommand change2 = new MySqlCommand
                ("update user_dep set worker_type=@wt, department_type=@dt where user_id=@login;",
                    database.GetConnection());
                change2.Parameters.AddWithValue("@login", searchField.Text);
                change2.Parameters.AddWithValue("@wt", workingXPComboBox.SelectedItem);
                change2.Parameters.AddWithValue("@dt", KnowledgeComboBox.SelectedItem);
                change2.ExecuteNonQuery();

                MySqlCommand changeUserXP = new MySqlCommand
                ("update user_xp set leader_name=@leader where login=@login;"
                    , database.GetConnection());
                changeUserXP.Parameters.AddWithValue("@login", searchField.Text);
                changeUserXP.Parameters.AddWithValue("@leader", scienceLeader.SelectedItem);
                changeUserXP.ExecuteNonQuery();

                if (!password.Equals(password1))
                {
                    MySqlCommand changePassword = new MySqlCommand
                    ("update user_list set password=@pass where login=@login;"
                        , database.GetConnection());
                    changePassword.Parameters.AddWithValue("@pass",
                        Convert.ToBase64String(signUp.sha512.ComputeHash(Encoding.UTF8.GetBytes(PasswordField.Text))));
                    changePassword.Parameters.AddWithValue("@login", searchField.Text);
                    changePassword.ExecuteNonQuery();
                }
                else
                {
                }


                MySqlCommand disable = new MySqlCommand("SET SQL_SAFE_UPDATES = 0;", database.GetConnection());
                disable.ExecuteNonQuery();


                MySqlCommand change3 = new MySqlCommand(
                    @"update user_xp set leader_name=@ln where login=@login;",
                    database.GetConnection());
                change3.Parameters.AddWithValue("@login", searchField.Text);
                change3.Parameters.AddWithValue("@ln", scienceLeader.SelectedItem);



                change3.ExecuteNonQuery();
                EmptyFields();
                searchField.Text = "";
                database.CloseConnection();
                MessageBox.Show(@"Изменено успешно");
            }
            catch (InvalidOperationException exception)
            {
                using (FileStream fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                {

                    byte[] array = Encoding.Default.GetBytes(exception.StackTrace);
                    // асинхронная запись массива байтов в файл
                    await fstream.WriteAsync(array,  0, array.Length);
                    await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);
                }
            }


        }

        private void CloseClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void CloseHover(object sender, EventArgs e)
        {
            CloseLabel.BackColor = Color.Red;
        }

        private void CloseLeave(object sender, EventArgs e)
        {
            CloseLabel.BackColor = Color.Transparent;

        }

        private async void searchByName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                

                long timeNow = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (searchByName.Text.Contains("%")) return;
                DataBase dataBase = new DataBase();
                dataBase.OpenConnection();
               
                //String[] name = searchByName.Text.Split(' ');
                // bool name_is_good = searchByName.Text.All => 
                string[] nameAndSurname;
                MySqlCommand cmd;
                try
                {
                    nameAndSurname = searchByName.Text.Split(' ', ' ');
                    if (nameAndSurname.Length==2 && !string.IsNullOrEmpty(searchByName.Text))
                    {
                         cmd = new MySqlCommand(
                            @"select ud.user_id as Логин, ud.worker_type as Тип, ud.department_type as Департамент, tab.name as Имя, tab.surname as Фамилия, 
                tab.mail as Почта, tab.phone as Телефон, tab.birth_date as `Дата Рождения` from user_dep ud inner join 
                (select ui.login_id, ui.name, ui.surname, ui.mail, ui.phone, ui.birth_date 
                from user_list ul left join user_info ui on ((ul.login=ui.login_id)) where ((ui.name like @name and ui.surname like @surname)))
                as tab on ud.user_id = tab.login_id order by id;"
                            , dataBase.GetConnection());
                        cmd.Parameters.AddWithValue("@name", nameAndSurname[0]);
                        cmd.Parameters.AddWithValue("@surname", nameAndSurname[1]);
                    }
                    else
                    {
                        throw new Exception();
                        // cmd.Parameters.AddWithValue("@name", searchByName.Text);
                        // cmd.Parameters.AddWithValue("@surname", null);
                    }

                }
                catch (Exception exception)
                {
                    
                    
                        cmd = new MySqlCommand(
                            @"select ud.user_id as Логин, ud.worker_type as Тип, ud.department_type as Департамент, tab.name as Имя, tab.surname as Фамилия, 
                tab.mail as Почта, tab.phone as Телефон, tab.birth_date as `Дата Рождения` from user_dep ud inner join 
                (select ui.login_id, ui.name, ui.surname, ui.mail, ui.phone, ui.birth_date 
                from user_list ul left join user_info ui on ((ul.login=ui.login_id)) where ((ui.name like @name)))
                as tab on ud.user_id = tab.login_id order by id;"
                            , dataBase.GetConnection());
                        cmd.Parameters.AddWithValue("@name",  searchByName.Text );
                        cmd.Parameters.AddWithValue("@surname", searchByName.Text);
                    
                    

                }

                
                
                MySqlDataReader dataReader = cmd.ExecuteReader();
                

                DataTable dataTable1 = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();

                dataGridView1.DataSource = dataTable1;
                dataReader.Close();
                adapter.SelectCommand = cmd;
                adapter.Fill(dataTable1);

                if (dataGridView1.Rows.Count <=1)
                {
                    dataGridView1.Visible = false;
                    label8.Visible = true;
                    label8.Text = "Ничего не найдено!";
                }
                else
                {
                    dataGridView1.Visible = true;
                    label8.Visible = true;
                    label8.Text = "Найдено рузльтатов: " + (dataGridView1.Rows.Count-1);
                }

                dataBase.CloseConnection();
                long timeEnd = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                label15.Text = "Время запроса:" + ((float) (timeEnd - timeNow) / 1000).ToString() + " сек";

                if (dataGridView1.Rows.Count > 1) label15.Visible = true;
                else label15.Visible = false;
            }
            catch (InvalidOperationException exception)
            {
                
                    using (FileStream fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                    {

                        byte[] array = Encoding.Default.GetBytes(exception.StackTrace);
                        // асинхронная запись массива байтов в файл
                        await fstream.WriteAsync(array,  0, array.Length);
                        await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                    }

                
            }
        }
        Point lastPoint;
        private void formMove(object sender, MouseEventArgs e)
        {
            int dx = e.X - lastPoint.X;
            int dy = e.Y - lastPoint.Y;
            if (e.Button.Equals(MouseButtons.Left))
            {
                this.Left += dx;
                this.Top += dy;
            }
        }

        private void formDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
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
            MinimizeLabel.BackColor = Color.FromArgb(70, 63, 72, 204);
        }
        private void MinimizeLeave(object sender, EventArgs e)
        {
            MinimizeLabel.BackColor = Color.Transparent;

        }

        private void войтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
                 new Auth().Show();
        
        }

        private void регистрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SignUp().Show();
        }

        private async void change(object sender, EventArgs e)
        {
            try
            {
                scienceLeader.SelectedItem = null;
                DataBase dataBase = new DataBase();
                dataBase.OpenConnection();
                List<String> list = new List<String>();

                list.Clear();
                scienceLeader.Items.Clear();
                MySqlCommand selectForLeader =
                    new MySqlCommand("select worker_type_officer from `worker_table_officer` where worker_type=@wt",
                        dataBase.GetConnection());
                selectForLeader.Parameters.AddWithValue("@wt", workingXPComboBox.SelectedItem);
                MySqlDataReader dataReader3 = selectForLeader.ExecuteReader();

                while (dataReader3.Read())
                {
                    String readData = dataReader3.GetValue(0).ToString();
                    if (!list.Contains(readData) && !scienceLeader.Items.Contains(readData))
                    {
                        list.Add(readData);

                        scienceLeader.Items.Add(readData);
                    }

                }

                dataReader3.Close();
                dataBase.CloseConnection();
            }
            catch (InvalidOperationException exception)
            {
                using (FileStream fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                {

                    byte[] array = Encoding.Default.GetBytes(exception.StackTrace);
                    // асинхронная запись массива байтов в файл
                    await fstream.WriteAsync(array,  0, array.Length);
                    await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                }

            }
        }

        private void размерыФайловToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().Show();
        }

        private async void change2(object sender, EventArgs e)
        {
            try
            {
                DataBase dataBase = new DataBase();
                dataBase.OpenConnection();

                HeadOfficer.Text = "";
                MySqlCommand officer = new MySqlCommand(
                    "select HeadOfficer from engineer_head_officer where department=@dep;"
                    , dataBase.GetConnection());
                officer.Parameters.AddWithValue("@dep", KnowledgeComboBox.SelectedItem);
                MySqlDataReader dataReader2 = officer.ExecuteReader();
                while (dataReader2.Read())
                {
                    HeadOfficer.Text = dataReader2.GetValue(0).ToString();
                }

                dataReader2.Close();
                dataBase.CloseConnection();

            }
            catch (InvalidOperationException exception)
            {
                using (FileStream fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                {

                    byte[] array = Encoding.Default.GetBytes(exception.StackTrace);
                    // асинхронная запись массива байтов в файл
                    await fstream.WriteAsync(array,  0, array.Length);
                    await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                }

            }
        }
    }
}
