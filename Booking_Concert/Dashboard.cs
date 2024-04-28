using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Booking_Concert
{
    public partial class Dashboard : Form
    {
        private string username;

        public Dashboard()
        {
            InitializeComponent();
        }

        public Dashboard(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Implement the click event for pictureBox2 if needed
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            // Implement the click event for pictureBox4 if needed
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Implement the text changed event for textBox2 if needed
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // Implement the text changed event for textBox6 if needed
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // Implement the click event for pictureBox5 if needed
        }

        private void account_Click(object sender, EventArgs e)
        {
            try
            {
                var admin = new Admin(this); // Pass the instance of the Dashboard form
                this.Hide();
                admin.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error navigating to admin form: " + ex.Message);
            }
        }

        private void tickets_Click(object sender, EventArgs e)
        {
            try
            {
                var book = new Ticket_booking(this); // Pass the instance of the Dashboard form
                this.Hide();
                book.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error navigating to booking of ticket form: " + ex.Message);
            }
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox9_Click(object sender, EventArgs e)
        {
            var record = new Record();
            this.Hide();
            record.Show();
        }

        private void textBox9_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_Click_1(object sender, EventArgs e)
        {
            var record = new Record();
            this.Hide();
            record.Show();
        }

        private void textBox9_TextChanged_2(object sender, EventArgs e)
        {

        }
    }
}
