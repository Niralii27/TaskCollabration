using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace TaskCollabration.Models
{
    public class AdminProjectModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter a Project Name")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Please Enter a Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Please Enter a Due Date")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Please Enter Members")]
        public string Members { get; set; }

        // Retrieve all projects
        public List<AdminProjectModel> getData()
        {
            List<AdminProjectModel> projects = new List<AdminProjectModel>();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Projects", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                projects.Add(new AdminProjectModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    ProjectName = dr["ProjectName"].ToString(),
                    Category = dr["Category"].ToString(),
                    DueDate = Convert.ToDateTime(dr["DueDate"]),
                    Members = dr["Members"].ToString()
                });
            }
            return projects;
        }

        // Retrieve a single project by ID
        public AdminProjectModel getData(string Id)
        {
            AdminProjectModel project = new AdminProjectModel();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Projects WHERE Id = @id", con);
            cmd.Parameters.AddWithValue("@id", Id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    project.Id = Convert.ToInt32(dr["Id"]);
                    project.ProjectName = dr["ProjectName"].ToString();
                    project.Category = dr["Category"].ToString();
                    project.DueDate = Convert.ToDateTime(dr["DueDate"]);
                    project.Members = dr["Members"].ToString();
                }
            }
            con.Close();
            return project;
        }

        // Insert a new project
        public bool insert(AdminProjectModel model)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Projects (ProjectName, Category, DueDate, Members) VALUES (@projectname, @category, @duedate, @members)", con);
            cmd.Parameters.AddWithValue("@projectname", model.ProjectName);
            cmd.Parameters.AddWithValue("@category", model.Category);
            cmd.Parameters.AddWithValue("@duedate", model.DueDate);
            cmd.Parameters.AddWithValue("@members", model.Members);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i >= 1;
        }

        // Update an existing project
        public bool update(AdminProjectModel project)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Projects SET ProjectName = @projectname, Category = @category, DueDate = @duedate, Members = @members WHERE Id = @id", con);
                cmd.Parameters.AddWithValue("@projectname", project.ProjectName);
                cmd.Parameters.AddWithValue("@category", project.Category);
                cmd.Parameters.AddWithValue("@duedate", project.DueDate);
                cmd.Parameters.AddWithValue("@members", project.Members);
                cmd.Parameters.AddWithValue("@id", project.Id);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (i >= 1)
                {
                    Console.WriteLine($"✅ Project {project.Id} updated successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"❌ Project {project.Id} update failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Update Error: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return false;
        }


        // Delete a project
        public bool delete(AdminProjectModel project)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Projects WHERE Id = @id", con);
            cmd.Parameters.AddWithValue("@id", project.Id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i >= 1;
        }
    }
}
