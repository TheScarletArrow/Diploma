using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Diploma
{
    public partial class Auth : Form
    {
        readonly SignUp sign = new SignUp();
        public Auth()
        {
            InitializeComponent();
                        
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(loginForm.Text) && !String.IsNullOrEmpty(passwordForm.Text))
                {
                    if ((loginForm.Text.Equals("admin")) && (passwordForm.Text.Equals("admin")))
                    {
                        
                        new AdminControlPanel().Show();
                        Hide();
                    }
                    else
                    {

                        DataBase database = new DataBase();
                        database.OpenConnection();
                        DataTable dataTable = new DataTable();
                        DataTable dataTable1 = new DataTable();
                        MySqlDataAdapter adapter = new MySqlDataAdapter();
                        MySqlDataAdapter adapter1 = new MySqlDataAdapter();
                        MySqlDataReader dataReader;

                        String name_ = loginForm.Text;
                        String Name = "";

                        MySqlCommand findPerson =
                            new MySqlCommand("select * from user_list where login=@ul and password = @up;",
                                database.GetConnection());
                        findPerson.Parameters.AddWithValue("@ul", name_);
                        String enc_pass = Convert.ToBase64String
                            (sign.sha512.ComputeHash(Encoding.UTF8.GetBytes(passwordForm.Text)));
                        findPerson.Parameters.AddWithValue("@up", enc_pass);

                        MySqlCommand findPersonByLogin =
                            new MySqlCommand(
                                "select ui.name from user_info ui left join user_list ul on (ui.login_id=ul.login) where ui.login_id=@ul;",
                                database.GetConnection());

                        findPersonByLogin.Parameters.AddWithValue("@ul", name_);


                        adapter.SelectCommand = findPerson;
                        adapter1.SelectCommand = findPersonByLogin;

                        adapter.Fill(dataTable);
                        adapter1.Fill(dataTable1);

                        dataReader = findPersonByLogin.ExecuteReader();
                        while (dataReader.Read())
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("name")))
                            {
                                try
                                {
                                    Name = dataReader.GetString("name");
                                }
                                catch (MySqlException ex)
                                {
                                    //MessageBox.Show("ошибка");
                                    MessageBox.Show(ex.StackTrace);
                                }
                            }



                        if ((dataTable1.Rows.Count > 0) && (dataTable.Rows.Count > 0))
                            MessageBox.Show(@"Привет, " + Name);
                        else
                            MessageBox.Show(@"Неверный логин или пароль");
                        database.CloseConnection();



                    }
                }
                else if (String.IsNullOrEmpty(loginForm.Text))
                {
                    MessageBox.Show(@"Логин пустой");
                }
                else
                {
                    MessageBox.Show(@"Пароль пустой");
                }
            }
            
            catch (Exception ex)
            {
                using (FileStream fstream =
                    new FileStream(
                        $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log",
                        FileMode.Append))
                {

                    byte[] array = Encoding.Default.GetBytes(ex.StackTrace);
                    // асинхронная запись массива байтов в файл
                    await fstream.WriteAsync(array, 0, array.Length);
                    await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                }            
            }
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
            //Application.Exit();
        }

        Point lastPoint;
        private void formMove(object sender, MouseEventArgs e)
        {
            int dx = e.X - lastPoint.X;
            int dy = e.Y - lastPoint.Y;
            if (e.Button.Equals(MouseButtons.Left))
            {
                Left += dx;
                Top += dy;
            }
        }

        private void formDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            new SignUp().Show();
            Hide();
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

    }
}
