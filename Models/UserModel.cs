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


        public List<UserModel> UsersList { get; set; } = new List<UserModel>(); // Initialize by default


        //Retrieve all Records From a Table

        public List<UserModel> getdata()
        {
            List<UserModel> lstuser = new List<UserModel>();
            SqlDataAdapter da = new SqlDataAdapter("select * from PersonalTask", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                lstuser.Add(new UserModel
                {
                    Id = Convert.ToInt32(dr["ID"].ToString()),
                    Title = dr["Title"].ToString(),
                    Description = dr["Description"].ToString(),
                    Priority = dr["Priority"].ToString(),
                    Status = dr["Status"].ToString(),
                    Date = Convert.ToDateTime(dr["Date"].ToString())
                }) ;
            }
            return lstuser;
        }

        //Retrieve Single Record From A PersonalTask Table

        public UserModel getData(string Id)
        {
            UserModel usr = new UserModel();
            SqlCommand cmd = new SqlCommand("select * from PersonalTask where id ='" + Id +
             "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                while (dr.Read())
                {
                    usr.Id = Convert.ToInt32(dr["ID"].ToString());
                    usr.Title = dr["Title"].ToString();
                    usr.Description = dr["Description"].ToString();
                    usr.Priority = dr["Priority"].ToString();
                    usr.Status = dr["Status"].ToString();
                }
            }
            con.Close();
            return usr;
        }

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
