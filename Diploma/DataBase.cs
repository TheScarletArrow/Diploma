﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    class DataBase
    {
        
       
        MySqlConnection mySQLConnection = new MySqlConnection("username=root;password=RaPtorJEDI1;database=userdb"); 
    
     //    MySqlConnection mySQLConnection = new MySqlConnection("username=u1163679_scarlet;password=RaPtorJEDI1;database=u1163679_userdb");

        public void OpenConnection()
        {
           
            if (mySQLConnection.State == System.Data.ConnectionState.Closed)
                mySQLConnection.Open();
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
