using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace TaskCollabration.Models
{
    public class AddUserTaskModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter a Task Title")]

        public string Title { get; set; }
        [Required(ErrorMessage = "Please Enter a Task Description")]

        public string Description { get; set; }
        [Required(ErrorMessage = "Please Select a Status of your Task")]

        public string Status { get; set; }
        [Required(ErrorMessage = "Please Select a Priority of your Task")]

        public string Priority { get; set; }
        [Required(ErrorMessage = "Please enter a Due Date of your Task")]

        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Please Select a User")]
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string? FilePath { get; set; } 

        public string FirstName { get; set; }
        public List<string> SelectedUsers { get; set; } // Ensure this is a List

        public List<AddUserTaskModel> UsersList { get; set; } = new List<AddUserTaskModel>(); // Initialize by default


        //Insert A Record in a PersonalTask Table

        public bool insert(AddUserTaskModel model1)
        {
            SqlCommand cmd = new SqlCommand("insert into UsersTask values(@title, @description, @status, @priority, @date, @filePath, @firstname, @userid)", con);

            cmd.Parameters.AddWithValue("@title", model1.Title);
            cmd.Parameters.AddWithValue("@description", model1.Description);
            cmd.Parameters.AddWithValue("@status", model1.Status);
            cmd.Parameters.AddWithValue("@priority", model1.Priority);
            cmd.Parameters.AddWithValue("@date", model1.Date);
            cmd.Parameters.AddWithValue("@filePath", model1.FilePath);
            cmd.Parameters.AddWithValue("@firstname", model1.FirstName);
            cmd.Parameters.AddWithValue("@userid", model1.UserId);


            con.Open();
            int i = cmd.ExecuteNonQuery();
            if(i>=1)
            {
                return true;
            }
            return false;
        }

        //Select Data from a Users Table

        public List<AddUserTaskModel> getData()
        {
            List<AddUserTaskModel> lstadm = new List<AddUserTaskModel>();
            SqlDataAdapter da = new SqlDataAdapter("select * from Users", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                lstadm.Add(new AddUserTaskModel
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    FirstName = dr["FirstName"].ToString(),
                });
            }
            return lstadm;
        }

    }
}

