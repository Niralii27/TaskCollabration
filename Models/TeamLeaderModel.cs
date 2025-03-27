using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace TaskCollabration.Models
{
    public class TeamLeaderModel
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

        public int UserID { get; set; }  // ✅ यह प्रॉपर्टी Add करें

        public List<TeamLeaderModel> UsersList { get; set; } = new List<TeamLeaderModel>(); // Initialize by default


        //Retrieve all Records From a Table

        public List<TeamLeaderModel> getdata(int userId)
        {
            List<TeamLeaderModel> lstuser = new List<TeamLeaderModel>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM PersonalTask WHERE UserId = @UserID", con);
            cmd.Parameters.AddWithValue("@UserID", userId); // ✅ अब Parameter Set होगा

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            con.Open();
            da.Fill(ds);
            con.Close();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstuser.Add(new TeamLeaderModel
                {
                    Id = Convert.ToInt32(dr["ID"].ToString()),
                    Title = dr["Title"].ToString(),
                    Description = dr["Description"].ToString(),
                    Priority = dr["Priority"].ToString(),
                    Status = dr["Status"].ToString(),
                    Date = Convert.ToDateTime(dr["Date"].ToString())
                });
            }
            return lstuser;
        }

        //Retrieve Single Record From A PersonalTask Table

        public TeamLeaderModel getData(string Id)
        {
            TeamLeaderModel team = new TeamLeaderModel();
            SqlCommand cmd = new SqlCommand("select * from PersonalTask where id ='" + Id +
             "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    team.Id = Convert.ToInt32(dr["ID"].ToString());
                    team.Title = dr["Title"].ToString();
                    team.Description = dr["Description"].ToString();
                    team.Priority = dr["Priority"].ToString();
                    team.Status = dr["Status"].ToString();
                }
            }
            con.Close();
            return team;
        }

        //Insert A Record in a PersonalTask Table

        public bool insert(TeamLeaderModel model)
        {
            SqlCommand cmd = new SqlCommand("Insert into PersonalTask values (@UserID, @title, @description, @status, @priority, @date)", con);

            cmd.Parameters.AddWithValue("@UserID", model.UserID);  
            cmd.Parameters.AddWithValue("@title", model.Title);
            cmd.Parameters.AddWithValue("@description", model.Description);
            cmd.Parameters.AddWithValue("status", model.Status);
            cmd.Parameters.AddWithValue("priority", model.Priority);
            cmd.Parameters.AddWithValue("date", model.Date);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }
            return false;
        }
    }
}

