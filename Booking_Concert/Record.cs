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
using OfficeOpenXml;
using System.IO;
using LicenseContext = OfficeOpenXml.LicenseContext; //EPP packages to manipulate the excel file
using OfficeOpenXml.Drawing.Chart;// for charts and graphs
using OfficeOpenXml.Style; // changes fonts, style, etc


namespace Booking_Concert
{
    public partial class Record : Form
    {
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

        public Record()
        {
            InitializeComponent();
            InitializeDatabase(); // Initialize database connection
            LoadDataIntoDataGridView(); // Load data into DataGridView 
        }
        private void InitializeDatabase()
        {
            server = "localhost";
            database = "concert";
            uid = "root";
            // Construct connection string
            pass = "trisha";
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={pass};";
            connection = new MySqlConnection(connectionString); // Create MySqlConnection object named "connection"
        }
        private void LoadDataIntoDataGridView()
        {
            try
            {
                connection.Open();

                string query = "SELECT CONCAT(first_name, ' ', last_name) AS CustomerName, " +
               "email AS Email, " +
               "event_name AS EventDetails, " +
               "booking_date AS BookingDate, " +
               "tickets_booked AS TicketsBooked, " +
               "ticket_type AS TicketType, " +
               "price AS Price " +
               "FROM bookings";

                command = new MySqlCommand(query, connection);

                adapter = new MySqlDataAdapter(command);
                dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void textBox55_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pritnbtn_Click(object sender, EventArgs e)
        {
            string filePath = @"C:\Users\avell\OneDrive\Desktop\ConcertReport.xlsx"; // file path to be saved

            string query = "SELECT CONCAT(first_name, ' ', last_name) AS CustomerName, " +
               "email AS Email, " +
               "event_name AS EventDetails, " +
               "booking_date AS BookingDate, " +
               "tickets_booked AS TicketsBooked, " +
               "ticket_type AS TicketType, " +
               "price AS Price " +
               "FROM bookings";
            string graphquery = "SELECT event_name AS EventName,SUM(tickets_booked) AS TotalTicketsBooked FROM bookings GROUP BY event_name;";

            try
            {
                connection.Open();

                // Execute the SQL query to fetch booking records
                using (MySqlCommand RecordCommand = new MySqlCommand(query, connection))
                {
                    // Execute the query and read the results
                    using (MySqlDataReader RecordReader = RecordCommand.ExecuteReader())
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        // Create a new Excel package
                        ExcelPackage excelPackage = new ExcelPackage(); // Using the Excel package

                        // Add a worksheet for concert reports
                        ExcelWorksheet worksheet_1 = excelPackage.Workbook.Worksheets.Add("Concert Report"); // Creating the first sheet

                        // Add a worksheet for graphs
                        ExcelWorksheet worksheet_2 = excelPackage.Workbook.Worksheets.Add("Graphs"); // Creating the second sheet

                        // Add the company logo to the concert report sheet
                        var picture = worksheet_1.Drawings.AddPicture("Logo", new FileInfo("F:\\VisualStudio2022 Repository\\Booking_Concert\\Booking_Concert\\Properties\\Melody Pass.png"));
                        picture.SetSize(70, 70); // Set the picture size

                        // Set the width and height of the first cell
                        worksheet_1.Column(1).Width = 13.71;
                        worksheet_1.Row(1).Height = 52.50;
                        worksheet_1.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet_1.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.Black);

                        // Calculate offsets to center the picture in cell A1
                        double cellWidth = worksheet_1.Column(1).Width;
                        double cellHeight = worksheet_1.Row(1).Height;
                        double xOffset = cellWidth / 13.71; // Horizontal offset
                        double yOffset = cellHeight / 4; // Vertical offset

                        // Set the position of the picture to center it in cell A1
                        picture.SetPosition(0, (int)xOffset, 0, (int)yOffset);

                        // Merge cells and add the company name to the concert report
                        ExcelRange cellsToMerge = worksheet_1.Cells["B1:I1"];
                        cellsToMerge.Merge = true;
                        cellsToMerge.Value = "Concert Management System";
                        cellsToMerge.Style.Font.Size = 20;
                        cellsToMerge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cellsToMerge.Style.Fill.BackgroundColor.SetColor(Color.Black);
                        cellsToMerge.Style.Font.Name = "Impact";
                        cellsToMerge.Style.Font.Color.SetColor(Color.White);
                        cellsToMerge.Style.Font.Bold = true;
                        cellsToMerge.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cellsToMerge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Merge cells and add the report title to the concert report
                        ExcelRange cellsToMerge1 = worksheet_1.Cells["B4:H4"];
                        cellsToMerge1.Merge = true;
                        cellsToMerge1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cellsToMerge1.Style.Fill.BackgroundColor.SetColor(Color.Black);
                        cellsToMerge1.Style.Font.Name = "Impact";
                        cellsToMerge1.Style.Font.Color.SetColor(Color.White);
                        cellsToMerge1.Value = "Customer Booked Details";
                        cellsToMerge1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cellsToMerge1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Set column headers for the concert report
                        worksheet_1.Cells["B5"].Value = "Customer Name";
                        worksheet_1.Cells["C5"].Value = "Email";
                        worksheet_1.Cells["D5"].Value = "Event Details";
                        worksheet_1.Cells["E5"].Value = "Booking Date";
                        worksheet_1.Cells["F5"].Value = "Tickets Booked";
                        worksheet_1.Cells["G5"].Value = "Ticket Type";
                        worksheet_1.Cells["H5"].Value = "Price";

                        // Populate concert report data from the database
                        int row = 6;
                        while (RecordReader.Read())
                        {
                            worksheet_1.Cells[row, 2].Value = RecordReader["CustomerName"];
                            worksheet_1.Cells[row, 3].Value = RecordReader["Email"];
                            worksheet_1.Cells[row, 4].Value = RecordReader["EventDetails"];
                            worksheet_1.Cells[row, 5].Value = RecordReader["BookingDate"];
                            worksheet_1.Cells[row, 6].Value = RecordReader["TicketsBooked"];
                            worksheet_1.Cells[row, 7].Value = RecordReader["TicketType"];
                            worksheet_1.Cells[row, 8].Value = RecordReader["Price"];
                            row++;
                        }

                        // Add management signature line to the concert report
                        worksheet_1.Cells[19, 8].Value = "_________________________";
                        worksheet_1.Cells[20, 8].Value = "Management";

                        // Center align all cells and auto-fit columns in the concert report
                        worksheet_1.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet_1.Cells.AutoFitColumns();

                        //for logo 
                        var picture1 = worksheet_2.Drawings.AddPicture("Logo", new FileInfo("F:\\VisualStudio2022 Repository\\Booking_Concert\\Booking_Concert\\Properties\\Melody Pass.png"));
                        // Set the size of the picture
                        picture1.SetSize(70, 70); // Set the picture size in pixels

                        // Set the width of column 1 and height of row 1
                        worksheet_2.Column(1).Width = 13.71;
                        worksheet_2.Row(1).Height = 52.50;
                        worksheet_2.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet_2.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.Black);

                        // Calculate the offsets to center the picture in cell A1
                        double cellWidths = worksheet_2.Column(1).Width;
                        double cellHeights = worksheet_2.Row(1).Height;
                        double xOffsets = cellWidths / 13.71; // Calculate horizontal offset
                        double yOffsets = cellHeights / 4; // Calculate vertical offset

                        // Set the position of the picture to center it in cell A1
                        picture.SetPosition(0, (int)xOffsets, 0, (int)yOffsets);

                        //Merging and adding the company name
                        ExcelRange cellsToMerge2 = worksheet_2.Cells["B1:I1"];
                        cellsToMerge2.Merge = true;
                        cellsToMerge2.Value = "Concert Management System";
                        cellsToMerge2.Style.Font.Size = 20;
                        cellsToMerge2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cellsToMerge2.Style.Fill.BackgroundColor.SetColor(Color.Black);
                        cellsToMerge2.Style.Font.Name = "Impact";
                        cellsToMerge2.Style.Font.Color.SetColor(Color.White);
                        cellsToMerge2.Style.Font.Bold = true;
                        cellsToMerge2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cellsToMerge2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Close the reader after use
                        RecordReader.Close();

                        // Execute the SQL query to get aggregated ticket bookings for each event
                        using (MySqlCommand graphCommand = new MySqlCommand(graphquery, connection))
                        {
                            // Execute the query and read the results
                            using (MySqlDataReader graphReader = graphCommand.ExecuteReader())
                            {
                                // Add column headers for the graph worksheet
                                worksheet_2.Cells["C5"].Value = "Event Name";
                                worksheet_2.Cells["D5"].Value = "Total Tickets Booked";

                                // Initialize row counter for writing data to the worksheet
                                int row1 = 6;
                                // Loop through the query results and write data to the worksheet
                                while (graphReader.Read())
                                {
                                    // Write event name to column C and total tickets booked to column D
                                    worksheet_2.Cells[row1, 3].Value = graphReader["EventName"];
                                    worksheet_2.Cells[row1, 4].Value = graphReader["TotalTicketsBooked"];
                                    row1++;
                                }

                                // Add a pie chart to the worksheet
                                var chart = worksheet_2.Drawings.AddChart("chart1", eChartType.Pie);
                                // Set position and size of the chart
                                chart.SetPosition(2, 0, 0, 0);
                                chart.SetSize(600, 400);
                                // Set chart title
                                chart.Title.Text = "Tickets Booked by Event";
                                chart.Title.Font.Bold = true;

                                // Define chart data using the aggregated ticket bookings
                                var series = chart.Series.Add(worksheet_2.Cells["D6:D" + (row1 - 1)], worksheet_2.Cells["C6:C" + (row1 - 1)]);

                                // Add custom labels to the data points for the pie chart
                                for (int i = 0; i < chart.Series.Count; i++)
                                {
                                    ExcelPieChartSerie pieSeries2 = (ExcelPieChartSerie)chart.Series[i];
                                    pieSeries2.DataLabel.ShowCategory = true; // Show event name as category
                                    pieSeries2.DataLabel.ShowValue = true;    // Show total tickets booked
                                    pieSeries2.DataLabel.ShowLeaderLines = true; // Show lines connecting labels to slices
                                }

                                // Add management signature line
                                worksheet_2.Cells[25, 8].Value = "_________________________";
                                worksheet_2.Cells[26, 8].Value = "Management";

                                // Center align all cells and auto-fit columns
                                worksheet_2.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet_2.Cells.AutoFitColumns();

                                // Save the Excel file
                                excelPackage.SaveAs(new FileInfo(filePath));

                                // Show success message
                                MessageBox.Show("Excel file saved successfully!");

                                // Dispose the ExcelPackage object
                                excelPackage.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); // Close the connection
            }

        }
    }
}
