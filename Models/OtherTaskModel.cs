using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace TaskCollabration.Models
{
    public class OtherTaskModel
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

        public String FilePath { get; set; }
        public int UserID { get; set; }



        public List<OtherTaskModel> UsersList { get; set; } = new List<OtherTaskModel>(); // Initialize by default


        //Retrieve all Records From a Table

        public List<OtherTaskModel> getdata(int userId)
        {
            List<OtherTaskModel> lstuser = new List<OtherTaskModel>();

            SqlCommand cmd = new SqlCommand("SELECT * FROM UsersTask WHERE UserId = @UserID", con);
            cmd.Parameters.AddWithValue("@UserID", userId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            con.Open();
            da.Fill(ds);
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstuser.Add(new OtherTaskModel
                {
                    Id = Convert.ToInt32(dr["ID"]),
                    Title = dr["Title"].ToString(),
                    Description = dr["Description"].ToString(),
                    Priority = dr["Priority"].ToString(),
                    Status = dr["Status"].ToString(),
                    Date = Convert.ToDateTime(dr["Date"]),
                    FilePath = dr["FilePath"].ToString(),
                });
            }

            return lstuser;
        }


        //Retrieve Single Record From A PersonalTask Table

        public OtherTaskModel getData(string Id)
        {
            OtherTaskModel usr = new OtherTaskModel();
            SqlCommand cmd = new SqlCommand("select * from UsersTask where id ='" + Id +
             "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    usr.Id = Convert.ToInt32(dr["ID"].ToString());
                    usr.Title = dr["Title"].ToString();
                    usr.Description = dr["Description"].ToString();
                    usr.Priority = dr["Priority"].ToString();
                    usr.Status = dr["Status"].ToString();
                    usr.Date = Convert.ToDateTime(dr["Date"].ToString());
                    usr.FilePath = dr["FilePath"].ToString();
                }
            }
            con.Close();
            return usr;
        }

        //Update a Task Record
        public bool update1(OtherTaskModel model)
        {
            SqlCommand cmd = new SqlCommand("update UsersTask set Status =  @status where Id = @id", con);

            
            cmd.Parameters.AddWithValue("@status", model.Status);
           
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
