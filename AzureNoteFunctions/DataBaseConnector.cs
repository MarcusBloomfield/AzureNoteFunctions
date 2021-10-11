using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace AzureNoteFunctions
{
    public class DataBaseConnector
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        ILogger log;

        public DataBaseConnector(ILogger log)
        {
            this.log = log;
            Initialize();
        }

        private void Initialize()
        {
            server = "notesalandatabase.mysql.database.azure.com";
            database = "notesalandatabase";
            uid = "MarcusBloomfield@notesalandatabase";
            password = "Chucky4cheese";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "respect binary flags=false;";

            connection = new MySqlConnection(connectionString);
        }
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                log.LogInformation("It worked!");
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        log.LogInformation("Cannot connect to server.Contact administrator");
                        break;
                    case 1045:
                        log.LogInformation("Invalid username/password, please try again");
                        break;
                }
                log.LogInformation(ex.Message + "Broke asf sonny");
                return false;
            }
        }
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public string TryLogin(string userName, string userPassword)
        {
            try
            {
                log.LogInformation("login1");
                string query = "SELECT * FROM userlogins WHERE BINARY userName='" + userName.Replace("'", "''") + "' AND BINARY Password='" + userPassword.Replace("'", "''") + "'";
                User user = new User();
                if (OpenConnection())
                {
                    log.LogInformation("login2");
                    MySqlCommand commmand = new MySqlCommand(query, connection);

                    MySqlDataReader dataReader = commmand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        log.LogInformation("READING DATA");
                        user.username = dataReader["userName"] + "";
                        user.password = dataReader["passWord"] + "";
                        user.notes = dataReader["notes"] + "";
                    }
                    log.LogInformation("login3");
                    dataReader.Close();
                    CloseConnection();
                    return user.notes;
                }
                else return string.Empty;
            }
            catch (MySqlException ex)
            {
                log.LogInformation("TRY LOGIN EXECPTION" + ex.Message);
                return string.Empty;
            }
        }
        public bool AddUsersNotes(string notes, string userName, string password)
        {
            log.LogInformation($"user info name {userName} password { password} ");
            log.LogInformation($"user notes {notes}  ");
            string query = "UPDATE userlogins SET notes='" + notes.Replace("'", "''") + "' WHERE username='" + userName.Replace("'", "''") + "' AND Password='" + password.Replace("'", "''") + "'";
            if (OpenConnection())
            {
                MySqlCommand commmand = new MySqlCommand(query, connection);
                commmand.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            connection.Close();
            return false;
        }
        public bool TryCreateAccount(string userName, string userPassword)
        {
            log.LogInformation("creatAccount");
            if (string.IsNullOrEmpty(TryLogin(userName, userPassword)))
            {
                log.LogInformation("creatAccount2");
                string query = "INSERT INTO userlogins (username, password) VALUES('" + userName.Replace("'", "''") + "','" + userPassword.Replace("'", "''") + "')";
                if (OpenConnection() == true)
                {
                    log.LogInformation("creatAccount3");
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    CloseConnection();
                    return true;
                }
            }
            return false;
        }
    }
}
