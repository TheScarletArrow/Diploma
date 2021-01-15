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
    public partial class Form1 : Form
    {
       public  SHA512 sha256 = SHA512.Create();

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {


     //       label2.Text =   Convert.ToBase64String( sha256.ComputeHash(Encoding.UTF8.GetBytes("TheMeAndYou")));
           // Clipboard.SetText(label2.Text);

                }

        private void button1_Click(object sender, EventArgs e)
        {

            if (Convert.ToBase64String
                (sha256.ComputeHash(Encoding.UTF8.GetBytes(textBox1.Text))).Equals(
                "TJE53a20/uiAZmvNEpLOgXuItvyRrO/c0RF67LHg/gO+vJkwDoLtfs+pl4BT737pesGA8O8x9m5+a7V13Cj2kw=="))
            {
                //label1.Text = "YEES";
            }
            else { }
              //  label1.Text = "#$%^&*";

            //label2.Text = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(textBox1.Text)));
            //label3.Text = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes("TheMeAndYou")));

            DataBase class1 = new DataBase();
            DataTable dataTable = new DataTable();
            
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            String password_encrypted = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(textBox2.Text)));
            String name_ = textBox1.Text;
            DateTime birthdate = DateTime.Parse(textBox3.Text);
            MySqlCommand mySqlCommand = new MySqlCommand("insert into user_list (name, password) VALUES (@ul, @up);", class1.GetConnection());
            MySqlCommand insertouserinfo = new MySqlCommand("insert into user_info (name,surname,birth_date) values (@un, 'Ada', @ud)", class1.GetConnection());
            insertouserinfo.Parameters.Add("@un", MySqlDbType.VarChar).Value = name_;
            insertouserinfo.Parameters.Add("@ud", MySqlDbType.DateTime).Value = birthdate; 
            mySqlCommand.Parameters.Add("@ul", MySqlDbType.VarChar).Value = name_;
            mySqlCommand.Parameters.Add("@up", MySqlDbType.VarChar).Value = password_encrypted;
            adapter.SelectCommand = mySqlCommand;
            adapter.SelectCommand = insertouserinfo;
            adapter.Fill(dataTable);
            MessageBox.Show(String.Format("Пользователь {0} был добавлен в базу данных", name_));

            //class1.OpenConnection();

//            if (mySqlCommand.ExecuteNonQuery() != -1) {             }
            /*
            if (dataTable.Rows.Count > 0)
                MessageBox.Show("Yes");
            else
                MessageBox.Show("No");*/
            //MySqlCommand truncate = new MySqlCommand("Truncate users;", class1.GetConnection());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataBase class1 = new DataBase();
            DataTable dataTable = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand truncate = new MySqlCommand("truncate users;", class1.GetConnection());
            adapter.SelectCommand = truncate;
            adapter.Fill(dataTable);
            MessageBox.Show("База данных очищена");
        }


    }

    public partial class CopyOfForm1 : Form
    {
        public SHA512 sha256 = SHA512.Create();

        public CopyOfForm1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {


            //       label2.Text =   Convert.ToBase64String( sha256.ComputeHash(Encoding.UTF8.GetBytes("TheMeAndYou")));
            // Clipboard.SetText(label2.Text);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (Convert.ToBase64String
                (sha256.ComputeHash(Encoding.UTF8.GetBytes(textBox1.Text))).Equals(
                "TJE53a20/uiAZmvNEpLOgXuItvyRrO/c0RF67LHg/gO+vJkwDoLtfs+pl4BT737pesGA8O8x9m5+a7V13Cj2kw=="))
            {
                //label1.Text = "YEES";
            }
            else { }
            //  label1.Text = "#$%^&*";

            //label2.Text = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(textBox1.Text)));
            //label3.Text = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes("TheMeAndYou")));

            DataBase class1 = new DataBase();
            DataTable dataTable = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            String password_encrypted = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(textBox2.Text)));
            String name_ = textBox1.Text;
            DateTime birthdate = DateTime.Parse(textBox3.Text);
            MySqlCommand mySqlCommand = new MySqlCommand("insert into user_list (name, password) VALUES (@ul, @up);", class1.GetConnection());
            MySqlCommand insertouserinfo = new MySqlCommand("insert into user_info (name,surname,birth_date) values (@un, 'Ada', @ud)", class1.GetConnection());
            insertouserinfo.Parameters.Add("@un", MySqlDbType.VarChar).Value = name_;
            insertouserinfo.Parameters.Add("@ud", MySqlDbType.DateTime).Value = birthdate;
            mySqlCommand.Parameters.Add("@ul", MySqlDbType.VarChar).Value = name_;
            mySqlCommand.Parameters.Add("@up", MySqlDbType.VarChar).Value = password_encrypted;
            adapter.SelectCommand = mySqlCommand;
            adapter.SelectCommand = insertouserinfo;
            adapter.Fill(dataTable);
            MessageBox.Show(String.Format("Пользователь {0} был добавлен в базу данных", name_));

            //class1.OpenConnection();

            //            if (mySqlCommand.ExecuteNonQuery() != -1) {             }
            /*
            if (dataTable.Rows.Count > 0)
                MessageBox.Show("Yes");
            else
                MessageBox.Show("No");*/
            //MySqlCommand truncate = new MySqlCommand("Truncate users;", class1.GetConnection());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataBase class1 = new DataBase();
            DataTable dataTable = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand truncate = new MySqlCommand("truncate users;", class1.GetConnection());
            adapter.SelectCommand = truncate;
            adapter.Fill(dataTable);
            MessageBox.Show("База данных очищена");
        }


    }

}
