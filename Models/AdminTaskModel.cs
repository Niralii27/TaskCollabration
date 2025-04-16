using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace TaskCollabration.Models
{
    public class AdminTaskModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter a Title")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Please Select a Status")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Please Select a Priority")]
        public string? Priority { get; set; }

        public DateTime? Date { get; set; }

        public string? FilePath { get; set; }

        public List<AdminTaskModel> GetData()
        {
            List<AdminTaskModel> tasks = new List<AdminTaskModel>();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM PersonalTask", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tasks.Add(new AdminTaskModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    UserId = Convert.ToInt32(dr["UserId"]),
                    Title = dr["Title"].ToString(),
                    Description = dr["Description"].ToString(),
                    Status = dr["Status"].ToString(),
                    Priority = dr["Priority"].ToString(),
                    Date = dr["Date"] == DBNull.Value ? null : Convert.ToDateTime(dr["Date"]),
                    FilePath = dr["FilePath"].ToString()
                });
            }
            return tasks;
        }

        public AdminTaskModel GetData(int id)
        {
            AdminTaskModel task = new AdminTaskModel();
            SqlCommand cmd = new SqlCommand("SELECT * FROM PersonalTask WHERE Id = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    task.Id = Convert.ToInt32(dr["Id"]);
                    task.UserId = Convert.ToInt32(dr["UserId"]);
                    task.Title = dr["Title"].ToString();
                    task.Description = dr["Description"].ToString();
                    task.Status = dr["Status"].ToString();
                    task.Priority = dr["Priority"].ToString();
                    task.Date = dr["Date"] == DBNull.Value ? null : Convert.ToDateTime(dr["Date"]);
                    task.FilePath = dr["FilePath"].ToString();
                }
            }
            con.Close();
            return task;
        }

        public bool Insert(AdminTaskModel model)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO PersonalTask (UserId, Title, Description, Status, Priority, Date, FilePath) VALUES (@userid, @title, @description, @status, @priority, @date, @filepath)", con);
            cmd.Parameters.AddWithValue("@userid", model.UserId);
            cmd.Parameters.AddWithValue("@title", model.Title);
            cmd.Parameters.AddWithValue("@description", model.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", model.Status);
            cmd.Parameters.AddWithValue("@priority", model.Priority);
            cmd.Parameters.AddWithValue("@date", model.Date ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@filepath", model.FilePath ?? (object)DBNull.Value);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i >= 1;
        }

        public bool Update(AdminTaskModel model)
        {
            SqlCommand cmd = new SqlCommand("UPDATE PersonalTask SET UserId = @userid, Title = @title, Description = @description, Status = @status, Priority = @priority, Date = @date, FilePath = @filepath WHERE Id = @id", con);
            cmd.Parameters.AddWithValue("@userid", model.UserId);
            cmd.Parameters.AddWithValue("@title", model.Title);
            cmd.Parameters.AddWithValue("@description", model.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", model.Status);
            cmd.Parameters.AddWithValue("@priority", model.Priority);
            cmd.Parameters.AddWithValue("@date", model.Date ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@filepath", model.FilePath ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", model.Id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i >= 1;
        }

        public bool Delete(int id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM PersonalTask WHERE Id = @id", con);
            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i >= 1;
        }
    }
}
