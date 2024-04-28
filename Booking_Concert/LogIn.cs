using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Booking_Concert
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = this.username.Text;
            string password = this.password.Text;
            string accType = (loginbtn.Text == "LOGIN") ? "user" : "admin"; // Determine account type based on button text

            string String = "Server=127.0.0.1;Port=3306;Database=concert;Uid=root;Pwd=trisha;";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(String))
                {
                    conn.Open();

                    // Hash the entered password
                    string hashedPassword = HashPassword(password);
                    Console.WriteLine("Hashed password: " + hashedPassword); // Debugging statement

                    // Query to check username and password
                    string sql = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password AND acc_type = @accType";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword); // Use the hashed password for comparison
                    cmd.Parameters.AddWithValue("@accType", accType);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        // Set the status to 1 for the logged-in user
                        string updateSql = "UPDATE users SET status = 1 WHERE username = @username";
                        MySqlCommand updateCmd = new MySqlCommand(updateSql, conn);
                        updateCmd.Parameters.AddWithValue("@username", username);
                        updateCmd.ExecuteNonQuery();

                        //MessageBox.Show("You are now logged in");
                        UserInfo.Username = username;

                        // Redirect to appropriate dashboard based on account type
                        var home = new Dashboard();
                        this.Hide();
                        home.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username/password");
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void reset_Click_1(object sender, EventArgs e)
        {
            resetPass resetPassForm = new resetPass();

            // Show the resetPass form
            resetPassForm.Show();

            // Optionally, you can hide the current form
            this.Hide();
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array, which we can convert to a string
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


    }
}
