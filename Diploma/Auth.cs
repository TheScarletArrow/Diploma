using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace Diploma
{
    public partial class Auth : Form
    {
       public  SHA512 sha512 = SHA512.Create();

        public Auth()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (Convert.ToBase64String
                (sha512.ComputeHash(Encoding.UTF8.GetBytes(loginForm.Text))).Equals(
                "TJE53a20/uiAZmvNEpLOgXuItvyRrO/c0RF67LHg/gO+vJkwDoLtfs+pl4BT737pesGA8O8x9m5+a7V13Cj2kw=="))
            {
                //label1.Text = "YEES";
            }
            else { }
              //  label1.Text = "#$%^&*";

            //label2.Text = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(textBox1.Text)));
            //label3.Text = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes("TheMeAndYou")));

            DataBase database = new DataBase();
            DataTable dataTable = new DataTable();
            
            MySqlDataAdapter adapter = new MySqlDataAdapter();
           // String password_encrypted = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(textBox2.Text)));
            String name_ = loginForm.Text;

            MySqlCommand findPerson = new MySqlCommand("select * from user_list where login=@ul and password = @up;", database.GetConnection());
            findPerson.Parameters.AddWithValue("@ul", name_);
            findPerson.Parameters.AddWithValue("@up", Convert.ToBase64String
                     (sha512.ComputeHash(Encoding.UTF8.GetBytes(passwordForm.Text))));
            //  MySqlCommand mySqlCommand = new MySqlCommand("insert into user_list (name, password) VALUES (@ul, @up);", class1.GetConnection());
            //  MySqlCommand insertouserinfo = new MySqlCommand("insert into user_info (name,surname,birth_date) values (@un, 'Ada', @ud)", class1.GetConnection());
            //   insertouserinfo.Parameters.Add("@un", MySqlDbType.VarChar).Value = name_;
            //   insertouserinfo.Parameters.Add("@ud", MySqlDbType.DateTime).Value = birthdate; 
            //   mySqlCommand.Parameters.Add("@ul", MySqlDbType.VarChar).Value = name_;
            //   mySqlCommand.Parameters.Add("@up", MySqlDbType.VarChar).Value = password_encrypted;
            //   adapter.SelectCommand = mySqlCommand;
            //   adapter.SelectCommand = insertouserinfo;
            adapter.SelectCommand = findPerson;
            adapter.Fill(dataTable);
            MessageBox.Show(String.Format("Пользователь {0} привет", name_));

            //class1.OpenConnection();

//            if (mySqlCommand.ExecuteNonQuery() != -1) {             }
            /*
            if (dataTable.Rows.Count > 0)
                MessageBox.Show("Yes");
            else
                MessageBox.Show("No");*/
            //MySqlCommand truncate = new MySqlCommand("Truncate users;", class1.GetConnection());

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
    }

    

}
