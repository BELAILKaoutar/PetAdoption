using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace PetAdoption
{
    public class SignupForm : Form
    {
        // MySQL connection string
        MySqlConnection conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");

        public SignupForm()
        {
            InitializeComponent();

        }
        private void btnSignup_Click(object sender, EventArgs e)
        {
            try
            {
                // Open the database connection
                conn.Open();

                // Get the adopter information from the form
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();  // Plain text password
                string hashedPassword = HashPassword(password);  // Hash the password

                string age = txtAge.Text.Trim();
                string gender = cmbGender.SelectedItem.ToString(); // Get gender from combo box
                string contact = txtContact.Text.Trim();

                // Check if the username already exists in the Adopter table
                string checkAdopterQuery = "SELECT COUNT(*) FROM `Adopter` WHERE `user` = @user";
                using (MySqlCommand cmd = new MySqlCommand(checkAdopterQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);

                    int adopterCount = Convert.ToInt32(cmd.ExecuteScalar());

                    // If username already exists in Adopter table
                    if (adopterCount > 0)
                    {
                        MessageBox.Show("Ce nom d'utilisateur est déjà pris. Veuillez en choisir un autre.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Insert adopter data into the Adopter table
                string insertAdopterQuery = "INSERT INTO `Adopter` (`user`, `Age`, `Gender`, `Contact`, `password`) VALUES (@user, @age, @gender, @contact, @password)";
                using (MySqlCommand cmd = new MySqlCommand(insertAdopterQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@contact", contact);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);  // Store the hashed password

                    // Execute the insert query
                    cmd.ExecuteNonQuery();
                }

                // Success message and redirect to login page
                MessageBox.Show("Inscription réussie!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Hide(); // Hide the current form
            }
            catch (Exception ex)
            {
                // Handle errors
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the connection is closed
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Method to hash the password
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes); // Return the hashed password as a base64 string
            }
        }

        private void InitializeComponent()
        {
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.txtContact = new System.Windows.Forms.TextBox();
            this.btnSignup = new System.Windows.Forms.Button();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblAge = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(150, 50);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 20);
            this.txtUsername.TabIndex = 0;

            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(150, 90);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;

            // 
            // txtAge
            // 
            this.txtAge.Location = new System.Drawing.Point(150, 130);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(200, 20);
            this.txtAge.TabIndex = 2;

            // 
            // cmbGender
            // 
            this.cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbGender.Items.AddRange(new object[] { "Male", "Female" });
            this.cmbGender.Location = new System.Drawing.Point(150, 170);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(200, 21);
            this.cmbGender.TabIndex = 3;

            // 
            // txtContact
            // 
            this.txtContact.Location = new System.Drawing.Point(150, 210);
            this.txtContact.Name = "txtContact";
            this.txtContact.Size = new System.Drawing.Size(200, 20);
            this.txtContact.TabIndex = 4;

            // 
            // btnSignup
            // 
            this.btnSignup.BackColor = System.Drawing.Color.DarkCyan; // Dark Cyan button color
            this.btnSignup.ForeColor = System.Drawing.Color.White;
            this.btnSignup.Location = new System.Drawing.Point(150, 250);
            this.btnSignup.Name = "btnSignup";
            this.btnSignup.Size = new System.Drawing.Size(200, 30);
            this.btnSignup.TabIndex = 5;
            this.btnSignup.Text = "Sign up";
            this.btnSignup.UseVisualStyleBackColor = false;
            this.btnSignup.Click += new System.EventHandler(this.btnSignup_Click);

            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(50, 50);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(61, 13);
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "Nom d'utilisateur";

            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(50, 90);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(41, 13);
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "Mot de passe";

            // 
            // lblAge
            // 
            this.lblAge.AutoSize = true;
            this.lblAge.Location = new System.Drawing.Point(50, 130);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(26, 13);
            this.lblAge.TabIndex = 8;
            this.lblAge.Text = "Âge";

            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(50, 170);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(44, 13);
            this.lblGender.TabIndex = 9;
            this.lblGender.Text = "Genre";

            // 
            // lblContact
            // 
            this.lblContact.AutoSize = true;
            this.lblContact.Location = new System.Drawing.Point(50, 210);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(44, 13);
            this.lblContact.TabIndex = 10;
            this.lblContact.Text = "Contact";

            // 
            // SignupForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.lblAge);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.btnSignup);
            this.Controls.Add(this.txtContact);
            this.Controls.Add(this.cmbGender);
            this.Controls.Add(this.txtAge);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.BackColor = System.Drawing.Color.Gray; // Beige background color
            this.Name = "SignupForm";
            this.Text = "Inscription Adopteur";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // Form components
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.TextBox txtContact;
        private System.Windows.Forms.Button btnSignup;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblContact;
    }
}
