using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;

using System.Data.SqlClient;
using System.Reflection;

namespace TaskCollabration.Models
{
    public class ProjectModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter a ProjectName")]

        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Please Enter a Category of Project")]

        public string Category { get; set; }
        [Required(ErrorMessage = "Please Enter a Due Date of project")]

        public DateTime DueDate { get; set; }
        [Required(ErrorMessage = "Please Enter a Member")]

        public string Members { get; set; }
        [Required(ErrorMessage = "Please Enter a Priority of Project")]

        public string Priority { get; set; }
        [Required(ErrorMessage = "Please Enter a Status of Project")]

        public string Status { get; set; }
        [Required(ErrorMessage = "Please Enter a Description")]

        public string Description { get; set; }

        public string FirstName { get; set; }

        public ProjectModel SelectedProject { get; set; }


        public List<ProjectModel> UsersList { get; set; } = new List<ProjectModel>(); // Initialize by default


        //Select Data From a Project Table
        public List<ProjectModel> getData2()
        {
            List<ProjectModel> lstadm1 = new List<ProjectModel>();
            SqlDataAdapter da = new SqlDataAdapter("select * from Projects", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstadm1.Add(new ProjectModel
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    ProjectName = dr["ProjectName"].ToString(),
                    Category = dr["Category"].ToString(),
                    DueDate = Convert.ToDateTime(dr["DueDate"].ToString()),
                    Status = dr["Status"].ToString(),
                    Priority = dr["Priority"].ToString(),
                    Members = dr["Members"].ToString(),
                    Description = dr["Description"].ToString(),
                });
            }
            return lstadm1;
        }

        //Retrieve single record from a table
        public ProjectModel getData(string Id)
        {
            ProjectModel admin = new ProjectModel();
            SqlCommand cmd = new SqlCommand("select * from Projects where id='" + Id +
            "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    admin.Id = Convert.ToInt32(dr["Id"].ToString());
                    admin.ProjectName = dr["ProjectName"].ToString();
                    admin.Category = dr["Category"].ToString();
                    admin.DueDate = Convert.ToDateTime(dr["DueDate"].ToString());
                    admin.Members = dr["Members"].ToString();
                    admin.Priority = dr["Priority"].ToString();
                    admin.Status = dr["Status"].ToString();
                    admin.Description = dr["Description"].ToString();
                }
            }
            con.Close();
            return admin;
        }

        //Update a Project Details
        public bool update(ProjectModel model)
        {
            SqlCommand cmd = new SqlCommand("update Projects set ProjectName = @projectname, DueDate = @duedate, Priority = @priority, Status = @status, Members = @members, Description = @description where Id = @id", con);

            cmd.Parameters.AddWithValue("@projectname", (object)model.ProjectName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@duedate", (object)model.DueDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@priority", (object)model.Priority ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@status", (object)model.Status ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@members", (object)model.Members ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@description", (object)model.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", model.Id);




            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }
            return false;

        }

        //Update Project Members
        public bool update1(ProjectModel model)
        {
            SqlCommand cmd = new SqlCommand("update Projects set Members = @members where Id = @id", con);

           
            cmd.Parameters.AddWithValue("@members", (object)model.Members ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", model.Id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }
            return false;

        }

        //Select Data from a Users Table

        public List<ProjectModel> getData1()
        {
            List<ProjectModel> lstadm = new List<ProjectModel>();
            SqlDataAdapter da = new SqlDataAdapter("select * from Users", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstadm.Add(new ProjectModel
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    FirstName = dr["FirstName"].ToString(),
                });
            }
            return lstadm;
        }
    }
}
