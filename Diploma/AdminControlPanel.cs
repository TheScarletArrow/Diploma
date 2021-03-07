using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;


namespace Diploma
{
    /// <summary>
    /// Панель администратора
    /// </summary>
    public partial class AdminControlPanel : Form
    {
        private readonly SignUp _signUp = new SignUp();
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
        }
        private async void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {

                var dataBase = new DataBase();
                dataBase.OpenConnection();
                EmptyFields();
                var cmd = new MySqlCommand(
                    @"select ui.login_id, ui.name, ui.surname, ul.password,
                ui.mail, ui.phone, ui.birth_date from user_list ul left join
                user_info ui on ((ul.login=ui.login_id)) where ui.login_id=@id;"
                    , dataBase.GetConnection());
                cmd.Parameters.AddWithValue("@id", searchField.Text);

                var dataReader = cmd.ExecuteReader();


                var passwordCached = PasswordField.Text;

                while (dataReader.Read())
                {
                    NameField.Text = dataReader.GetValue(1).ToString();
                    SurnameField.Text = dataReader.GetValue(2).ToString();
                    passwordCached = dataReader.GetValue(3).ToString();
                    PasswordField.Text = passwordCached;
                    MailField.Text = dataReader.GetValue(4).ToString();
                    PhoneField.Text = dataReader.GetValue(5).ToString();
                    DateField.Text = dataReader.GetValue(6).ToString();
                }

                dataReader.Close();

                var cmd2 = new MySqlCommand(
                    "select ud.user_id, ud.worker_type, ud.department_type, eho.HeadOfficer from user_dep ud left join engineer_head_officer eho on ud.department_type=Department where  user_id=@ui;"
                    , dataBase.GetConnection());
                cmd2.Parameters.AddWithValue("@ui", searchField.Text);
                var dataReader1 = cmd2.ExecuteReader();

                while (dataReader1.Read())
                {
                    workingXPComboBox.SelectedItem = dataReader1.GetValue(1).ToString();
                    KnowledgeComboBox.SelectedItem = dataReader1.GetValue(2).ToString();
                    HeadOfficer.Text = dataReader1.GetValue(3).ToString();
                }

                dataReader1.Close();

                var selectScienceLeader = new MySqlCommand
                ("select ux.leader_name from user_xp ux left join worker_table_officer wo on wo.worker_type_officer=ux.leader_name where login=@ul;"
                    , dataBase.GetConnection());
                selectScienceLeader.Parameters.AddWithValue("@ul", searchField.Text);
                var sceinceLeaderReader = selectScienceLeader.ExecuteReader();
                while (sceinceLeaderReader.Read())
                {
                    scienceLeader.SelectedItem = sceinceLeaderReader.GetValue(0).ToString();
                }

                sceinceLeaderReader.Close();


                dataBase.CloseConnection();
            }
            catch (InvalidOperationException exception)
            {
                using (var fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                {

                    var array = Encoding.Default.GetBytes(exception.StackTrace);
                    // асинхронная запись массива байтов в файл
                    await fstream.WriteAsync(array,  0, array.Length);
                    await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                }

            }
                

        }

        //удалить
        private async void button3_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show(@"Удалить данного пользователя? Это действие нельзя отменить!", @"Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) == DialogResult.Yes)
                try
                {
                    var dataBase = new DataBase();
                    dataBase.OpenConnection();

                    var dropFromUserInfo = new MySqlCommand("delete from user_info where login_id=@login;",
                        dataBase.GetConnection());
                    var dropFromUserList =
                        new MySqlCommand("delete from user_list where login=@login;", dataBase.GetConnection());
                    var dropFromUserDep =
                        new MySqlCommand("delete from user_dep where user_id=@login", dataBase.GetConnection());
                    var dropFromUserXp =
                        new MySqlCommand("delete from user_xp where login=@login", dataBase.GetConnection());

                    dropFromUserInfo.Parameters.AddWithValue("@login", searchField.Text);
                    dropFromUserList.Parameters.AddWithValue("@login", searchField.Text);
                    dropFromUserDep.Parameters.AddWithValue("@login", searchField.Text);
                    dropFromUserXp.Parameters.AddWithValue("@login", searchField.Text);

                    dropFromUserList.ExecuteNonQuery();
                    dropFromUserDep.ExecuteNonQuery();
                    dropFromUserInfo.ExecuteNonQuery();
                    dropFromUserXp.ExecuteNonQuery();

                    dataBase.CloseConnection();

                    EmptyFields();
                    searchField.Text = "";

                    MessageBox.Show(@"Пользователь удален");
                }
                catch (InvalidOperationException exception)
                {
                    using (var fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                    {

                        var array = Encoding.Default.GetBytes(exception.StackTrace);
                        // асинхронная запись массива байтов в файл
                        await fstream.WriteAsync(array,  0, array.Length);
                        await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                    }
                }

        }

        //изменить
        private async void button1_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show(@"Изменить данные данного пользователя?", @"Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) == DialogResult.Yes)

             try
             {
                 var password = PasswordField.Text;
                 var password1 = "";
                 var database = new DataBase();
                 database.OpenConnection();
                 var getPasswordFromDb = new MySqlCommand("select password from user_list where login=@ul",
                     database.GetConnection());
                 getPasswordFromDb.Parameters.AddWithValue("@ul", searchField.Text);
                 var readPassword = getPasswordFromDb.ExecuteReader();
                 while (readPassword.Read())
                 {
                     password1 = readPassword.GetValue(0).ToString();
                 }

                 readPassword.Close();


                 var change = new MySqlCommand
                 ("update user_info set name=@name,surname=@surname,mail=@mail,phone=@phone,birth_date=@birthdate where login_id=@login;"
                     , database.GetConnection());

                 change.Parameters.AddWithValue("@name", NameField.Text);
                 change.Parameters.AddWithValue("@surname", SurnameField.Text);
                 change.Parameters.AddWithValue("@mail", MailField.Text);
                 change.Parameters.AddWithValue("@phone", PhoneField.Text);
                 change.Parameters.AddWithValue("@birthdate", DateTime.Parse(DateField.Text));
                 change.Parameters.AddWithValue("@login", searchField.Text);
                 change.ExecuteNonQuery();


                 var change2 = new MySqlCommand
                 ("update user_dep set worker_type=@wt, department_type=@dt where user_id=@login;",
                     database.GetConnection());
                 change2.Parameters.AddWithValue("@login", searchField.Text);
                 change2.Parameters.AddWithValue("@wt", workingXPComboBox.SelectedItem);
                 change2.Parameters.AddWithValue("@dt", KnowledgeComboBox.SelectedItem);
                 change2.ExecuteNonQuery();

                 var changeUserXp = new MySqlCommand
                 ("update user_xp set leader_name=@leader where login=@login;"
                     , database.GetConnection());
                 changeUserXp.Parameters.AddWithValue("@login", searchField.Text);
                 changeUserXp.Parameters.AddWithValue("@leader", scienceLeader.SelectedItem);
                 changeUserXp.ExecuteNonQuery();

                 if (!password.Equals(password1))
                 {
                     var changePassword = new MySqlCommand
                     ("update user_list set password=@pass where login=@login;"
                         , database.GetConnection());
                     changePassword.Parameters.AddWithValue("@pass",
                         Convert.ToBase64String(_signUp.Sha512.ComputeHash(Encoding.UTF8.GetBytes(PasswordField.Text))));
                     changePassword.Parameters.AddWithValue("@login", searchField.Text);
                     changePassword.ExecuteNonQuery();
                 }
                 else
                 {
                 }


                 var disable = new MySqlCommand("SET SQL_SAFE_UPDATES = 0;", database.GetConnection());
                 disable.ExecuteNonQuery();


                 var change3 = new MySqlCommand(
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
                 using (var fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                 {

                     var array = Encoding.Default.GetBytes(exception.StackTrace);
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
                

                var timeNow = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (searchByName.Text.Contains("%")) return;
                var dataBase = new DataBase();
                dataBase.OpenConnection();
               
                //String[] name = searchByName.Text.Split(' ');
                // bool name_is_good = searchByName.Text.All => 
                MySqlCommand cmd;
                try
                {
                    var nameAndSurname = searchByName.Text.Split(' ', ' ');
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
                catch (Exception)
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

                
                
                var dataReader = cmd.ExecuteReader();
                

                var dataTable1 = new DataTable();
                var adapter = new MySqlDataAdapter();

                dataGridView1.DataSource = dataTable1;
                
                dataReader.Close();
                adapter.SelectCommand = cmd;
                adapter.Fill(dataTable1);

                if (dataGridView1.Rows.Count <=1)
                {
                    dataGridView1.Visible = false;
                    label8.Visible = true;
                    label8.Text = @"Ничего не найдено!";
                }
                else
                {
                    dataGridView1.Visible = true;
                    label8.Visible = true;
                    label8.Text = @"Найдено рузльтатов: " + (dataGridView1.Rows.Count-1);
                }

                dataBase.CloseConnection();
                var timeEnd = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                label15.Text = @"Время запроса:" + ((float) (timeEnd - timeNow) / 1000) + @" сек";

                label15.Visible = dataGridView1.Rows.Count > 1;
            }
            catch (InvalidOperationException exception)
            {
                
                    using (var fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                    {

                        var array = Encoding.Default.GetBytes(exception.StackTrace);
                        // асинхронная запись массива байтов в файл
                        await fstream.WriteAsync(array,  0, array.Length);
                        await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                    }

                
            }
        }

        private Point _lastPoint;
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

        private async void Change(object sender, EventArgs e)
        {
            try
            {
                scienceLeader.SelectedItem = null;
                var dataBase = new DataBase();
                dataBase.OpenConnection();
                var list = new List<String>();

                list.Clear();
                scienceLeader.Items.Clear();
                var selectForLeader =
                    new MySqlCommand("select worker_type_officer from `worker_table_officer` where worker_type=@wt",
                        dataBase.GetConnection());
                selectForLeader.Parameters.AddWithValue("@wt", workingXPComboBox.SelectedItem);
                var dataReader3 = selectForLeader.ExecuteReader();

                while (dataReader3.Read())
                {
                    var readData = dataReader3.GetValue(0).ToString();
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
                using (var fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                {

                    var array = Encoding.Default.GetBytes(exception.StackTrace);
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

        private async void Change2(object sender, EventArgs e)
        {
            try
            {
                var dataBase = new DataBase();
                dataBase.OpenConnection();

                HeadOfficer.Text = "";
                var officer = new MySqlCommand(
                    "select HeadOfficer from engineer_head_officer where department=@dep;"
                    , dataBase.GetConnection());
                officer.Parameters.AddWithValue("@dep", KnowledgeComboBox.SelectedItem);
                var dataReader2 = officer.ExecuteReader();
                while (dataReader2.Read())
                {
                    HeadOfficer.Text = dataReader2.GetValue(0).ToString();
                }

                dataReader2.Close();
                dataBase.CloseConnection();

            }
            catch (InvalidOperationException exception)
            {
                using (var fstream = new FileStream($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log", FileMode.Append))
                {

                    var array = Encoding.Default.GetBytes(exception.StackTrace);
                    // асинхронная запись массива байтов в файл
                    await fstream.WriteAsync(array,  0, array.Length);
                    await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                }

            }
        }

        private void AdminControlPanel_Load(object sender, EventArgs e)
        {
            var t = new ToolTip();
            t.SetToolTip(button1, "Изменить данные пользователя");
      
            var t1 = new ToolTip();
            t.SetToolTip(searchField, "Введите логин пользователя");
            t1.SetToolTip(NameField, "Имя пользователя");
            t1.SetToolTip(SurnameField, "Фамилия пользователя");
            t1.SetToolTip(PasswordField, "Пароль пользователя");
            t1.SetToolTip(PhoneField, "Телефон пользователя");
            t1.SetToolTip(DateField, "Дата рождения");
            t1.SetToolTip(workingXPComboBox, "Выберите стаж");
            t1.SetToolTip(KnowledgeComboBox, "Выберите сферу деятельности");
            t1.SetToolTip(scienceLeader, "Выберите начальника отдела");
            var t2 = new ToolTip();

            t2.SetToolTip(HeadOfficer, string.IsNullOrEmpty(HeadOfficer.Text) ? null : "Начальник");
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var text = dataGridView1.CurrentCell.Value.ToString();
            if (!text.StartsWith("user")) return;
            tabControl1.SelectTab(0);
            searchField.Text = text;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
                
           

            var dataGridViewSelectedCellCollection = dataGridView1.CurrentCell.Value;
            
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(dataGridView1.CurrentCell.Value.ToString());
        }

        private void смотретьПолнуюИнформациюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var text = dataGridView1.CurrentCell.Value.ToString();
            searchField.Text = !text.StartsWith("user") ? dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString() : text;
            tabControl1.SelectTab(0);
            //searchField.Text = text;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentCell.ContextMenuStrip = contextMenuStrip1;
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $@"Этот программный продукт позволяет управлять базой данных пользователей в графическом интерфейсе.
Разработано и спроектировано Антоном Юрковым.
----------------------------------------------
Версии:
Версия mySQL - Server 5.5
Версия .NET Framework - {RuntimeInformation.FrameworkDescription}
Версия ОС - {RuntimeInformation.OSDescription}
Версия CLR - {Environment.Version}
Версия данного программного продукта - {Application.ProductVersion}");
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fstream =
                    new FileStream(
                        $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/info_about{searchField.Text}.txt",
                        FileMode.Append))
                {
                    var listoflabels = new List<byte[]>()
                    {
                        Encoding.Default.GetBytes("Логин"),
                        Encoding.Default.GetBytes(label6.Text),
                        Encoding.Default.GetBytes(label5.Text),
                        Encoding.Default.GetBytes(label4.Text),
                        Encoding.Default.GetBytes(label3.Text),
                        Encoding.Default.GetBytes(label2.Text),
                        Encoding.Default.GetBytes(label1.Text),
                        Encoding.Default.GetBytes(label9.Text),
                        Encoding.Default.GetBytes(label10.Text),
                        Encoding.Default.GetBytes(label7.Text),
                        Encoding.Default.GetBytes(label14.Text)

                    };
                    for (int i = 0; i < listoflabels.Count ; i++)
                    {
                        if (listoflabels[i] == null)
                            return;
                    }

                    var listoftextboxes = new List<byte[]>()
                    {
                        Encoding.Default.GetBytes(searchField.Text),
                        Encoding.Default.GetBytes(NameField.Text),
                        Encoding.Default.GetBytes(SurnameField.Text),
                        Encoding.Default.GetBytes(PasswordField.Text),
                        Encoding.Default.GetBytes(MailField.Text),
                        Encoding.Default.GetBytes(PhoneField.Text),
                        Encoding.Default.GetBytes(DateField.Text),
                        Encoding.Default.GetBytes(workingXPComboBox.SelectedItem.ToString()),
                        Encoding.Default.GetBytes(KnowledgeComboBox.SelectedItem.ToString()),
                        Encoding.Default.GetBytes(HeadOfficer.Text),
                        Encoding.Default.GetBytes(scienceLeader.SelectedItem==null?" ":scienceLeader.SelectedItem.ToString())
                    };
                    for (int i = 0; i < listoftextboxes.Count; i++)
                    {
                        if (listoftextboxes[i] == null)
return;
                        
                    }

                    // асинхронная запись массива байтов в файл
                    await fstream.WriteAsync(new byte[] {123}, 0, 1);
                    for (var i = 0; i < listoflabels.Count; i++)
                    {
                        if (listoflabels[i] != null && listoftextboxes[i] != null)
                        {

                            await fstream.WriteAsync(new byte[] {123}, 0, 1);
                            await fstream.WriteAsync(listoflabels[i], 0, listoflabels[i].Length);
                            await fstream.WriteAsync(new byte[] {58}, 0, 1);

                            await fstream.WriteAsync(listoftextboxes[i], 0, listoftextboxes[i].Length);

                            await fstream.WriteAsync(new byte[] {125}, 0, 1);
                            await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);
                        }
                        else if (listoflabels[i] == null)
                        {

                            await fstream.WriteAsync(new byte[] {123}, 0, 1);
                            await fstream.WriteAsync(new byte[] {32}, 0, 1);
                            await fstream.WriteAsync(new byte[] {58}, 0, 1);

                            await fstream.WriteAsync(listoftextboxes[i], 0, listoftextboxes[i].Length);

                            await fstream.WriteAsync(new byte[] {125}, 0, 1);
                            await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);
                        }
                        else
                        {

                            await fstream.WriteAsync(new byte[] {123}, 0, 1);
                            await fstream.WriteAsync(listoflabels[i], 0, listoflabels[i].Length);
                            await fstream.WriteAsync(new byte[] {58}, 0, 1);

                            await fstream.WriteAsync(new byte[] {32}, 0, 1);

                            await fstream.WriteAsync(new byte[] {125}, 0, 1);
                            await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);
                        }
                    }

                    await fstream.WriteAsync(new byte[] {125}, 0, 1);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Убедитесь, что все поля заполнены!");
                
            }
        }
    }
}
