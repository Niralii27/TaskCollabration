using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;

using System.Data.SqlClient;
using System.Reflection;


namespace TaskCollabration.Models
{
    public class AdminModel
    {
      SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        [Key]
        public int Id { get; set; }
       [Required(ErrorMessage = "Please Enter a FirstName")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter a LastName")]

        public string LastName { get; set; }
        [Required(ErrorMessage = "Please Enter a StreetAddress1")]

        public string StreetAddress1 { get; set; }
        [Required(ErrorMessage = "Please Enter a StreetAddress2")]

        public string StreetAddress2 { get; set; }
        [Required(ErrorMessage = "Please Enter a Type")]

        public string Type { get;set; }
        [Required(ErrorMessage = "Please Enter a MobileNumber")]

        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "Please Enter a Email")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter a PinCode")]
        public string Pincode { get; set; }
        [Required(ErrorMessage = "Please Enter a City")]

        public string City { get; set; }
        [Required(ErrorMessage = "Please Enter a Password")]

        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter a UserRole")]

        public string UserRole { get; set; }
        [Required(ErrorMessage = "Please Enter a Image")]

        public string Image { get; set; }


        
        //Retrieve all Records From a table
        public List<AdminModel> getData()
        {
            List<AdminModel> lstadm = new List<AdminModel>();
            SqlDataAdapter da = new SqlDataAdapter("select * from Users", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstadm.Add(new AdminModel
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
            return lstadm;
        }
        //Retrieve single record from a table
        public AdminModel getData(string Id)
        {
            AdminModel admin = new AdminModel();
            SqlCommand cmd = new SqlCommand("select * from Users where id='" + Id +
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
        //Insert A record in table

        public bool insert(AdminModel model)
        {

            
            SqlCommand cmd = new SqlCommand("insert into Users values(@firstname, @lastname, @streetaddress1, @streetaddress2, @type, @mobilenumber, @email, @pincode, @city, @password, @userrole, @image)", con);

            cmd.Parameters.AddWithValue("@firstname", model.FirstName);
            cmd.Parameters.AddWithValue("@lastname", model.LastName);
            cmd.Parameters.AddWithValue("@streetaddress1", model.StreetAddress1);
            cmd.Parameters.AddWithValue("@streetaddress2", model.StreetAddress2);
            cmd.Parameters.AddWithValue("@type", model.Type);
            cmd.Parameters.AddWithValue("@mobilenumber", model.MobileNumber);
            cmd.Parameters.AddWithValue("@email", model.Email);
            cmd.Parameters.AddWithValue("@pincode", model.Pincode);
            cmd.Parameters.AddWithValue("@city", model.City);
            cmd.Parameters.AddWithValue("@password", model.Password);
            cmd.Parameters.AddWithValue("@userrole", model.UserRole);
            cmd.Parameters.AddWithValue("@image", model.Image);


            con.Open();
            int i = cmd.ExecuteNonQuery();
            if(i>=1)
            {
                return true;
            }
            return false;
        }

        //update a record

       public bool update(AdminModel admin)
        {
            SqlCommand cmd = new SqlCommand("update Users set Firstname = @firstname, LastName = @lastname, StreetAddress1 = @streetaddress1, StreetAddress2 = @streetaddress2, Type = @type, MobileNumber = @mobilenumber, Email = @email, Pincode = @pincode, City = @city, Userrole = @userrole where Id = @id", con);

            cmd.Parameters.AddWithValue("@firstname", admin.FirstName);
            cmd.Parameters.AddWithValue("@lastname", admin.LastName);
            cmd.Parameters.AddWithValue("@streetaddress1", admin.StreetAddress1);
            cmd.Parameters.AddWithValue("@streetaddress2", admin.StreetAddress2);
            cmd.Parameters.AddWithValue("@type", admin.Type);
            cmd.Parameters.AddWithValue("@mobilenumber", admin.MobileNumber);
            cmd.Parameters.AddWithValue("@email", admin.Email);
            cmd.Parameters.AddWithValue("@pincode", admin.Pincode);
            cmd.Parameters.AddWithValue("@city", admin.City);
            cmd.Parameters.AddWithValue("@userrole", admin.UserRole);
            cmd.Parameters.AddWithValue("@id", admin.Id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            if(i>=1)
            {
                return true;
            }
            return false;

        }
       

    }
}
