using MySql.Data.MySqlClient;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Booking_Concert
{
          public partial class SignUp : Form
        {
            public SignUp()
            {
                InitializeComponent();
            }

            private void button1_Click(object sender, EventArgs e)
            {
                string username = textBox1.Text;
                string password = textBox2.Text;
                string confirmPassword = textBox4.Text;

                // Validate if all fields are filled
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                // Validate if password and confirm password match
                if (password != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match.");
                    return;
                }

                // Hash the password
                string hashedPassword = HashPassword(password);

                // Insert user into the database
                string connectionString = "Server=127.0.0.1;Port=3306;Database=concert;Uid=root;Pwd=trisha;";
                string insertQuery = "INSERT INTO users (username, password) VALUES (@username, @password)";

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Account created successfully. You can now log in.");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error creating account: " + ex.Message);
                }
            }

            private string HashPassword(string password)
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }

            private void label5_Click(object sender, EventArgs e)
        {
            var login = new LogIn();
            this.Hide();
            login.Show();
        }
    }
}

