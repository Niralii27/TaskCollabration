using System.ComponentModel.DataAnnotations;
using System.Data;

using System.Data.SqlClient;

namespace TaskCollabration.Models
{
    public class FetchTaskUserModel
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
        public List<FetchTaskUserModel> UsersList1 { get; set; } = new List<FetchTaskUserModel>(); // Initialize by default

        //Fetch the Data from a UserTask Table 

        public List<FetchTaskUserModel> getdata1(int userId, int projectId)
        {
            List<FetchTaskUserModel> lstuser = new List<FetchTaskUserModel>();

            string query = "SELECT * FROM UsersTask WHERE UserId = @UserID AND ProjectId = @ProjectID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ProjectID", projectId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                da.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    lstuser.Add(new FetchTaskUserModel
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
            }

            return lstuser;
        }


        //Retrieve single record from a table
        public FetchTaskUserModel getData1(string Id)
        {
            FetchTaskUserModel admin = new FetchTaskUserModel();
            SqlCommand cmd = new SqlCommand("select * from UsersTask where id='" + Id +
            "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    admin.Id = Convert.ToInt32(dr["ID"]);
                    admin.Title = dr["Title"].ToString();
                    admin.Description = dr["Description"].ToString();
                    admin.Priority = dr["Priority"].ToString();
                    admin.Status = dr["Status"].ToString();
                    admin.Date = Convert.ToDateTime(dr["Date"]);
                    admin.FilePath = dr["FilePath"].ToString();
                }
            }
            con.Close();
            return admin;
        }

    }
}
