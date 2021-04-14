using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Diploma.Entities;
using MySql.Data.MySqlClient;

namespace Diploma
{
    public partial class Auth : Form
    {
        readonly SignUp _sign = new SignUp();
        public Auth()
        {
            InitializeComponent();
                        
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(loginForm.Text) && !string.IsNullOrEmpty(passwordForm.Text))
                {
                    if (loginForm.Text.Equals("admin") && passwordForm.Text.Equals("admin"))
                    {
                        var fc = Application.OpenForms["AdminControlPanel"];
                        if (fc != null)
                        {
                            Hide();
                        }
                        else
                        {
                            new AdminControlPanel().Show();
                            Hide();
                        }
                        //  fc?.Hide();

                        
                    }
                    else
                    {

                        var database = new DataBase();
                        database.OpenConnection();
                        var dataTable = new DataTable();
                        var dataTable1 = new DataTable();
                        var adapter = new MySqlDataAdapter();
                        var adapter1 = new MySqlDataAdapter();

                        var nameAsParam = loginForm.Text;
                        var nameFromDb = "";

                        var findPerson =
                            new MySqlCommand("select * from user_list where login=@ul and password = @up;",
                                database.GetConnection());
                        findPerson.Parameters.AddWithValue("@ul", nameAsParam);
                        var encPass = Convert.ToBase64String
                            (_sign.Sha512.ComputeHash(Encoding.UTF8.GetBytes(passwordForm.Text)));
                        findPerson.Parameters.AddWithValue("@up", encPass);

                        var findPersonByLogin =
                            new MySqlCommand(
                                "select ui.name from user_info ui left join user_list ul on (ui.login_id=ul.login) where ui.login_id=@ul;",
                                database.GetConnection());

                        findPersonByLogin.Parameters.AddWithValue("@ul", nameAsParam);


                        findPerson.ExecuteNonQuery();
                        
                        adapter.SelectCommand = findPerson;
                        adapter.Fill(dataTable);

                        
                        adapter1.SelectCommand = findPersonByLogin;

                        adapter1.Fill(dataTable1);

                        var dataReader = findPersonByLogin.ExecuteReader();
                        while (dataReader.Read())
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("name")))
                            {
                                try
                                {
                                    nameFromDb = dataReader.GetString("name");
                                }
                                catch (MySqlException ex)
                                {
                                    //MessageBox.Show("ошибка");
                                    MessageBox.Show(ex.StackTrace);
                                }
                            }



                        if ((dataTable1.Rows.Count > 0) && (dataTable.Rows.Count > 0))
                            MessageBox.Show(@"Привет, " + nameFromDb);
                        else
                            MessageBox.Show(@"Неверный логин или пароль");
                        database.CloseConnection();



                    }
                }
                else if (string.IsNullOrEmpty(loginForm.Text))
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
                using (var fstream =
                    new FileStream(
                        $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/error{DateTime.Now.ToShortDateString()}.log",
                        FileMode.Append))
                {

                    var array = Encoding.Default.GetBytes(ex.StackTrace);
                    // асинхронная запись массива байтов в файл
                    await fstream.WriteAsync(array, 0, array.Length);
                    await fstream.WriteAsync(new byte[] {13, 10}, 0, 2);

                }            
            }
        }



        private void CloseHover(object sender, EventArgs e)
        {
            CloseLabel.BackColor = Color.FromArgb(70, Color.Red);
        }

        private void CloseLeave(object sender, EventArgs e)
        {
            CloseLabel.BackColor = Color.Transparent;

        }

        private void CloseClick(object sender, EventArgs e)
        {
            Close();
            //Application.Exit();
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
