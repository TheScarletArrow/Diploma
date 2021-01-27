using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diploma
{
    class DataBase
    {
        
       
        MySqlConnection mySQLConnection = new MySqlConnection("username=root;password=RaPtorJEDI1;database=userdb");

        

        public void OpenConnection()
        {
            try
            {
                if (mySQLConnection.State == System.Data.ConnectionState.Closed)
                    mySQLConnection.Open();
            }
            catch (MySqlException exception){
                MessageBox.Show("Невозможно открыть связь с базой данных\n" + exception.StackTrace);

            }
        }
        public void CloseConnection()
        {
            if (mySQLConnection.State == System.Data.ConnectionState.Open)
                mySQLConnection.Close();
        }

        public MySqlConnection GetConnection() {
            return mySQLConnection;
        }

       

    }
    
}
