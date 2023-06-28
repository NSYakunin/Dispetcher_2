using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Dispetcher2
{
    public class WorkWithDatabase
    {
        //Конструктор, принимает строку соединения и создает подключение к БД
        public WorkWithDatabase(string connectString)
        {
            this.connectString = connectString;
        }

        //Обычный конструктор
        public WorkWithDatabase()
        {

        }

        private static WorkWithDatabase _instance = null;

        public static WorkWithDatabase Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WorkWithDatabase();
                return _instance;
            }
        }

        SqlConnection connect; //Соединение с БД
        public SqlConnection Connect { get { return connect; } }

        readonly string connectString; //Строка подключения к БД
        public string ConnectString { get { return connectString; } }


        //Заполнение данными таблицы по запросу sql (запрос select)
        public DataTable ExecuteQueryInTable(string sql)
        {
            connect = new SqlConnection(connectString);
            using (connect)
            {
                using (var cmd = new SqlCommand() { CommandTimeout = 7, /**/Connection = connect, /*соединение*/CommandType = CommandType.Text, CommandText = sql }) //создаем команду для запроса
                //                var cmd = new SqlCommand() { CommandTimeout = 7, /**/Connection = connect, /*соединение*/CommandType = CommandType.Text, CommandText = sql };
                {
                    //cmd.Connection = connect; //соединение
                    //cmd.CommandType = CommandType.Text;
                    //cmd.CommandText = sql; //сам текст запроса
                    try
                    {
                        connect.Open();
                        using (var adapter = new SqlDataAdapter(cmd)) //создаем адаптер на выполнение команды
                        {
                            using (var table = new DataTable()) //создаем таблицу для данных
                            {
                                adapter.Fill(table); //заполнение таблицы
                                connect.Close();
                                return table;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
            }
        }

        //Обычное выполнение запроса (insert/delete/update)
        public void ExecuteQuery(string sql)
        {
            connect = new SqlConnection(connectString);
            using (connect)
            {
                //создаем команду для запроса
                using (var cmd = new SqlCommand() { CommandTimeout = 3, /**/Connection = connect, /*соединение*/CommandType = CommandType.Text, CommandText = sql })
                {
                    //сам текст запроса
                    connect.Open();
                    cmd.ExecuteNonQuery();
                    connect.Close();
                }
            }
        }

        public int ExecuteScalar(string query)
        {

            connect = new SqlConnection(connectString);
            using (connect)
            {
                using (var cmd = new SqlCommand() { CommandTimeout = 3, /**/Connection = connect, /*соединение*/CommandType = CommandType.Text, CommandText = query + " SELECT SCOPE_IDENTITY()" })
                {
                    connect.Open();
                    var result = cmd.ExecuteScalar();
                    int modified = (int)(decimal)result;
                    connect.Close();
                    return modified;
                }
            }

        }
    }
}
