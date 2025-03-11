using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PetAdoption
{
    public class HomeAdopter : Form
    {
        private Panel topNavBar;
        private TableLayoutPanel buttonPanel;
        private Button btnHome;
        private Button btnAbout;
        private Button btnPet;
        private Label lblWelcome;
        private Panel mainPanel;
        private Button btnLogOut;
        private Form1 loginForm;


        private string connectionString = "server=localhost;port=3306;username=root;password=;database=pet";
        private int adopterId; 

        public HomeAdopter(int loggedInAdopterId) 
        {
            this.adopterId = loggedInAdopterId;
            InitializeUI();
            ShowHome();
        }

        private void InitializeUI()
        {
            // Form settings
            this.Text = "Home Adopter";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Top navigation bar
            topNavBar = new Panel
            {
                Size = new Size(this.Width, 50),
                Dock = DockStyle.Top,
                BackColor = Color.Teal
            };
            this.Controls.Add(topNavBar);

            // Buttons panel
            buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4, // Changer à 4 pour les 4 boutons
                RowCount = 1,
                BackColor = Color.Transparent
            };

            // Set equal width for all columns
            for (int i = 0; i < buttonPanel.ColumnCount; i++)
            {
                buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f)); // 25% pour chaque bouton
            }

            topNavBar.Controls.Add(buttonPanel);

            // Buttons
            btnHome = CreateNavButton("Home");
            btnAbout = CreateNavButton("About");
            btnPet = CreateNavButton("Pet");
            btnLogOut = CreateNavButton("Log Out");

            // Add buttons to the button panel
            buttonPanel.Controls.Add(btnHome, 0, 0);
            buttonPanel.Controls.Add(btnAbout, 1, 0);
            buttonPanel.Controls.Add(btnPet, 2, 0);
            buttonPanel.Controls.Add(btnLogOut, 3, 0); // Ajout du bouton de déconnexion

            // Main panel (content area)
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            this.Controls.Add(mainPanel);

            // Event handlers for buttons
            btnHome.Click += (s, e) => ShowHome();
            btnAbout.Click += (s, e) => ShowAbout();
            btnPet.Click += (s, e) => ShowPet();
            btnLogOut.Click += (s, e) => LogOut();
        }

        private Button CreateNavButton(string text)
        {
            return new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Teal,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Margin = new Padding(0)
            };
        }

        private void ShowHome()
        {
            mainPanel.Controls.Clear();

            // Create a container panel to center the label
            Panel centerPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            // Add the welcome label
            lblWelcome = new Label
            {
                Text = "Welcome to the place of big hearts,\nYour pet is waiting for you!",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.Teal,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Center the label within the panel
            centerPanel.Controls.Add(lblWelcome);
            centerPanel.Controls[0].Anchor = AnchorStyles.None; // Center the label
            lblWelcome.Location = new Point(
                (centerPanel.Width - lblWelcome.Width) / 2,
                (centerPanel.Height - lblWelcome.Height) / 2
            );

            // Add the centered panel to the main panel
            mainPanel.Controls.Add(centerPanel);
        }

        private void ShowAbout()
        {
            mainPanel.Controls.Clear();

            // Create a TableLayoutPanel for layout
            var aboutLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            // Set column widths
            aboutLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30)); // Image takes 30%
            aboutLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70)); // Text takes 70%

            // Add image
            var aboutImage = new PictureBox
            {
                Image = Image.FromFile("D:\\PetAdoption\\logo.jpg"), // Replace with your image path
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Fill
            };
            aboutLayout.Controls.Add(aboutImage, 0, 0); // Add image to the first column

            // Add text
            var lblAboutText = new Label
            {
                Text = "About Us\n\nOur goal is to create a platform where adopters and pets can connect. " +
                       "This application helps match loving adopters with pets in need of a forever home. " +
                       "We aim to simplify the adoption process and ensure every pet finds a caring family.",
                Font = new Font("Arial", 14, FontStyle.Regular),
                ForeColor = Color.Teal,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            aboutLayout.Controls.Add(lblAboutText, 1, 0); // Add text to the second column

            // Add the layout to the main panel
            mainPanel.Controls.Add(aboutLayout);
        }

        private void ShowPet()
        {
            mainPanel.Controls.Clear();

            // Create a FlowLayoutPanel to center the pet cards
            FlowLayoutPanel petFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight, // Align items horizontally
                WrapContents = true, // Wrap items to next line
                AutoScroll = true, // Add scrolling if needed
                Padding = new Padding(10, 50, 10, 10), // Padding (left, top, right, bottom)
                HorizontalScroll = { Enabled = false }
            };

            mainPanel.Controls.Add(petFlowPanel);

            // Load pets from the database
            LoadAvailablePets(petFlowPanel);
        }

        private void LoadAvailablePets(FlowLayoutPanel petFlowPanel)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT PetID, Name, Gender, Age, Description, Image FROM pet WHERE Adopted = 0";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable petTable = new DataTable();
                    adapter.Fill(petTable);

                    // Clear previous controls
                    petFlowPanel.Controls.Clear();

                    // Loop through each pet and create a panel for each one
                    foreach (DataRow row in petTable.Rows)
                    {
                        Panel petPanel = new Panel
                        {
                            Size = new Size(200, 350),  // Increase size to accommodate image
                            Margin = new Padding(10),
                            BorderStyle = BorderStyle.FixedSingle,
                            Padding = new Padding(10),
                            BackColor = Color.White
                        };

                        // Pet Name
                        Label lblName = new Label
                        {
                            Text = row["Name"].ToString(),
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Dock = DockStyle.Top,
                            Height = 40
                        };

                        // Pet Gender and Age
                        Label lblGenderAge = new Label
                        {
                            Text = $"Gender: {row["Gender"]}\nAge: {row["Age"]} year(s)",
                            Font = new Font("Arial", 10),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Dock = DockStyle.Top,
                            Height = 50
                        };

                        // Pet Description
                        Label lblDescription = new Label
                        {
                            Text = row["Description"].ToString(),
                            Font = new Font("Arial", 10),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Dock = DockStyle.Fill
                        };

                        // Load pet image (if available)
                        PictureBox picPet = new PictureBox
                        {
                            Size = new Size(180, 180),  // Adjust size of image
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Dock = DockStyle.Top
                        };

                        if (row["Image"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])row["Image"];  // Convert BLOB data to byte array
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                picPet.Image = Image.FromStream(ms);  // Create an Image object from the byte array
                            }
                        }
                        else
                        {
                            picPet.Image = Image.FromFile(@"C:\Users\hp\source\repos\PetAdoption\PetAdoption\Resources\dog_logo.png");
                        }

                        // Adopt Button
                        Button btnAdopt = new Button
                        {
                            Text = "Adopt",
                            BackColor = Color.Teal,
                            ForeColor = Color.White,
                            Font = new Font("Arial", 12, FontStyle.Bold),
                            Dock = DockStyle.Bottom,
                            Height = 40
                        };
                        btnAdopt.Click += (sender, e) => AdoptPet(Convert.ToInt32(row["PetID"])); // Call AdoptPet when clicked

                        // Add controls to the pet panel
                        petPanel.Controls.Add(lblName);
                        petPanel.Controls.Add(lblGenderAge);
                        petPanel.Controls.Add(lblDescription);
                        petPanel.Controls.Add(picPet);
                        petPanel.Controls.Add(btnAdopt);

                        // Add the pet panel to the FlowLayoutPanel
                        petFlowPanel.Controls.Add(petPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void AdoptPet(int petId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Insertion de la demande d'adoption
                    string queryInsert = "INSERT INTO adoptionrequest (AdopterID, PetID, RequestDate, Status) " +
                                         "VALUES (@AdopterID, @PetID, @RequestDate, 'En attente')";
                    using (MySqlCommand command = new MySqlCommand(queryInsert, connection))
                    {
                        command.Parameters.AddWithValue("@AdopterID", adopterId);
                        command.Parameters.AddWithValue("@PetID", petId);
                        command.Parameters.AddWithValue("@RequestDate", DateTime.Now);

                        command.ExecuteNonQuery();
                    }

                    // Mise à jour de la colonne adopted dans la table pet
                    string queryUpdate = "UPDATE pet SET adopted = 1 WHERE PetID = @PetID";
                    using (MySqlCommand command = new MySqlCommand(queryUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@PetID", petId);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Adoption request submitted successfully and pet marked as adopted!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adopting pet: {ex.Message}");
            }
        }







        private void AdoptButton_Click(object sender, EventArgs e, int petId)
        {
            // Handle adoption request when the "Adopt" button is clicked
            CreateAdoptionRequest(petId);
        }


        private void CreateAdoptionRequest(int petId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO adoptionrequest (AdopterID, PetID, RequestDate, Status) " +
                                   "VALUES (@AdopterID, @PetID, @RequestDate, 'Pending')";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AdopterID", adopterId);
                        command.Parameters.AddWithValue("@PetID", petId);
                        command.Parameters.AddWithValue("@RequestDate", DateTime.Now);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Adoption request submitted successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating adoption request: " + ex.Message);
            }
        }
        private void LogOut()
        {
            if (loginForm == null || loginForm.IsDisposed)
            {
                loginForm = new Form1();
            }
            this.Hide();
            loginForm.Show();
            loginForm.FormClosed += (s, e) => this.Close();
        }
        /*[STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HomeAdopter());
        }*/
    }
}
