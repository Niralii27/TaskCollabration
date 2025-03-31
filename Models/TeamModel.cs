using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace TaskCollabration.Models
{
    public class TeamModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter a Task Title")]

        public string TaskName { get; set; }

        public string AssignTo { get; set; }

        public DateTime DueDate { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Checklist { get; set; }

        public string Attachments { get; set; }

        //Retrieve all Records From a Team Table

        public List<TeamModel> getData()
        {
            List<TeamModel> lstadm = new List<TeamModel>();
            SqlDataAdapter da = new SqlDataAdapter("select * from Teams", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstadm.Add(new TeamModel
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    TaskName = dr["TaskName"].ToString(),
                    AssignTo = dr["AssignTo"].ToString(),
                    DueDate =Convert.ToDateTime(dr["DueDate"].ToString()),
                    Category = dr["Category"].ToString(),
                    Description = dr["Description"].ToString(),
                    Checklist = dr["Checklist"].ToString(),
                    Attachments = dr["Attachments"].ToString(),
                   
                });
            }
            return lstadm;
        }

    }
}
