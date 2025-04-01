
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;


namespace TaskCollabration.Models
{
    public class AdminTeamModel
    {

        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        [Key]
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please Enter a Eplaoyyee Name")]
        [MinLength(3, ErrorMessage = "Employee Name must be at least 3 characters long.")]
        public string TaskName { get; set; }

        [Required(ErrorMessage = "Please enter a valid Phone Number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone Number must be 10 digits")]
        public string AssignTo { get; set; }

        [Required(ErrorMessage = "Please Enter a BirthDate")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Please Enter a Category")]
        public string Category { get; set; }


        [Required(ErrorMessage = "Please enter an Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid Email address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid Email format. Example: example@domain.com")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please Enter a Bio")]
        [MinLength(10, ErrorMessage = "Bio must be at least 10 characters long.")]
        public string Checklist { get; set; }

        //[Required(ErrorMessage = "Please select a Profile Picture")]
        //[RegularExpression(@"^.*\.(jpg|jpeg|png|gif)$", ErrorMessage = "Only JPG, JPEG, PNG, and GIF formats are allowed.")]
        public string Attachments { get; set; }

        //Retrive All Data
        internal List<AdminTeamModel> getData()
        {
            List<AdminTeamModel> teams = new List<AdminTeamModel>();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Teams", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                teams.Add(new AdminTeamModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    TaskName = dr["TaskName"].ToString(),
                    AssignTo = dr["AssignTo"].ToString(),
                    DueDate = Convert.ToDateTime(dr["DueDate"]),
                    Category = dr["Category"].ToString(),
                    Description = dr["Description"].ToString(),
                    Checklist = dr["Checklist"].ToString(),
                    Attachments = dr["Attachments"].ToString()
                });
            }
            return teams;
        }

        //Retrive Single Data
        public AdminTeamModel getData(string Id)
        {
            AdminTeamModel team = new AdminTeamModel();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Teams WHERE Id = @id", con);
            cmd.Parameters.AddWithValue("@id", Id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    team.Id = Convert.ToInt32(dr["Id"]);
                    team.TaskName = dr["TaskName"].ToString();
                    team.AssignTo = dr["AssignTo"].ToString();
                    team.DueDate = Convert.ToDateTime(dr["DueDate"]);
                    team.Category = dr["Category"].ToString();
                    team.Description = dr["Description"].ToString();
                    team.Checklist = dr["Checklist"].ToString();
                    team.Attachments = dr["Attachments"].ToString();
                }
            }
            con.Close();
            return team;
        }


        //Insert
        public bool insert(AdminTeamModel model)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Teams (TaskName, AssignTo, DueDate, Category, Description, Checklist, Attachments) VALUES (@taskname, @assignto, @duedate, @category, @description, @checklist, @attachments)", con);
            cmd.Parameters.AddWithValue("@taskname", model.TaskName);
            cmd.Parameters.AddWithValue("@assignto", model.AssignTo);
            cmd.Parameters.AddWithValue("@duedate", model.DueDate);
            cmd.Parameters.AddWithValue("@category", model.Category);
            cmd.Parameters.AddWithValue("@description", model.Description);
            //cmd.Parameters.AddWithValue("@checklist", model.Checklist);
            //cmd.Parameters.AddWithValue("@attachments", model.Attachments);
            cmd.Parameters.AddWithValue("@checklist", model.Checklist ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@attachments", model.Attachments ?? (object)DBNull.Value);


            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i >= 1;
        }

        //Update
        public bool update(AdminTeamModel team)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Teams SET TaskName = @taskname, AssignTo = @assignto, DueDate = @duedate, Category = @category, Description = @description, Checklist = @checklist, Attachments = @attachments WHERE Id = @id", con);

            cmd.Parameters.AddWithValue("@taskname", team.TaskName);
            cmd.Parameters.AddWithValue("@assignto", team.AssignTo);
            cmd.Parameters.AddWithValue("@duedate", team.DueDate);
            cmd.Parameters.AddWithValue("@category", team.Category);
            cmd.Parameters.AddWithValue("@description", team.Description);
            cmd.Parameters.AddWithValue("@checklist", string.IsNullOrEmpty(team.Checklist) ? (object)DBNull.Value : team.Checklist);
            cmd.Parameters.AddWithValue("@attachments", string.IsNullOrEmpty(team.Attachments) ? (object)DBNull.Value : team.Attachments);
            cmd.Parameters.AddWithValue("@id", team.Id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i >= 1;
        }




        //Delete
        public bool delete(AdminTeamModel team)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Teams WHERE Id = @id", con);
            cmd.Parameters.AddWithValue("@id", team.Id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i >= 1;
        }
    }
}
