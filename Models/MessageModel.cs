using System.ComponentModel.DataAnnotations;
using System.Data;

using System.Data.SqlClient;

namespace TaskCollabration.Models
{
    public class MessageModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter a Message")]

        public string Message { get; set; }

        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public DateTime DateTime { get; set; }

        public bool insert(MessageModel model)
        {
            using (SqlConnection con = new SqlConnection("YourConnectionString"))
            {
                string query = "INSERT INTO ProjectDiscussion (UserId, Message, ProjectId, DateTime) VALUES (@UserId, @message, @projectid, @datetime)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", model.UserId);
                    cmd.Parameters.AddWithValue("@message", model.Message);
                    cmd.Parameters.AddWithValue("@projectid", model.ProjectId);
                    cmd.Parameters.AddWithValue("@datetime", model.DateTime);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    return i >= 1;
                }
            }
        }

    }
}
