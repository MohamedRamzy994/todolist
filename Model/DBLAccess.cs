using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;



namespace ToDoList.Model
{
    public class DBLAccess
    {
        string conn= Properties.Settings.Default.ToDoConn;
        public int ExecuteNonQuery(AddTaskModel addTaskModel)
        {
            int result;
            using (SqlConnection con = new  SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "stAddTask";
                cmd.Parameters.AddWithValue("@TaskTitle", addTaskModel.TaskTitle);
                cmd.Parameters.AddWithValue("@TaskStartDate", addTaskModel.TaskStartDate);
                cmd.Parameters.AddWithValue("@TaskEndDate", addTaskModel.TaskEndDate);
                cmd.Parameters.AddWithValue("@TaskNotes", addTaskModel.TaskNotes);
                result = cmd.ExecuteNonQuery();
                con.Close();

               




            }
            return result;
        }

        public SqlDataReader ExecuteReader(string procuder)
        {
            SqlDataReader objDataReader;
            SqlConnection con = new SqlConnection(conn);
          
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procuder;

                objDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return objDataReader;

           
     
        }
        public int DeleteExecuteNonQuery(int Id)
        {
            int result;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "stDeleteTask";
                cmd.Parameters.AddWithValue("@Id", Id);
            
                result = cmd.ExecuteNonQuery();
                con.Close();






            }
            return result;
        }

        public SqlDataReader ExecuteReaderSelectById(string procuder ,int Id)
        {
            SqlDataReader objDataReader;
            SqlConnection con = new SqlConnection(conn);

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procuder;
            cmd.Parameters.AddWithValue("@Id",Id) ;

            objDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            return objDataReader;



        }

        public int ExecuteNonQueryUpdate(AddTaskModel addTaskModel)
        {
            int result;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "stEditTaskById";
                cmd.Parameters.AddWithValue("@Id", addTaskModel.Id);
                cmd.Parameters.AddWithValue("@TaskTitle", addTaskModel.TaskTitle);
                cmd.Parameters.AddWithValue("@TaskStartDate", addTaskModel.TaskStartDate);
                cmd.Parameters.AddWithValue("@TaskEndDate", addTaskModel.TaskEndDate);
                cmd.Parameters.AddWithValue("@TaskNotes", addTaskModel.TaskNotes);
                result = cmd.ExecuteNonQuery();
                con.Close();






            }
            return result;
        }

    }
}
