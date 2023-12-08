using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Hard_Chatik;
using System.Collections.ObjectModel;

namespace Hard_Chatik_WPF
{
    internal class DbAccess
    {

        public void CheckAccess()
        {
            string connectionString = "Data Source=83.246.225.68;Initial Catalog=Hard Chatik Data Base;User ID=HOME;Password=123";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                MessageBox.Show("Успешно подключились");
                connection.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось подключиться");
            }

        }


        public void UserReg(string username, string password)
        {
            string connectionString = "Data Source=83.246.225.68;Initial Catalog=Hard Chatik Data Base;User ID=HOME;Password=123";
            SqlConnection connection = new SqlConnection(connectionString);
            string insertQuery = "INSERT INTO [User] (UserName, Password, HardWare) VALUES (@Value1, @Value2, @Value3)";
            SqlCommand sqlCommand = new SqlCommand(insertQuery, connection);


            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось подключиться");
            }

            sqlCommand.Parameters.AddWithValue("@Value1", username);
            sqlCommand.Parameters.AddWithValue("@Value2", Cryptographer.Encrypt(password));

            HardWare hardWare = new HardWare();
            
            sqlCommand.Parameters.AddWithValue("@Value3", hardWare.GetMotherBoardId());
            try
            {
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Регистрация успешна, теперь можешь залогиниться");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка регистрации: " + ex);
            }
            connection.Close();
        }

        public bool UserAuth(string username, string password, out int userID)
        {
            string connectionString = "Data Source=83.246.225.68;Initial Catalog=Hard Chatik Data Base;User ID=HOME;Password=123";
            SqlConnection connection = new SqlConnection(connectionString);
            userID = -1;
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось подключиться");
            }

            string query = "SELECT ID, Password, HardWare FROM [User] WHERE Username=@Username";
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Username", username);
            HardWare hardWare = new HardWare();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            
            if(reader.Read())
            {
                userID = int.Parse(reader["ID"].ToString());
                if (reader["Password"].ToString().ToLower() == Cryptographer.Encrypt(password).ToLower())
                {
                    {
                        if (reader["HardWare"].ToString() == hardWare.GetMotherBoardId())
                        {
                            MessageBox.Show("Авторизация успешна");
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Аккаунт зарегистрирован на другое железо");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Неверный пароль");
                }
            }
            else
            {
                MessageBox.Show("Пользователь с таким именем не найден");
            }
            return false;
        }

        public ObservableCollection<ChatMessage> LoadChatMessages()
        {
            ObservableCollection <ChatMessage> messages = new ObservableCollection<ChatMessage>();
            string connectionString = "Data Source=83.246.225.68;Initial Catalog=Hard Chatik Data Base;User ID=HOME;Password=123";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                {
                    connection.Open();
                    string query = "SELECT UserName, Text FROM Chat LEFT JOIN [User] ON [UserID] = [ID]";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ChatMessage message = new ChatMessage
                        {
                            UserID = reader["UserName"].ToString(),
                            Text = reader["Text"].ToString()
                        };
                        messages.Add(message);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            return messages;
        }

        public void SendMessage(int userID, string text)
        {
            string connectionString = "Data Source=83.246.225.68;Initial Catalog=Hard Chatik Data Base;User ID=HOME;Password=123";
            SqlConnection connection = new SqlConnection(connectionString);
            string insertQuery = "INSERT INTO [Chat] (UserID, Text, TimeStamp) VALUES (@Value1, @Value2, @Value3)";
            SqlCommand sqlCommand = new SqlCommand(insertQuery, connection);


            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось подключиться");
            }

            sqlCommand.Parameters.AddWithValue("@Value1", userID);
            sqlCommand.Parameters.AddWithValue("@Value2", text);
            sqlCommand.Parameters.AddWithValue("@Value3", DateTime.Now);
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка регистрации: " + ex);
            }
            connection.Close();
        }
    }
}
