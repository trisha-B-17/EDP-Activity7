using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OfficeOpenXml;
using System.IO;
using LicenseContext = OfficeOpenXml.LicenseContext; //EPP packages to manipulate the excel file
using OfficeOpenXml.Drawing.Chart;// for charts and graphs
using OfficeOpenXml.Style; // changes fonts, style, etc



namespace Booking_Concert
{
    public partial class Ticket_booking : Form
    {
        // Connection string
        // Declare private fields for database connection and command
        private MySqlConnection connection;
        private MySqlCommand command;
        // Database connection parameters
        private string server;
        private string database;
        private string uid;
        private string pass;
        private MySqlDataAdapter adapter;
        private DataTable dataTable;
        private Dashboard dashboard;

        private const string String = "Server=127.0.0.1;Port=3306;Database=concert;Uid=root;Pwd=trisha;";

        public Ticket_booking()
        {
            InitializeComponent();
            connection = new MySqlConnection(String);
        }

        public Ticket_booking(Dashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;        
             connection = new MySqlConnection(String);
        }
        
        private void btnbook_click(object sender, EventArgs e)
        {
            string insertQuery = "INSERT INTO bookings (first_name, last_name, email, event_name, booking_date, tickets_booked, ticket_type) " +
                                 "VALUES (@first_name, @last_name, @email, @event_name, @booking_date, @tickets_booked, @ticket_type)";

            string selectPriceQuery = "SELECT price FROM bookings";

            {
                connection.Open();

                // Fetch the price based on the selected ticket_type
                using (MySqlCommand priceCmd = new MySqlCommand(selectPriceQuery, connection))
                {
                    priceCmd.Parameters.AddWithValue("@ticket_type", textBox5.Text);

                    object priceResult = priceCmd.ExecuteScalar();

                    if (priceResult != null)
                    {
                        decimal price = Convert.ToDecimal(priceResult);

                        // Set the parameters from the TextBox values
                        using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@first_name", textBox1.Text);
                            cmd.Parameters.AddWithValue("@last_name", textBox2.Text);
                            cmd.Parameters.AddWithValue("@email", textBox3.Text);
                            cmd.Parameters.AddWithValue("@event_name", textBox8.Text);
                            cmd.Parameters.AddWithValue("@booking_date", DateTime.Now);  // You can replace this with a DateTimePicker value
                            cmd.Parameters.AddWithValue("@tickets_booked", int.Parse(textBox6.Text));  // Assuming tickets_booked is an integer
                            cmd.Parameters.AddWithValue("@ticket_type", textBox5.Text);

                            try
                            {
                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Booking inserted successfully!");
                                    string formattedPrice = price.ToString("0.00");
                                    textBox7.Text = "Price: $" + formattedPrice;
                                }
                                else
                                {
                                    MessageBox.Show("No rows affected.");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ticket type not found.");
                    }
                }
            }
        }




        private void button1_Click(object sender, EventArgs e)
        {
            var dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}