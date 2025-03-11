using MySql.Data.MySqlClient;
using System.Data;
namespace PetAdoption
{
    public partial class Form1 : Form
    {
        MySqlConnection conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");
        public static int LoggedInAdopterId { get; set; }

        public Form1()
        {
            InitializeComponent();
            txtmotdepassse.PasswordChar = '*';
        }

        private void txtuser_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Open the database connection
                conn.Open();

                // Define the query to check the user in the Login table first (regular user)
                string loginQuery = "SELECT COUNT(*) FROM `Login` WHERE `user` = @user AND `password` = @password";
                // Define the query to check the user in the Adopter table
                string adopterQuery = "SELECT `adopterId` FROM `Adopter` WHERE `user` = @user AND `password` = @password";

                // Create a MySQL command for Login table
                using (MySqlCommand loginCmd = new MySqlCommand(loginQuery, conn))
                {
                    // Add parameters to the query to prevent SQL injection
                    loginCmd.Parameters.AddWithValue("@user", txtuser.Text.Trim());
                    loginCmd.Parameters.AddWithValue("@password", txtmotdepassse.Text.Trim());

                    // Execute the query for regular user
                    int loginCount = Convert.ToInt32(loginCmd.ExecuteScalar());

                    if (loginCount == 1)
                    {
                        // User found in Login table (regular user)
                        MessageBox.Show("Vous faites partie du système", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Open the HomeAdmin form for regular users
                        HomeAdmin homeAdmin = new HomeAdmin();
                        homeAdmin.Show();

                        // Hide the current login form
                        this.Hide();
                        return; // Exit the method
                    }
                }

                // If not found in Login table, check in Adopter table
                using (MySqlCommand adopterCmd = new MySqlCommand(adopterQuery, conn))
                {
                    // Add parameters to the query to prevent SQL injection
                    adopterCmd.Parameters.AddWithValue("@user", txtuser.Text.Trim());
                    adopterCmd.Parameters.AddWithValue("@password", txtmotdepassse.Text.Trim());

                    // Execute the query for adopter
                    object result = adopterCmd.ExecuteScalar();
                    if (result != null)
                    {
                        // User found in Adopter table (adopter)
                        int adopterId = Convert.ToInt32(result);
                        LoggedInAdopterId = adopterId; // Assign the adopterId to the static property

                        MessageBox.Show("Vous faites partie du système", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Open the HomeAdopter form for adopters
                        HomeAdopter homeAdopter = new HomeAdopter(LoggedInAdopterId);
                        homeAdopter.Show();

                        // Hide the current login form
                        this.Hide();
                        return; // Exit the method
                    }
                }

                // If not found in either table
                MessageBox.Show("Mot de passe ou nom d'utilisateur incorrect", "Erreur", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Handle any errors
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the connection is always closed
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }



        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SignupForm signupform = new SignupForm(); // Create an instance of Form2
            signupform.Show(); // Open Form2
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
      [STAThread]
        static void Main()
        {
            // Ensures that the application runs with a single-threaded apartment model
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start the application with Form1 as the main form
            Application.Run(new Form1());
        }
    }
}
