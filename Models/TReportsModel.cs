using System.ComponentModel.DataAnnotations;
using System.Data;

using System.Data.SqlClient;

namespace TaskCollabration.Models
{
    public class TReportsModel
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


        public List<TReportsModel> ReportList1 { get; set; } = new List<TReportsModel>(); // Initialize by default


        //Retrieve all Records From a Task Table

        public List<TReportsModel> getdata(int userId)
        {
            List<TReportsModel> lstuser = new List<TReportsModel>();
            // Modifica la consulta SQL para filtrar por ID de usuario
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM PersonalTask WHERE UserId = @userId", con);
            da.SelectCommand.Parameters.AddWithValue("@userId", userId);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstuser.Add(new TReportsModel
                {
                    Id = Convert.ToInt32(dr["id"].ToString()),
                    Title = dr["Title"].ToString(),
                    Description = dr["Description"].ToString(),
                    Priority = dr["Priority"].ToString(),
                    Status = dr["Status"].ToString(),
                    Date = Convert.ToDateTime(dr["Date"].ToString()),
                    FilePath = dr["FilePath"].ToString(),
                });
            }
            return lstuser;
        }
    }
}
