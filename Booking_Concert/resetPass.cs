using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Booking_Concert
{
    public partial class resetPass : Form
    {
        public resetPass()
        {
            InitializeComponent();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            string username = usernametxt.Text;
            string password = passwordtxt.Text;

            // Validate input
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and new password.");
                return;
            }

            // Hash the password
            string hashedPassword = HashPassword(password);

            // Validate password strength (e.g., length, complexity)

            string myConnectionString = "server=127.0.0.1;uid=root;pwd=trisha;database=concert";
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(myConnectionString);
                conn.Open();

                // Check if the username exists
                string sql = "SELECT COUNT(*) FROM users WHERE username = @username";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Username not found.");
                        return;
                    }
                }

                // Update the password
                sql = "UPDATE users SET password = @password WHERE username = @username";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@username", username);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password reset successful.");
                        // Clear textboxes
                        usernametxt.Clear();
                        passwordtxt.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Password reset failed.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close(); // Close the connection
                    conn.Dispose(); // Dispose of the connection object
                }
            }

            // Return to login form
            var login = new LogIn();
            login.Show();
            this.Hide();
        }

        private string HashPassword(string password)
        {
            // Convert the password to bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Create a SHA256 hash algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value of the password bytes
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hash bytes to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
