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
        public List<string> UserId { get; set; } // Change from string to List<string>

        public string? FilePath { get; set; }

        public string FirstName { get; set; }
        public List<string> SelectedUsers { get; set; } // Ensure this is a List

        public List<AddUserTaskModel> UsersList { get; set; } = new List<AddUserTaskModel>(); // Initialize by default


        //Insert A Record in a PersonalTask Table

        public bool insert(AddUserTaskModel model1)
        {
            bool isInserted = false;

            if (model1.UserId != null && model1.UserId.Count > 0) // Check if list is not empty
            {
                con.Open();
                foreach (var id in model1.UserId)
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO UsersTask VALUES (@title, @description, @status, @priority, @date, @filePath, @firstname)", con);

                    cmd.Parameters.AddWithValue("@title", model1.Title);
                    cmd.Parameters.AddWithValue("@description", model1.Description);
                    cmd.Parameters.AddWithValue("@status", model1.Status);
                    cmd.Parameters.AddWithValue("@priority", model1.Priority);
                    cmd.Parameters.AddWithValue("@date", model1.Date);
                    cmd.Parameters.AddWithValue("@filePath", model1.FilePath);
                    cmd.Parameters.AddWithValue("@firstname", id.Trim());

                    int i = cmd.ExecuteNonQuery();
                    if (i >= 1)
                    {
                        isInserted = true;
                    }
                }
                con.Close();
            }

            return isInserted;
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

        //Retrieve all Records From a Table

        public List<AddUserTaskModel> getdata()
        {
            List<AddUserTaskModel> lstuser = new List<AddUserTaskModel>();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM UsersTask", con);
            DataSet ds = new DataSet();

            con.Open();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstuser.Add(new AddUserTaskModel
                {
                    Id = Convert.ToInt32(dr["id"].ToString()),
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

        //Retrieve Single Record From A UsersTask Table

        public AddUserTaskModel getData(string Id)
        {
            AddUserTaskModel team = new AddUserTaskModel();
            SqlCommand cmd = new SqlCommand("select * from UsersTask where id ='" + Id +
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

        //Update a Task Record
        public bool update1(AddUserTaskModel model)
        {
            SqlCommand cmd = new SqlCommand("update UsersTask set Title = @title, Description = @description, Status =  @status, Priority = @priority, Date = @date, FilePath = @filepath where Id = @id", con);

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
        public bool delete(AddUserTaskModel model)
        {
            SqlCommand cmd = new SqlCommand("delete UsersTask where Id = @id", con);
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