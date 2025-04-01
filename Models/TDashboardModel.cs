using System.ComponentModel.DataAnnotations;
using System.Data;

using System.Data.SqlClient;


namespace TaskCollabration.Models
{
    public class TDashboardModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

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

        public string FilePath { get; set; }
        public int UserID { get; set; }
        public string ProjectId { get; set; }

        public string Source { get; set; } // To track which table the data came from


        public List<TReportsModel> ReportList1 { get; set; } = new List<TReportsModel>(); // Initialize by default


        //Retrieve all Records From a Task Table

        public List<TDashboardModel> getdata(int userId)
        {
            List<TDashboardModel> lstuser = new List<TDashboardModel>();

            try
            {
                // Create DataSet to hold both tables
                DataSet ds = new DataSet();

                // First get data from PersonalTask
                SqlDataAdapter personalTaskAdapter = new SqlDataAdapter("SELECT * FROM PersonalTask WHERE UserId = @userId", con);
                personalTaskAdapter.SelectCommand.Parameters.AddWithValue("@userId", userId);

                // Second get data from UsersTask
                SqlDataAdapter usersTaskAdapter = new SqlDataAdapter("SELECT * FROM UsersTask WHERE UserId = @userId", con);
                usersTaskAdapter.SelectCommand.Parameters.AddWithValue("@userId", userId);

                // Open connection and fill dataset with both tables
                con.Open();
                personalTaskAdapter.Fill(ds, "PersonalTask");
                usersTaskAdapter.Fill(ds, "UsersTask");
                con.Close();

                // Process PersonalTask data
                foreach (DataRow dr in ds.Tables["PersonalTask"].Rows)
                {
                    lstuser.Add(new TDashboardModel
                    {
                        Id = Convert.ToInt32(dr["id"].ToString()),
                        Title = dr["Title"].ToString(),
                        Description = dr["Description"].ToString(),
                        Priority = dr["Priority"].ToString(),
                        Status = dr["Status"].ToString(),
                        Date = Convert.ToDateTime(dr["Date"].ToString()),
                        FilePath = dr["FilePath"].ToString(),
                        // You might want to add a field to indicate which table this came from
                        Source = "PersonalTask"
                    });
                }

                // Process UsersTask data
                foreach (DataRow dr in ds.Tables["UsersTask"].Rows)
                {
                    lstuser.Add(new TDashboardModel
                    {
                        Id = Convert.ToInt32(dr["id"].ToString()),
                        Title = dr["Title"].ToString(),
                        Description = dr["Description"].ToString(),
                        Priority = dr["Priority"].ToString(),
                        Status = dr["Status"].ToString(),
                        Date = Convert.ToDateTime(dr["Date"].ToString()),
                        FilePath = dr["FilePath"].ToString(),
                        // You might want to add a field to indicate which table this came from
                        Source = "UsersTask"
                    });
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Consider logging the exception
            }
            finally
            {
                // Make sure the connection is closed even if an exception occurs
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return lstuser;
        }
    }
}
