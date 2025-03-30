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

        public int UserID { get; set; }  
        public string FilePath { get; set; }

        public List<TeamLeaderModel> UsersList { get; set; } = new List<TeamLeaderModel>(); // Initialize by default


        //Retrieve all Records From a Table

        public List<TeamLeaderModel> getdata(int userId)
        {
            List<TeamLeaderModel> lstuser = new List<TeamLeaderModel>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM PersonalTask WHERE UserId = @UserID", con);
            cmd.Parameters.AddWithValue("@UserID", userId); 

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
                    Date = Convert.ToDateTime(dr["Date"].ToString()),
                    FilePath = dr["FilePath"].ToString()
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
                    team.Date = Convert.ToDateTime(dr["Date"].ToString());
                    team.Status = dr["Status"].ToString();
                    team.FilePath = dr["FilePath"].ToString();

                }
            }
            con.Close();
            return team;
        }

        //Insert A Record in a PersonalTask Table

        public bool insert(TeamLeaderModel model)
        {
            SqlCommand cmd = new SqlCommand("Insert into PersonalTask values (@UserID, @title, @description, @status, @priority, @date, @filepath)", con);

            cmd.Parameters.AddWithValue("@UserID", model.UserID);  
            cmd.Parameters.AddWithValue("@title", model.Title);
            cmd.Parameters.AddWithValue("@description", model.Description);
            cmd.Parameters.AddWithValue("status", model.Status);
            cmd.Parameters.AddWithValue("priority", model.Priority);
            cmd.Parameters.AddWithValue("date", model.Date);
            cmd.Parameters.AddWithValue("filepath", model.FilePath);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }
            return false;
        }

        //Update a Task Record
        public bool update(TeamLeaderModel model)
        {
            SqlCommand cmd = new SqlCommand("update PersonalTask set Title = @title, Description = @description, Status =  @status, Priority = @priority, Date = @date, FilePath = @filepath where Id = @id", con);

            cmd.Parameters.AddWithValue("@title", model.Title);
            cmd.Parameters.AddWithValue("@description", model.Description);
            cmd.Parameters.AddWithValue("@status", model.Status);
            cmd.Parameters.AddWithValue("@priority", model.Priority);
            cmd.Parameters.AddWithValue("@date", model.Date);
            cmd.Parameters.AddWithValue("@filepath", model.FilePath);
            cmd.Parameters.AddWithValue("@id", model.Id);


            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }
            return false;
        }

        //Delete Personal Task
        public bool delete(TeamLeaderModel model)
        {
            SqlCommand cmd = new SqlCommand("delete PersonalTask where Id = @id", con);
            cmd.Parameters.AddWithValue("@id", model.Id);
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

