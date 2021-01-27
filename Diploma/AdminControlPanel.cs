using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Diploma
{
    public partial class AdminControlPanel : Form
    {
        public AdminControlPanel()
        {
            InitializeComponent();
        }
        private void AdminControlPanel_Load(object sender, EventArgs e)
        {
      
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
            dataBase.CloseConnection();
        }

        //удалить
        private void button3_Click(object sender, EventArgs e)
        {
            DataBase dataBase = new DataBase();
        }

        //изменить
        private void button1_Click(object sender, EventArgs e)
        {
            DataBase database = new DataBase();
            database.OpenConnection();

            MySqlCommand change = new MySqlCommand
                ("update user_info set name=@name,surname=@surname,mail=@mail,phone=@phone,birth_date=@birthdate where login=@login;"
                , database.GetConnection());

            change.Parameters.AddWithValue("@name", NameField.Text);
            change.Parameters.AddWithValue("@surname", SurnameField.Text);
            change.Parameters.AddWithValue("@mail", MailField.Text);
            change.Parameters.AddWithValue("@phone", PhoneField.Text);
            change.Parameters.AddWithValue("@birthdate", DateTime.Parse(  DateField.Text));
            change.Parameters.AddWithValue("@login", searchField.Text);
            change.ExecuteNonQuery();

            database.CloseConnection();
            MessageBox.Show("Изменено успешно");

        }
    }
}
