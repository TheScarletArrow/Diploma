using System;
using System.Data;
using System.Windows.Forms;
using Diploma.Entities;
using MySql.Data.MySqlClient;

namespace Diploma
{
    public partial class ShowTables : Form
    {
        public ShowTables()
        {
            InitializeComponent();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataBase dataBase = new DataBase();
            dataBase.OpenConnection();
            MySqlCommand mySqlCommand = null;

            if (radioButton1.Checked)
            {
                
                    try
                    {
                        mySqlCommand = new MySqlCommand("select * from user_list;", dataBase.GetConnection());
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                
            }
            if (radioButton2.Checked)
            {
                
                try
                {
                    mySqlCommand = new MySqlCommand("select * from user_info;", dataBase.GetConnection());
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                
            }
            if (radioButton3.Checked)
            {
                
                try
                {
                    mySqlCommand = new MySqlCommand("select * from user_xp;", dataBase.GetConnection());
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                
            }
            if (radioButton4.Checked)
            {
                
                try
                {
                    mySqlCommand = new MySqlCommand("select * from worker_table_officer;", dataBase.GetConnection());
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message)
                        ;
                }
                
            }if (radioButton5.Checked)
            {
                
                try
                {
                    mySqlCommand = new MySqlCommand("select * from engineer_head_officer;", dataBase.GetConnection());
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message)
                        ;
                }
                
            }
            if (radioButton6.Checked)
            {
                
                try
                {
                    mySqlCommand = new MySqlCommand("select * from user_dep;", dataBase.GetConnection());
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message)
                        ;
                }
                
            }
            MySqlDataReader dataReader = null;
            try
            {
                if (mySqlCommand != null) dataReader = mySqlCommand.ExecuteReader();
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

            dataGridView1.DataSource = dataTable1;
            dataReader.Close();
            dataBase.OpenConnection();
            adapter.SelectCommand = mySqlCommand;
            adapter.Fill(dataTable1);
            dataGridView1.Visible = dataGridView1.Rows.Count > 1;
            
            //label16.Visible = true;
            //label16.Text = @"Найдено " + (dataGridView2.Rows.Count - 1).ToString()
              //                         + @" результат(ов), соответствующих заданным параметрам";
        }

        private void ShowTables_Load(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentCell.ContextMenuStrip = contextMenuStrip1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dataBase = new DataBase();
            dataBase.OpenConnection();
            try
            {

                if (radioButton2.Checked)
                {
                    //
                    try
                    {
                        var dataGridViewRow = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex];
                        var mySqlCommand = new MySqlCommand("update user_info set name=@name,surname=@surname,mail=@mail,phone=@phone,birth_date=@birthdate where login_id=@login;", dataBase.GetConnection());
                        mySqlCommand.Parameters.AddWithValue("@name", dataGridViewRow.Cells[1].Value);
                        mySqlCommand.Parameters.AddWithValue("@surname", dataGridViewRow.Cells[2].Value);
                        mySqlCommand.Parameters.AddWithValue("@mail", dataGridViewRow.Cells[3].Value);
                        mySqlCommand.Parameters.AddWithValue("@phone", dataGridViewRow.Cells[4].Value);
                        mySqlCommand.Parameters.AddWithValue("@birthdate", dataGridViewRow.Cells[5].Value);

                        mySqlCommand.Parameters.AddWithValue("@login", dataGridViewRow.Cells[6].Value);


                        mySqlCommand.ExecuteNonQuery();
                        MessageBox.Show(@"Данные были успешно обновлены");

                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }

                }
                else 
                        MessageBox.Show(@"Обновление для данной таблицы невозможно в данном окне");
                    
                                

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }
    }
}