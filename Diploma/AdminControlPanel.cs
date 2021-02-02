using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Diploma
{
    public partial class AdminControlPanel : Form
    {
        SignUp signUp = new SignUp();
        public AdminControlPanel()
        {
            InitializeComponent();
        }
       
        private void EmptyFields() {
            NameField.Text = "";
            SurnameField.Text = "";
            PasswordField.Text = "";
            MailField.Text = "";
            PhoneField.Text = "";
            DateField.Text = "";
            KnowledgeComboBox.SelectedItem = null;
            workingXPComboBox.SelectedItem = null;
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            DataBase dataBase =new DataBase();
            dataBase.OpenConnection();
            MySqlCommand cmd = new MySqlCommand(
                "select ui.login, ui.name, ui.surname, ul.password, ui.mail, ui.phone, ui.birth_date from user_list ul left join user_info ui on ((ul.login=ui.login)) where ui.login=@id;"
                , dataBase.GetConnection());
            cmd.Parameters.AddWithValue("@id", searchField.Text);

            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read()) {
                NameField.Text = dataReader.GetValue(1).ToString();
                SurnameField.Text = dataReader.GetValue(2).ToString();
                PasswordField.Text = dataReader.GetValue(3).ToString();
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

            dataBase.CloseConnection();
            
        }

        //удалить
        private void button3_Click(object sender, EventArgs e)
        {
            DataBase dataBase = new DataBase();
            dataBase.OpenConnection();

            MySqlCommand dropFromUserInfo = new MySqlCommand("delete from user_info where login=@login;", dataBase.GetConnection());
            MySqlCommand dropFromUserList = new MySqlCommand("delete from user_list where login=@login;", dataBase.GetConnection());

            dropFromUserInfo.Parameters.AddWithValue("@login", searchField.Text);
            dropFromUserList.Parameters.AddWithValue("@login", searchField.Text);

            dropFromUserInfo.ExecuteNonQuery();
            dropFromUserList.ExecuteNonQuery();

            dataBase.CloseConnection();

            EmptyFields();
            
            MessageBox.Show("Пользователь удален");

        }

        //изменить
        private void button1_Click(object sender, EventArgs e)
        {
            String password = PasswordField.Text;
            DataBase database = new DataBase();
            database.OpenConnection();

            MySqlCommand change = new MySqlCommand
                ("update user_info set name=@name,surname=@surname,mail=@mail,phone=@phone,birth_date=@birthdate where login=@login;"
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

             
            MySqlCommand changePassword = new MySqlCommand
                ("update user_list set password=@pass where login=@login;"
                , database.GetConnection());
            changePassword.Parameters.AddWithValue("@pass", Convert.ToBase64String(signUp.sha512.ComputeHash(Encoding.UTF8.GetBytes(PasswordField.Text))));
            changePassword.Parameters.AddWithValue("@login", searchField.Text);
            changePassword.ExecuteNonQuery(); 
            
            EmptyFields();
            MessageBox.Show("Изменено успешно");

        }
    }
}
