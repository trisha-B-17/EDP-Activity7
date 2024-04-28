using MySql.Data.MySqlClient;
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
    public partial class Admin : Form
    {
        private const string String = "Server=127.0.0.1;Port=3306;Database=concert;Uid=root;Pwd=trisha;";
        private Dashboard dashboard;

        public Admin(Admin admin)
        {
            InitializeComponent();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            LoadUserAccounts();
        }

        public Admin(Dashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard; // Store the reference to the Dashboard form
        }

        private void LoadUserAccounts()
        {
            dataGridView1.Rows.Clear();

            string query = "SELECT username, password, acc_type, status FROM users";

            using (MySqlConnection connection = new MySqlConnection(String))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                bool isActive = reader.GetBoolean("status");
                                string status = isActive ? "Active" : "Inactive";
                                dataGridView1.Rows.Add(reader.GetString("username"), reader.GetString("password"), reader.GetString("acc_type"), status);
                            }
                        }
                    }
                }
            }
        }

    
        private void EditBtn_Click(object sender, EventArgs e)
        {
            ToggleAccountForm(true, true, false, true);
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                PopulateTextBoxes(selectedRow);
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }

       
        private void DeleteUserRecord(string username)
        {
            string query = "DELETE FROM users WHERE username = @username";

            using (MySqlConnection connection = new MySqlConnection(String))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                            MessageBox.Show("Record deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No records deleted.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Error deleting record: " + ex.Message);
                    }
                }
            }
        }

        private void ToggleAccountForm(bool formVisible, bool saveEditVisible, bool addLabelVisible, bool editLabelVisible)
        {
            formAccount.Visible = formVisible;
        }

        private void HideAccountForm()
        {
            formAccount.Hide();
        }

        private void PopulateTextBoxes(DataGridViewRow selectedRow)
        {
            formAccount.Visible = true;
            usernameTxtBx.Text = selectedRow.Cells["Username"].Value.ToString();
            passTxtBx.Text = selectedRow.Cells["Password"].Value.ToString();
            typeTxtBx.Text = selectedRow.Cells["acc_type"].Value.ToString();
        }

        private void LogoutUser()
        {
            using (MySqlConnection conn = new MySqlConnection(String))
            {
                conn.Open();
                string updateSql = "UPDATE users SET status = 0 WHERE username = @username";
                MySqlCommand updateCmd = new MySqlCommand(updateSql, conn);
                updateCmd.Parameters.AddWithValue("@username", UserInfo.Username);
                updateCmd.ExecuteNonQuery();

                var login = new LogIn();
                this.Hide();
                login.Show();
            }
        }

        private void SavePassBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string oldUsername = selectedRow.Cells["Username"].Value.ToString();
                string newUsername = usernameTxtBx.Text;
                string password = passTxtBx.Text;
                string type = typeTxtBx.Text;

                string query = "UPDATE users SET username = @newUsername, password = @password, acc_type = @type WHERE username = @oldUsername";

                using (MySqlConnection connection = new MySqlConnection(String))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@newUsername", newUsername);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@oldUsername", oldUsername);

                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Data updated successfully.");
                                LoadUserAccounts();
                                ToggleAccountForm(false, false, false, false);
                                ClearTextBoxes();
                            }
                            else
                            {
                                MessageBox.Show("No records were updated.");
                            }
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show("Error updating data: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }

        private void ClearTextBoxes()
        {
            usernameTxtBx.Clear();
            passTxtBx.Clear();
            typeTxtBx.Clear();
        }

          
        private void SaveEditAccBtn_Click(object sender, EventArgs e)
        {
            string username = usernameTxtBx.Text;
            string password = passTxtBx.Text;
            string type = typeTxtBx.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(type))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Editing an existing account
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string oldUsername = selectedRow.Cells["Username"].Value.ToString();
                EditUserRecord(oldUsername, username, password, type);
            }
            else
            {
                // Adding a new account
                AddUserRecord(username, password, type);
            }
        }

        private void AddUserRecord(string username, string password, string type)
        {
            string query = "INSERT INTO users (username, password, acc_type, status) VALUES (@username, @password, @type, 1)";

            using (MySqlConnection connection = new MySqlConnection(String))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@type", type);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("New account added successfully.");
                            LoadUserAccounts();
                            ToggleAccountForm(false, false, false, false);
                            ClearTextBoxes();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add new account.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Error adding new account: " + ex.Message);
                    }
                }
            }
        }

        private void EditUserRecord(string oldUsername, string newUsername, string password, string type)
        {
            string query = "UPDATE users SET username = @newUsername, password = @password, acc_type = @type WHERE username = @oldUsername";

            using (MySqlConnection connection = new MySqlConnection(String))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newUsername", newUsername);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@oldUsername", oldUsername);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Account updated successfully.");
                            LoadUserAccounts();
                            ToggleAccountForm(false, false, false, false);
                            ClearTextBoxes();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update account.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Error updating account: " + ex.Message);
                    }
                }
            }
        }

       
        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            string keyword = SearchBox.Text.Trim().ToLower();

            dataGridView1.ClearSelection();

            if (!string.IsNullOrEmpty(keyword))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["username"].Value.ToString().ToLower().Contains(keyword))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Visible = true;
                }
            }
        }

        private void formAccount_Paint(object sender, PaintEventArgs e)
        {

        }

        private void logout_Click(object sender, EventArgs e)
        {
            LogoutUser();
        }

        private void typeLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
