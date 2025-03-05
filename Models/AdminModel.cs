using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace TaskCollabration.Models
{
    public class AdminModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Task;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter a name")]

        public String UserName { get; set; }
        [Required(ErrorMessage = "Please enter an Email")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a Password")]

        public String Password { get; set; }
        [Required(ErrorMessage = "Please enter a Type")]

        public String Type { get; set; }
        [Required(ErrorMessage = "Please enter a First Name")]

        public String FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a Last Name")]

        public String LastName { get; set; }
        [Required(ErrorMessage = "Please enter a Mobile Number")]
        public String MobileNumber { get; set; }
        [Required(ErrorMessage = "Please enter a Street Address1")]

        public String StreetAddress1 { get; set; }
        [Required(ErrorMessage = "Please enter a street Address2")]

        public String StreetAddress2 { get; set; }
        [Required(ErrorMessage = "Please enter a PinCode")]

        public String Pincode { get; set; }
        [Required(ErrorMessage = "Please enter a City")]

        public String City { get; set; }
        public String Image { get; set; }
        [Required(ErrorMessage = "Enter a User role")]

        public String UserRole { get; set; }


        //Insert a User Record

        public bool insert(AdminModel admin)
        {
            SqlCommand cmd = new SqlCommand("insert into user values(@username, @email, @password, @type, @first_name, @last_name, @mo_number, @street_Address1, @street_Address2, @pin_code, @city, @image, @userRole)", con);

            cmd.Parameters.AddWithValue("@username", admin.UserName);
            cmd.Parameters.AddWithValue("@email", admin.Email);
            cmd.Parameters.AddWithValue("@password", admin.Password);
            cmd.Parameters.AddWithValue("@type", admin.Type);
            cmd.Parameters.AddWithValue("@first_name", admin.FirstName);
            cmd.Parameters.AddWithValue("@last_name", admin.LastName);
            cmd.Parameters.AddWithValue("mo_number", admin.MobileNumber);
            cmd.Parameters.AddWithValue("@street_Address1", admin.StreetAddress1);
            cmd.Parameters.AddWithValue("@street_Address2", admin.StreetAddress2);
            cmd.Parameters.AddWithValue("@pin_code", admin.Pincode);
            cmd.Parameters.AddWithValue("@city", admin.City);
            cmd.Parameters.AddWithValue("@image", admin.Image);
            cmd.Parameters.AddWithValue("@userRole", admin.UserRole);

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
