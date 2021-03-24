using System;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Diploma
{
    class DataBase
    {
       private readonly MySqlConnection _mySqlConnection = 
            new MySqlConnection("username=root;password=RaPtorJEDI1;database=userdb");

        

        public void OpenConnection()
        {
            try
            {
                if (_mySqlConnection.State == System.Data.ConnectionState.Closed)
                    _mySqlConnection.Open();
               
            }
            catch (MySqlException exception)
            {
                MessageBox.Show(@"Невозможно открыть связь с базой данных" + exception.StackTrace);

            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show(@"Невозможно открыть связь с базой данных" + exception.StackTrace);

            }
        }
        public void CloseConnection()
        {
            if (_mySqlConnection.State == System.Data.ConnectionState.Open)
                _mySqlConnection.Close();
        }

        public MySqlConnection GetConnection() {
            return _mySqlConnection;
        }

       

    }
    
}
