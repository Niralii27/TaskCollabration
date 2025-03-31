using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
namespace TaskCollabration.Models
{
    public class SettingModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get;set; }   

        public string Type { get; set; }

        public string City { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public string Pincode { get; set; }

        public string Password { get; set; }

        public string UserRole { get; set; }

        public string Image { get; set; }

        public int UserID { get; set; }

        public List<SettingModel> SettingList { get; set; } = new List<SettingModel>(); // Initialize by default


        //Retrieve all records from a Users Table
        public List<SettingModel> getdata(int userId)
        {
            List<SettingModel> lstuser = new List<SettingModel>();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Id = @UserID", con);
            cmd.Parameters.AddWithValue("@UserID", userId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            con.Open();
            da.Fill(ds);
            con.Close();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstuser.Add(new SettingModel
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    FirstName = dr["FirstName"].ToString(),
                    LastName = dr["LastName"].ToString(),
                    StreetAddress1 = dr["StreetAddress1"].ToString(),
                    StreetAddress2 = dr["StreetAddress2"].ToString(),
                    Type = dr["Type"].ToString(),
                    MobileNumber = dr["MobileNumber"].ToString(),
                    Email = dr["Email"].ToString(),
                    Pincode = dr["Pincode"].ToString(),
                    City = dr["City"].ToString(),
                    Password = dr["Password"].ToString(),
                    UserRole = dr["UserRole"].ToString(),
                    Image = dr["Image"].ToString()
                });
            }

            return lstuser;
        }


        //Retrieve Single Record From A PersonalTask Table

        public SettingModel getData(string Id)
        {
            SettingModel admin = new SettingModel();
            SqlCommand cmd = new SqlCommand("select * from Users where id ='" + Id +
             "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    admin.Id = Convert.ToInt32(dr["Id"].ToString());
                    admin.FirstName = dr["FirstName"].ToString();
                    admin.LastName = dr["LastName"].ToString();
                    admin.StreetAddress1 = dr["StreetAddress1"].ToString();
                    admin.StreetAddress2 = dr["StreetAddress2"].ToString();
                    admin.Type = dr["Type"].ToString();
                    admin.MobileNumber = dr["MobileNumber"].ToString();
                    admin.Email = dr["Email"].ToString();
                    admin.Pincode = dr["Pincode"].ToString();
                    admin.City = dr["City"].ToString();
                    admin.UserRole = dr["UserRole"].ToString();

                }
            }
            con.Close();
            return admin;
        }

        //Update a Users Table Record
        public bool update(SettingModel model)
        {
            SqlCommand cmd = new SqlCommand("update Users set FirstName = @firstname, LastName = @lastname, StreetAddress1 = @streetaddress1, StreetAddress2 = @streetaddress2, MobileNumber = @mobilenumber, Email = @email, Pincode = @pincode, City = @city where Id = @id", con);

            cmd.Parameters.AddWithValue("@firstname", model.FirstName);
            cmd.Parameters.AddWithValue("@lastname", model.LastName);
            cmd.Parameters.AddWithValue("@streetaddress1", model.StreetAddress1);
            cmd.Parameters.AddWithValue("@streetaddress2", model.StreetAddress2);
            cmd.Parameters.AddWithValue("@mobilenumber", model.MobileNumber);
            cmd.Parameters.AddWithValue("@email", model.Email);
            cmd.Parameters.AddWithValue("@pincode", model.Pincode);
            cmd.Parameters.AddWithValue("@city", model.City);
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
