using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace TaskCollabration.Models
{
    public class UserModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter a Task Title")]

        public string Title { get; set; }
        [Required(ErrorMessage ="Please Enter a Task Description")]

        public string Description { get; set; }
        [Required(ErrorMessage = "Please Select a Status of your Task")]

        public string Status { get; set; }
        [Required(ErrorMessage = "Please Select a Priority of your Task")]
        
        public string Priority { get; set; }
        [Required(ErrorMessage = "Please enter a Due Date of your Task")]

        public DateTime Date { get; set; }

        //Insert A Record in a PersonalTask Table

        public bool insert(UserModel model)
        {
            SqlCommand cmd = new SqlCommand("Insert into PersonalTask values (@title, @description, @status, @priority, @date)", con);

            cmd.Parameters.AddWithValue("@title", model.Title);
            cmd.Parameters.AddWithValue("@description", model.Description);
            cmd.Parameters.AddWithValue("status", model.Status);
            cmd.Parameters.AddWithValue("priority", model.Priority);
            cmd.Parameters.AddWithValue("date", model.Date);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            if(i >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
