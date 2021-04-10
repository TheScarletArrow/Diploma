using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;

namespace Diploma
{
    internal class DataBase
    {
       private readonly MySqlConnection _mySqlConnection = 
            new MySqlConnection("username=root;password=RaPtorJEDI1;database=userdb");
        
        public void OpenConnection()
        {
            try
            {
                if (_mySqlConnection.State == ConnectionState.Closed)
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
            if (_mySqlConnection.State == ConnectionState.Open)
                _mySqlConnection.Close();
        }

        public MySqlConnection GetConnection() {
            return _mySqlConnection;
        }

        public bool Ping()
        {
            OpenConnection();
            var ping = _mySqlConnection.Ping();
            CloseConnection();
            return ping;
        }

        public string ShowVersion()
        {
            return _mySqlConnection.ServerVersion;
        }
    }
    
}
