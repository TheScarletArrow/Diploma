using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Diploma
{
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

        }
        private void textBox7_TextChanged(object sender, System.EventArgs e)
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
            }
            dataReader1.Close();

            MySqlCommand officer = new MySqlCommand("select eho.HeadOfficer from user_dep ud left join engineer_head_officer eho on ud.department_type=Department where user_id=@id;", dataBase.GetConnection());
            officer.Parameters.AddWithValue("@id", searchField.Text);
            MySqlDataReader dataReader2 = officer.ExecuteReader();
            while (dataReader2.Read())
            {
                HeadOfficer.Text = dataReader2.GetValue(0).ToString();
            }
            dataReader2.Close();

            MySqlCommand SelectScienceLeader = new MySqlCommand("select * from user_xp where login = @ul;"
                , dataBase.GetConnection());
            SelectScienceLeader.Parameters.AddWithValue("@ul", searchField.Text);
            MySqlDataReader SceinceLeaderReader = SelectScienceLeader.ExecuteReader();
            while (SceinceLeaderReader.Read())
            {
                scienceLeader.SelectedItem = SceinceLeaderReader.GetValue(0).ToString();
            }
            SceinceLeaderReader.Close();
            
            dataBase.CloseConnection();
            
        }

        //удалить
        private void button3_Click(object sender, EventArgs e)
        {
            DataBase dataBase = new DataBase();
            dataBase.OpenConnection();

            MySqlCommand dropFromUserInfo = new MySqlCommand("delete from user_info where login_id=@login;", dataBase.GetConnection());
            MySqlCommand dropFromUserList = new MySqlCommand("delete from user_list where login=@login;", dataBase.GetConnection());
            MySqlCommand dropFromUserDep = new MySqlCommand("delete from user_dep where user_id=@login", dataBase.GetConnection());

            dropFromUserInfo.Parameters.AddWithValue("@login", searchField.Text);
            dropFromUserList.Parameters.AddWithValue("@login", searchField.Text);
            dropFromUserDep.Parameters.AddWithValue("@login", searchField.Text);

            dropFromUserList.ExecuteNonQuery();
            dropFromUserDep.ExecuteNonQuery();
            dropFromUserInfo.ExecuteNonQuery();

            dataBase.CloseConnection();

            EmptyFields();
            searchField.Text = "";

            MessageBox.Show("Пользователь удален");

        }

        //изменить
        private void button1_Click(object sender, EventArgs e)
        {
            String password = PasswordField.Text;
            String password1 = "";
            DataBase database = new DataBase();
            database.OpenConnection();
            MySqlCommand getPasswordFromDB = new MySqlCommand("select password from user_list where login=@ul", database.GetConnection());
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

            if (!password.Equals(password1))
            {
                MySqlCommand changePassword = new MySqlCommand
                    ("update user_list set password=@pass where login=@login;"
                    , database.GetConnection());
                changePassword.Parameters.AddWithValue("@pass", Convert.ToBase64String(signUp.sha512.ComputeHash(Encoding.UTF8.GetBytes(PasswordField.Text))));
                changePassword.Parameters.AddWithValue("@login", searchField.Text);
                changePassword.ExecuteNonQuery();
            }
            else { }
           

            MySqlCommand disable = new MySqlCommand("SET SQL_SAFE_UPDATES = 0;", database.GetConnection());
            disable.ExecuteNonQuery();


            MySqlCommand change3 = new MySqlCommand(
                @"update user_xp set leader_name=@ln where login=@login;",
                database.GetConnection());
            change3.Parameters.AddWithValue("@login", searchField.Text);
            change3.Parameters.AddWithValue("@ln", scienceLeader.SelectedItem);
            


            change3.ExecuteNonQuery(); 
            EmptyFields(); searchField.Text = "";
            MessageBox.Show("Изменено успешно");

        }

        private void CloseClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void CloseHover(object sender, EventArgs e)
        {
            CloseLabel.BackColor = System.Drawing.Color.Red;
        }

        private void CloseLeave(object sender, EventArgs e)
        {
            CloseLabel.BackColor = System.Drawing.Color.Transparent;

        }

        private void searchByName_TextChanged(object sender, EventArgs e)
        {
            if (searchByName.Text.Contains("%")) return;
            DataBase dataBase = new DataBase();
            dataBase.OpenConnection();
            MySqlCommand cmd = new MySqlCommand(
                @"select ud.user_id as Логин, ud.worker_type as Тип, ud.department_type as Департамент, tab.name as Имя, tab.surname as Фамилия, 
                tab.mail as Почта, tab.phone as Телефон, tab.birth_date as `Дата Рождения` from user_dep ud inner join 
                (select ui.login_id, ui.name, ui.surname, ui.mail, ui.phone, ui.birth_date 
                from user_list ul left join user_info ui on ((ul.login=ui.login_id)) where ui.name like @name) 
                as tab on ud.user_id = tab.login_id order by id;"
                , dataBase.GetConnection());
            //String[] name = searchByName.Text.Split(' ');
            // bool name_is_good = searchByName.Text.All => 
            if (!String.IsNullOrEmpty(searchByName.Text) && searchByName.Text.All(c => Char.IsLetter(c)) && searchByName.Text.Length > 1)
                cmd.Parameters.AddWithValue("@name", '%' + searchByName.Text + '%');
            else cmd.Parameters.AddWithValue("@name", searchByName.Text);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            DataTable dataTable1 = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            dataGridView1.DataSource = dataTable1;
            dataReader.Close();
            adapter.SelectCommand = cmd;
            adapter.Fill(dataTable1);
           
            if (dataGridView1.Rows.Count == 1) { dataGridView1.Visible = false; label8.Visible = true; label8.Text = "Ничего не найдено!"; }
            else { dataGridView1.Visible = true; label8.Text = "Найдено рузльтатов: "+ dataGridView1.Rows.Count; }

            dataBase.CloseConnection();
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

            if (this.WindowState.Equals(FormWindowState.Normal))
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }
        private void MinimizeHover(object sender, EventArgs e)
        {
            MinimizeLabel.BackColor = System.Drawing.Color.FromArgb(70, 63, 72, 204);
        }
        private void MinimizeLeave(object sender, EventArgs e)
        {
            MinimizeLabel.BackColor = System.Drawing.Color.Transparent;

        }

        private void войтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
                 new Auth().Show();

            

        }

        private void регистрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SignUp().Show();
        }

        private void change(object sender, EventArgs e)
        {
            
            DataBase dataBase = new DataBase();
            dataBase.OpenConnection();
            List<String> list = new List<String>(); 
            list.Clear();
            scienceLeader.Items.Clear();
            MySqlCommand selectForLeader = new MySqlCommand("select worker_type_officer from `worker_table_officer` where worker_type=@wt", dataBase.GetConnection());
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
        }
    }
}
