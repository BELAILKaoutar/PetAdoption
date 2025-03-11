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
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography;

namespace PetAdoption
{
    public partial class Form2 : Form
    {
        MySqlConnection conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");

        public Form2()
        {
            InitializeComponent();

        }

        private void txt_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
            {
                // Define your connection string (replace with your database details)
                conn.Open();

                // SQL Insert Query
                string query = "INSERT INTO `login`(`user`, `password`, `DateOfBirth`, `PhoneNumber`, `EmailAddress`, `PhysicalAddress`, `Occupation`) " +
                                   "VALUES (@user, @password, @DateOfBirth, @PhoneNumber, @EmailAddress, @PhysicalAddress, @Occupation)";

                // Use a try-catch block to handle errors
                try
                {
                    using (MySqlConnection conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet"))
                    {
                        conn.Open();

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            // Hash the password before storing it
                            string hashedPassword = HashPassword(txtPassword.Text);

                            // Bind parameters to prevent SQL injection
                            cmd.Parameters.AddWithValue("@user", txtUser.Text);
                            cmd.Parameters.AddWithValue("@password", hashedPassword); // Use the hashed password
                            cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(txtDateOfBirth.Text));
                            cmd.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text);
                            cmd.Parameters.AddWithValue("@EmailAddress", txtEmailAddress.Text);
                            cmd.Parameters.AddWithValue("@PhysicalAddress", txtPhysicalAddress.Text);
                            cmd.Parameters.AddWithValue("@Occupation", txtOccupation.Text);

                            // Execute the query
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Function to hash the password using SHA-256
            private string HashPassword(string password)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    // Convert the input string to a byte array and compute the hash
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                    // Convert the byte array to a hexadecimal string
                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in bytes)
                    {
                        builder.Append(b.ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
    

    private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
