using System.ComponentModel.DataAnnotations;
using System.Data;

using System.Data.SqlClient;

namespace TaskCollabration.Models
{
    public class UserProjectModel
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
        public string Title { get; set; }

        public DateTime Date { get; set; }
         public string FilePath { get; set; }
        public int UserID { get; set; }
        
        public List<UserProjectModel> UsersList { get; set; } = new List<UserProjectModel>(); // Initialize by default
        public List<UserProjectModel> UserTasks { get; set; } = new List<UserProjectModel>();
        public List<UserProjectModel> UserProjects { get; set; } = new List<UserProjectModel>();


        //Select Data From a Project Table
        public List<UserProjectModel> getData2(int userId)
        {
            List<UserProjectModel> lstadm1 = new List<UserProjectModel>();

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Projects WHERE Members LIKE @UserID1 OR Members LIKE @UserID2 OR Members LIKE @UserID3 OR Members = @UserID4", con))
            {
                cmd.Parameters.AddWithValue("@UserID1", userId + ",%");
                cmd.Parameters.AddWithValue("@UserID2", "%," + userId + ",%");
                cmd.Parameters.AddWithValue("@UserID3", "%," + userId);
                cmd.Parameters.AddWithValue("@UserID4", userId.ToString()); // Exact match

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lstadm1.Add(new UserProjectModel
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ProjectName = dr["ProjectName"].ToString(),
                        Category = dr["Category"].ToString(),
                        DueDate = Convert.ToDateTime(dr["DueDate"]),
                        Status = dr["Status"].ToString(),
                        Priority = dr["Priority"].ToString(),
                        Members = dr["Members"].ToString(),
                        Description = dr["Description"].ToString(),
                    });
                }
            }
            return lstadm1;
        }
        //Retrieve single record from a table
        public UserProjectModel getData(string Id)
        {
            UserProjectModel admin = new UserProjectModel();
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

        //Fetch the Data from a UserTask Table 

        public List<UserProjectModel> getdata(int userId)
        {
            List<UserProjectModel> lstuser = new List<UserProjectModel>();

            SqlCommand cmd = new SqlCommand("SELECT * FROM UsersTask WHERE UserId = @UserID", con);
            cmd.Parameters.AddWithValue("@UserID", userId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            con.Open();
            da.Fill(ds);
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstuser.Add(new UserProjectModel
                {
                    Id = Convert.ToInt32(dr["ID"]),
                    Title = dr["Title"].ToString(),
                    Description = dr["Description"].ToString(),
                    Priority = dr["Priority"].ToString(),
                    Status = dr["Status"].ToString(),
                    Date = Convert.ToDateTime(dr["Date"]),
                    FilePath = dr["FilePath"].ToString(),
                });
            }

            return lstuser;
        }
    }
}
