using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

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
        public int UserID { get; set; }
        public string FilePath { get; set; }

        public string FirstName { get; set; }

        //Insert A Record in a PersonalTask Table

        public bool insert(AddUserTaskModel model)
        {
            SqlCommand cmd = new SqlCommand("Insert into PersonalTask values (@UserID, @title, @description, @status, @priority, @date, @filepath, @username)", con);

            cmd.Parameters.AddWithValue("@UserID", model.UserID);
            cmd.Parameters.AddWithValue("@title", model.Title);
            cmd.Parameters.AddWithValue("@description", model.Description);
            cmd.Parameters.AddWithValue("status", model.Status);
            cmd.Parameters.AddWithValue("priority", model.Priority);
            cmd.Parameters.AddWithValue("date", model.Date);
            cmd.Parameters.AddWithValue("filepath", model.FilePath);
            cmd.Parameters.AddWithValue("username", model.UserName);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }
            return false;
        }

        //Select Data from a Users Table

       public List<AddUserTaskModel> getData()
        {
            List<AddUserTaskModel> lstadm = new List<AddUserTaskModel>();
            SqlDataAdapter da = new SqlDataAdapter("select * from Users",con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                lstadm.Add(new AddUserTaskModel
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    FirstName = dr["FirstName"].ToString()
                });
            }
            return lstadm;

        }
     
    }
}
