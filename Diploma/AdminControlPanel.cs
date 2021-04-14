using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Threading;
using Diploma.Controllers;
using Diploma.Entities;


namespace Diploma
{
    /// <summary>
    /// Панель администратора
    /// </summary>
    public partial class AdminControlPanel : Form
    {
        private SHA512 _sha512;
        public AdminControlPanel()
        {
            InitializeComponent();
            label8.Visible = false;
            label16.Visible = false;

            if (_persons.Count == 0 || _persons==null)
            {
                menuComboBox.SelectedText = "История пуста";
                menuComboBox.SelectedItem = "История пуста";
            }
        }

        private readonly List<Person> _persons = new Person().GetPersons();
        
        
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

                UserControllerImpl impl = new UserControllerImpl();
                try
                {
                    impl.Get(searchField.Text);
                }
                catch (NoSuchUserException ex)
                {
                    MessageBox.Show("222222");
                }
                catch (Exception)
                {
                    return;
                }
                dataBase.CloseConnection();

                var cond = !string.IsNullOrEmpty(searchField.Text) &&
                           !string.IsNullOrEmpty(NameField.Text) &&
                           !string.IsNullOrEmpty(SurnameField.Text) &&
                           !string.IsNullOrEmpty(MailField.Text) &&
                           !string.IsNullOrEmpty(PhoneField.Text) &&
                           !string.IsNullOrEmpty(DateField.Text) &&
                           !string.IsNullOrEmpty(HeadOfficer.Text) &&
                           !string.IsNullOrEmpty(workingXPComboBox.SelectedItem.ToString()) &&
                           !string.IsNullOrEmpty(KnowledgeComboBox.SelectedItem.ToString());
                try
                {
                    if (cond &&
                        (!scienceLeader.SelectedItem.ToString().Equals(null) ||
                         !string.IsNullOrEmpty(scienceLeader.SelectedItem.ToString()) ||
                         !string.IsNullOrWhiteSpace(scienceLeader.SelectedItem.ToString())))

                    {

                        var person = new Person(searchField.Text, NameField.Text, SurnameField.Text, MailField.Text,
                            PhoneField.Text,
                            DateTime.Parse(DateField.Text),
                            HeadOfficer.Text, scienceLeader.SelectedItem.ToString(),
                            workingXPComboBox.SelectedItem.ToString(), KnowledgeComboBox.SelectedItem.ToString());
                        if (!menuComboBox.Items.Contains(person.ToString()))
                        {
                            menuComboBox.Items.Add(person.ToString());
                            _persons.Add(person);
                        }

                    }

                    if (cond &&
                        (scienceLeader.SelectedItem.ToString().Equals(null) ||
                         string.IsNullOrEmpty(scienceLeader.SelectedItem.ToString()) ||
                         string.IsNullOrWhiteSpace(scienceLeader.SelectedItem.ToString()))

                    )
                    {
                        var person = new Person(searchField.Text, NameField.Text, SurnameField.Text, MailField.Text,
                            PhoneField.Text,
                            DateTime.Parse(DateField.Text), HeadOfficer.Text,
                            workingXPComboBox.SelectedItem.ToString(), KnowledgeComboBox.SelectedItem.ToString());
                        if (!menuComboBox.Items.Contains(person.ToString()))
                        {
                            menuComboBox.Items.Add(person.ToString());
                            _persons.Add(person);
                        }
                    }
                }
                catch (Exception)
                {
                    var person = new Person(searchField.Text, NameField.Text, SurnameField.Text, MailField.Text,
                        PhoneField.Text,
                        DateTime.Parse(DateField.Text), HeadOfficer.Text,
                        workingXPComboBox.SelectedItem.ToString(), KnowledgeComboBox.SelectedItem.ToString());
                    if (!menuComboBox.Items.Contains(person.ToString()))
                    {
                        menuComboBox.Items.Add(person.ToString());
                        _persons.Add(person);
                    }
                }
                
            }
            catch (InvalidOperationException exception)
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
            catch (NoSuchUserException exception)
            {
            }
        }

        //удалить
        private async void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchField.Text))
            if ((MessageBox.Show(@"Удалить данного пользователя? Это действие нельзя отменить!", @"Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) == DialogResult.Yes)
                try
                {
                    Person person = null;
                    try
                    {
                        person = new Person(searchField.Text, NameField.Text, SurnameField.Text, MailField.Text,
                            PhoneField.Text,
                            DateTime.Parse(DateField.Text), HeadOfficer.Text,
                            workingXPComboBox.SelectedItem.ToString(), KnowledgeComboBox.SelectedItem.ToString());
                        menuComboBox.Items.Remove(person.ToString());

                    }
                    catch (FormatException)
                    {

                    }
                    catch (NullReferenceException)
                    {
                        
                    }

                    var impl = new UserControllerImpl();
                    try
                    {
                        impl.Get(searchField.Text);

                        var person1 = new Person(searchField.Text, NameField.Text, SurnameField.Text, MailField.Text,
                            PhoneField.Text,
                            DateTime.Parse(DateField.Text), HeadOfficer.Text,
                            workingXPComboBox.SelectedItem.ToString(), KnowledgeComboBox.SelectedItem.ToString());
                        if (_persons.Contains(person1))
                        {
                            impl.Delete(searchField.Text);
                        }
                    }
                    catch (NoSuchUserException)
                    {
                        MessageBox.Show(@"Такого пользователя нет в базе");
                        return;
                    }
                    catch (FormatException)
                    {
                        return;
                    }
                    finally
                    {
                        impl.Delete(searchField.Text);
                    }
                

                 

                    MessageBox.Show(@"Пользователь удален");
                    if (_persons.Contains(person))
                    {
                        _persons.Remove(person);
                        menuComboBox.Items.Remove(person.ToString());
                    }
                    EmptyFields();
                    searchField.Text = "";
                    
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
                if (!searchField.Equals(null))
                    try
                    {
                                          
                       UserControllerImpl userControllerImpl = new UserControllerImpl();
                       userControllerImpl.Update(searchField.Text);

                        var person = new Person(searchField.Text, NameField.Text, SurnameField.Text, MailField.Text,
                            PhoneField.Text,
                            DateTime.Parse(DateField.Text),
                            HeadOfficer.Text, scienceLeader.SelectedItem.ToString(),
                            workingXPComboBox.SelectedItem.ToString(), KnowledgeComboBox.SelectedItem.ToString());
                        if (menuComboBox.Items.Contains(person.ToString()))
                        {
                            menuComboBox.Items.Remove(person.ToString());
                            menuComboBox.Items.Add(person.ToString());
    
                        }

                        EmptyFields();
                        searchField.Text = "";
                        MessageBox.Show($@"Изменено успешно");
                        var database = new DataBase();
                        database.OpenConnection();
                        var enable = new MySqlCommand("SET SQL_SAFE_UPDATES = 1;", database.GetConnection()); 
                        enable.ExecuteNonQuery();
                        database.CloseConnection();
                    }
                    catch (InvalidOperationException exception)
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
                    catch (NullReferenceException)
                    {
                              var person = new Person(searchField.Text, NameField.Text, SurnameField.Text, MailField.Text,
                               PhoneField.Text,
                               DateTime.Parse(DateField.Text),
                               HeadOfficer.Text, 
                               workingXPComboBox.SelectedItem.ToString(), KnowledgeComboBox.SelectedItem.ToString());
                           if (menuComboBox.Items.Contains(person.ToString()))
                           {
                               menuComboBox.Items.Remove(person.ToString());
                               menuComboBox.Items.Add(person.ToString());
                           }
                        //MessageBox.Show("Убедитесь, что все поля заполнены!");
                        MessageBox.Show(@"Изменено успешно");
                        EmptyFields();
                        searchField.Text = "";

                    }
                    catch (FormatException)
                    {
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

        }

        private void CloseClick(object sender, EventArgs e)
        {
          Application.Exit();
        }
        private void CloseHover(object sender, EventArgs e)
        {
            label30.BackColor = Color.Red;
        }

        private void CloseLeave(object sender, EventArgs e)
        {
            label30.BackColor = Color.Transparent;

        }

        private async void searchByName_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(searchByName.Text)|| string.IsNullOrWhiteSpace(searchByName.Text))
                {
                    label8.Text = @"Введен пустой запрос";
                    label15.Visible = false;
                    dataGridView1.Visible = false;
                    return;
                }
                var timeNow = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (searchByName.Text.Contains("%")) return;
                var dataBase = new DataBase();
                dataBase.OpenConnection();
               
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
                        cmd.Parameters.AddWithValue("@name", "%"+nameAndSurname[0].Split(' ')[0]+"%");
                        cmd.Parameters.AddWithValue("@surname", "%"+nameAndSurname[1]+"%");
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    
                    
                        cmd = new MySqlCommand(
                            @"select ud.user_id as Логин, ud.worker_type as Тип, ud.department_type as Департамент, tab.name as Имя, tab.surname as Фамилия, 
                tab.mail as Почта, tab.phone as Телефон, tab.birth_date as `Дата Рождения` from user_dep ud inner join 
                (select ui.login_id, ui.name, ui.surname, ui.mail, ui.phone, ui.birth_date 
                from user_list ul left join user_info ui on ((ul.login=ui.login_id)) where ((ui.name like @name)||(ui.surname like @surname)))
                as tab on ud.user_id = tab.login_id order by id;"
                            , dataBase.GetConnection());
                        cmd.Parameters.AddWithValue("@name",  "%"+searchByName.Text+"%" );
                        cmd.Parameters.AddWithValue("@surname", "%"+searchByName.Text+"%");
                    
                    

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            label29.BackColor = Color.FromArgb(70, 63, 72, 204);
        }
        private void MinimizeLeave(object sender, EventArgs e)
        {
            label29.BackColor = Color.Transparent;

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

        [Obsolete("is deprecated.")]
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(dataGridView1.CurrentCell.Value.ToString());
            
        }
        private void смотретьПолнуюИнформациюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var impl = new UserControllerImpl();
           
            var text = dataGridView1.CurrentCell.Value.ToString();
            searchField.Text = !text.StartsWith("user") ? dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString() : text;
            impl.Get(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].ToString());
            tabControl1.SelectTab(0);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            dataGridView1.CurrentCell.ContextMenuStrip = contextMenuStrip1;
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dataBase = new DataBase();
            dataBase.OpenConnection();
            MessageBox.Show(
                $@"Этот программный продукт позволяет управлять базой данных пользователей в графическом интерфейсе.
Разработано и спроектировано Антоном Юрковым.
----------------------------------------------
Версии:
Версия mySQL - {dataBase.ShowVersion()}
Версия .NET Framework - {RuntimeInformation.FrameworkDescription}
Версия ОС - {RuntimeInformation.OSDescription}
Версия CLR - {Environment.Version}
Версия данного программного продукта - {Application.ProductVersion}");
            dataBase.CloseConnection();
        }

       private async void button2_Click(object sender, EventArgs e)
       {
         //  saveFileDialog1.ShowDialog();
           saveFileDialog1.Filter = @"JSON|*.json";
           saveFileDialog1.FileName = $"{searchField.Text}";

           if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
           try
           {
               using (var fstream =
                   new FileStream(
                       //$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/info_about{searchField.Text}.txt",
                       $"{saveFileDialog1.FileName}", 
                       FileMode.OpenOrCreate))
               {
                   var listoflabels = new List<byte[]>()
                   {
                       Encoding.UTF8.GetBytes("Логин"),
                       Encoding.UTF8.GetBytes(label6.Text),
                       Encoding.UTF8.GetBytes(label5.Text),
                       Encoding.UTF8.GetBytes(label4.Text),
                       Encoding.UTF8.GetBytes(label3.Text),
                       Encoding.UTF8.GetBytes(label2.Text),
                       Encoding.UTF8.GetBytes(label1.Text),
                       Encoding.UTF8.GetBytes(label9.Text),
                       Encoding.UTF8.GetBytes(label10.Text),
                       Encoding.UTF8.GetBytes(label7.Text),
                       Encoding.UTF8.GetBytes(label14.Text)

                   };
                   if (listoflabels.Any(t => t == null))
                   {
                       return;
                   }

                   var listoftextboxes = new List<byte[]>()
                   {
                       Encoding.UTF8.GetBytes(searchField.Text),
                       Encoding.UTF8.GetBytes(NameField.Text),
                       Encoding.UTF8.GetBytes(SurnameField.Text),
                       Encoding.UTF8.GetBytes(PasswordField.Text),
                       Encoding.UTF8.GetBytes(MailField.Text),
                       Encoding.UTF8.GetBytes(PhoneField.Text),
                       Encoding.UTF8.GetBytes(DateField.Text),
                       Encoding.UTF8.GetBytes(workingXPComboBox.SelectedItem.ToString()),
                       Encoding.UTF8.GetBytes(KnowledgeComboBox.SelectedItem.ToString()),
                       Encoding.UTF8.GetBytes(HeadOfficer.Text),
                       Encoding.UTF8.GetBytes(scienceLeader.SelectedItem==null?" ":scienceLeader.SelectedItem.ToString())
                   };
                   if (listoftextboxes.Any(t => t == null))
                   {
                       return;
                   }

                   // асинхронная запись массива байтов в файл
                   await fstream.WriteAsync(new byte[] {123}, 0, 1);
                   for (var i = 0; i < listoflabels.Count-1; i++)
                   {
                       if (listoflabels[i] == null || listoftextboxes[i] == null) continue;
                       await fstream.WriteAsync(new byte[] {34}, 0, 1);

                       await fstream.WriteAsync(listoflabels[i], 0, listoflabels[i].Length);
                       await fstream.WriteAsync(new byte[] {34}, 0, 1);

                       await fstream.WriteAsync(new byte[] {58}, 0, 1);
                       await fstream.WriteAsync(new byte[] {34}, 0, 1);

                       await fstream.WriteAsync(listoftextboxes[i], 0, listoftextboxes[i].Length);
                       await fstream.WriteAsync(new byte[] {34}, 0, 1);
                       await fstream.WriteAsync(new byte[] {44}, 0, 1);

                       await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                   }

                   for (var i = listoflabels.Count-1; i < listoflabels.Count; i++)
                   {
                       if (listoflabels[i] == null || listoftextboxes[i] == null) continue;
                       await fstream.WriteAsync(new byte[] {34}, 0, 1);

                       await fstream.WriteAsync(listoflabels[i], 0, listoflabels[i].Length);
                       await fstream.WriteAsync(new byte[] {34}, 0, 1);

                       await fstream.WriteAsync(new byte[] {58}, 0, 1);
                       await fstream.WriteAsync(new byte[] {34}, 0, 1);

                       await fstream.WriteAsync(listoftextboxes[i], 0, listoftextboxes[i].Length);
                       await fstream.WriteAsync(new byte[] {34}, 0, 1);

                       await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);
                   }

                   await fstream.WriteAsync(new byte[] {125}, 0, 1);

               }
           }
           catch (Exception)
           {
               MessageBox.Show(@"Убедитесь, что все поля заполнены!");
                
           }
       }

 
       private void workbenchToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Process.Start("C:\\Program Files\\MySQL\\MySQL Workbench 8.0 CE\\MySQLWorkbench.exe");          
           Thread.Sleep(100);
       }


      
        
       private void оБазеДанныхToolStripMenuItem_Click(object sender, EventArgs e)
       {
           
           var countUsers = 0;
           var countTables = 0;
           var database = new DataBase();
           try
           {
        
               database.OpenConnection();
               var cmd = new MySqlCommand("select count(*) from user_info", database.GetConnection());
               var datareader = cmd.ExecuteReader();
               while (datareader.Read())
               {
                   countUsers = int.Parse(datareader.GetValue(0).ToString());
               }
               
               datareader.Close();
               database.OpenConnection();

               var cmd2 = new MySqlCommand("SELECT count(*) FROM INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA='userdb'",
                   database.GetConnection());
               var datareader2 = cmd2.ExecuteReader();
               while (datareader2.Read())
               {
                   countTables = int.Parse(datareader2.GetValue(0).ToString());
               }
               database.CloseConnection();
           }
           catch (Exception exception)
           {
               MessageBox.Show(exception.Message);
           }
           finally
           {
               MessageBox.Show(@"Количество пользователей в базе данных - " + countUsers + '\n'+
                               $@"Количество таблиц в базе данных {database.GetConnection().Database} - " + countTables);
           }
       }



       private void очиститьИсториюПросмотровToolStripMenuItem_Click(object sender, EventArgs e)
       {
           if (menuComboBox.Items.Count == 0) return;
           else
           {
               menuComboBox.Items.Clear();
               new Person().ClearAllPersons();
               menuComboBox.SelectedItem = null;
           }
       }


       private void удалитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
       {
            if ((MessageBox.Show(@"Удалить данного пользователя? Это действие нельзя отменить!", @"Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) == DialogResult.Yes)
                try
                {
                    
                    var text = 
                        dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();

                    var dataBase = new DataBase();
                    dataBase.OpenConnection();

                    UserControllerImpl impl = new UserControllerImpl();
                    impl.Delete(text);

                    EmptyFields();
                    searchField.Text = "";

                    MessageBox.Show(@"Пользователь удален");
                    dataBase.CloseConnection();
                }
                catch (Exception exception)
                {
                    var message = exception.Message;
                    Console.WriteLine(message);
                }
       }

       private void HistoryChangedIndex(object sender, EventArgs e)
       {
           searchField.Text = menuComboBox.SelectedItem.ToString();
           
       }

    

       private void button5_Click(object sender, EventArgs e)
       {
           var dataBase = new DataBase();
           
           using (var connection = dataBase.GetConnection())
           {

               if (dataBase.Ping())
                  dataBase.OpenConnection();
               else
               {
                   MessageBox.Show(@"Невозможно подключиться к базе");
                   return;
               }
               MySqlCommand CommandBefore = null;
               if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked)
                   try
                   {
                       var datePicked = dateTimePicker1.Value;

                       if (radioButton1.Checked)
                       {
                           CommandBefore = new MySqlCommand(
                               "select login_id as `Логин`, name as `Имя`, surname as `Фамилия`, mail as `Почта`, phone as `Номер Телефона`, birth_date as `Дата рождения` from user_info where birth_date<@datepicked;",
                               dataBase.GetConnection());
                           CommandBefore.Parameters.AddWithValue("@datepicked", datePicked);
                       }

                       if (radioButton2.Checked)
                       {
                           CommandBefore = new MySqlCommand(
                               "select login_id as `Логин`, name as `Имя`, surname as `Фамилия`, mail as `Почта`, phone as `Номер Телефона`, birth_date as `Дата рождения` from user_info where birth_date>@datepicked;",
                               dataBase.GetConnection());
                           CommandBefore.Parameters.AddWithValue("@datepicked", datePicked);
                       }

                       if (radioButton3.Checked)
                       {
                           CommandBefore = new MySqlCommand(
                               "select login_id as `Логин`, name as `Имя`, surname as `Фамилия`, mail as `Почта`, phone as `Номер Телефона`, birth_date as `Дата рождения` from user_info where birth_date=@datepicked;",
                               dataBase.GetConnection());
                           CommandBefore.Parameters.AddWithValue("@datepicked", datePicked);
                       }
                   }
                   catch (Exception)
                   {
                       return;
                   }
               else
               {
                   MessageBox.Show("Убедитесь, что выделено значение параметра \"До\",\"После\" или \"В\" ");

               }

               MySqlDataReader dataReader = null;
               try
               {
                   if (CommandBefore != null) dataReader = CommandBefore.ExecuteReader();
                   else
                   {
                       throw new NullReferenceException();
                   }
               }
               catch (NullReferenceException)
               {
                   return;
               }

               var dataTable1 = new DataTable();
               var adapter = new MySqlDataAdapter();

               dataGridView2.DataSource = dataTable1;
               dataReader.Close();
               adapter.SelectCommand = CommandBefore;
               adapter.Fill(dataTable1);
               dataGridView2.Visible = dataGridView2.Rows.Count > 1;
               label16.Visible = true;
               label16.Text = @"Найдено " + (dataGridView2.Rows.Count - 1).ToString()
                                          + @" результат(ов), соответствующих заданным параметрам";
           }
       }

       private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
       {
           dataGridView2.CurrentCell.ContextMenuStrip = contextMenuStrip2;

       }

       private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
       {
           var text = dataGridView2.CurrentCell.Value.ToString();
           if (!text.StartsWith("user")) return;
           tabControl1.SelectTab(0);
           searchField.Text = text;
       }


       private void полнаяИнформацияToolStripMenuItem_Click(object sender, EventArgs e)
       {
           var text = dataGridView2.CurrentCell.Value.ToString();
           searchField.Text = !text.StartsWith("user") ? dataGridView2[0, dataGridView2.CurrentCell.RowIndex].Value.ToString() : text;
           tabControl1.SelectTab(0);       }


       private void копироватьЗначениеToolStripMenuItem_Click(object sender, EventArgs e)
       {
           Clipboard.SetText(dataGridView2.CurrentCell.Value.ToString());
       }

       private void проверитьСоединениеToolStripMenuItem_Click(object sender, EventArgs e)
       {
           DataBase dataBase = new DataBase();
           MessageBox.Show(dataBase.Ping() ? "Соединение прошло успешно" : "Соединение не прошло");
       }

      

       private void смотретьБазуToolStripMenuItem_Click(object sender, EventArgs e)
       {
           new ShowTables().Show();       }

        private void показатьБазуДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Антон\RiderProjects\DB Search\DB Search\bin\Debug\net5.0-windows\DB Search.exe";
            //new ShowTables().Show();
            Process.Start(path);          
            Thread.Sleep(100);
        }


        private async void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                string line = "";
                saveFileDialog1.Filter = @"CSV|*.csv";
                saveFileDialog1.FileName = "List of people";

                if (dataGridView2.RowCount > 1)
                {
                    if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

                    for (var j = 0; j < dataGridView2.RowCount - 1; j++)
                    {
                        for (int i = 0; i < dataGridView2.ColumnCount - 1; i++)
                        {
                            line += dataGridView2[i, j].Value.ToString();
                            line += ",";
                        }

                        for (int i = dataGridView2.Columns.Count - 1; i < dataGridView2.ColumnCount; i++)
                        {
                            line += dataGridView2[i, 0].Value.ToString();
                            line += "\r\n";
                        }
                    }
                }

                label31.Text = line;
                using (var fstream =
                    new FileStream(
                        $"{saveFileDialog1.FileName}",
                        FileMode.OpenOrCreate))
                {

                    var array = Encoding.Default.GetBytes(line);
                    await fstream.WriteAsync(array, 0, array.Length);

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Файл открыт в другой программе");
            }
        }
    }
}
    

