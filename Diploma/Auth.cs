﻿using System;
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
        public SHA512 sha512 = SHA512.Create();

        public Auth()
        {
            InitializeComponent();

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(loginForm.Text) && !String.IsNullOrEmpty(passwordForm.Text)){
                if ((loginForm.Text.Equals("admin")) && (passwordForm.Text.Equals("admin")))
                {
                    new AdminControlPanel().Show();
                    this.Hide();
                }
                else
                {
                    //label2.Text = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(textBox1.Text)));
                    //label3.Text = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes("TheMeAndYou")));

                    DataBase database = new DataBase();
                    database.OpenConnection();
                    DataTable dataTable = new DataTable();
                    DataTable dataTable1 = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    MySqlDataAdapter adapter1 = new MySqlDataAdapter();
                    MySqlDataReader dataReader;

                    String name_ = loginForm.Text;
                    String Name = "";
                    MySqlCommand findPerson = new MySqlCommand("select * from user_list where login=@ul and password = @up;", database.GetConnection());
                    findPerson.Parameters.AddWithValue("@ul", name_);
                    findPerson.Parameters.AddWithValue("@up", Convert.ToBase64String
                             (sha512.ComputeHash(Encoding.UTF8.GetBytes(passwordForm.Text))));

                    MySqlCommand findPersonByLogin =
                        new MySqlCommand("select ui.name from user_info ui left join user_list ul on (ui.login=ul.login) where ui.login=@ul;",
                        database.GetConnection());

                    findPersonByLogin.Parameters.AddWithValue("@ul", name_);


                    adapter.SelectCommand = findPerson;
                    adapter.Fill(dataTable);

                    adapter1.SelectCommand = findPersonByLogin;
                    adapter1.Fill(dataTable1);

                    //fmTI57ZgCo u15
                    dataReader = findPersonByLogin.ExecuteReader();
                    while(  dataReader.Read())
                         if (!dataReader.IsDBNull(dataReader.GetOrdinal("name")))
                             try
                                 {
                                    Name = dataReader.GetString("name");
                                    }
                             catch (MySqlException ex) {
                                //MessageBox.Show("ошибка");
                                MessageBox.Show(ex.StackTrace);
                             }




                    if ((dataTable1.Rows.Count > 0) && (dataTable.Rows.Count > 0))
                        MessageBox.Show("Привет, " + Name);
                    else
                        MessageBox.Show("Неверный логин или пароль");
                }
            }
            else
                if (String.IsNullOrEmpty(loginForm.Text)) { MessageBox.Show("Логин пустой"); }
                else { MessageBox.Show("Пароль пустой"); }
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

        private void label4_Click(object sender, EventArgs e)
        {
            new SignUp().Show();
            this.Hide();
        }




    }
}
