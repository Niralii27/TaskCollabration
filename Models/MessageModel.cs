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

        public string Message { get; set; }  // Make sure this is spelled exactly like this

        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public DateTime DateTime { get; set; }
        public string UserName { get; set; } // Add this property


        public bool insert(MessageModel model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True"))
                {
                    string query = "INSERT INTO ProjectDiscussion (UserId, Message, ProjectId, DateTime) VALUES (@UserId, @message, @projectid, @datetime)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", model.UserId);
                        cmd.Parameters.AddWithValue("@message", model.Message ?? string.Empty);
                        cmd.Parameters.AddWithValue("@projectid", model.ProjectId);
                        cmd.Parameters.AddWithValue("@datetime", model.DateTime);

                        con.Open();
                        return cmd.ExecuteNonQuery() >= 1;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database Error: {ex.Message}");
                return false;
            }
        }

        public List<MessageModel> GetProjectMessages(int projectId)
        {
            List<MessageModel> messages = new List<MessageModel>();

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True"))
                {
                    // This query joins ProjectDiscussion with Users
                    // It matches UserId from ProjectDiscussion with id from Users
                    // And fetches FirstName from Users table
                    string query = @"SELECT pd.*, u.FirstName 
                            FROM ProjectDiscussion pd 
                            INNER JOIN Users u ON pd.UserId = u.id 
                            WHERE pd.ProjectId = @projectId 
                            ORDER BY pd.DateTime DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@projectId", projectId);
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MessageModel message = new MessageModel();

                                // Get values from reader
                                message.UserId = Convert.ToInt32(reader["UserId"]);
                                message.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                message.Message = reader["Message"].ToString();
                                message.DateTime = Convert.ToDateTime(reader["DateTime"]);
                                message.UserName = reader["FirstName"].ToString(); // Get FirstName from Users table

                                messages.Add(message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database Error: {ex.Message}");
            }

            return messages;
        }
    }
}
