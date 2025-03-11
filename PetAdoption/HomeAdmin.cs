using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PetAdoption
{
    public class HomeAdmin : Form
    {
        // Variables existantes...
        private Panel topNavBar;
        private TableLayoutPanel buttonPanel;
        private Button btnShelter;
        private Button btnPet;
        private Button btnAdopter;
        private Button btnAdoptionRequest;
        private Button btnLogOut;
        private Label lblWelcome;
        private PictureBox petImage;
        private Panel mainPanel;

        // Variables pour les totaux
        private Label lblShelterCount;
        private Label lblPetCount;
        private Label lblAdoptionRequestCount;

        // Connexion à la base de données
        private MySqlConnection conn;

        // Références aux formulaires existants
        private ShelterForm shelterForm;
        private PetForm petForm;
        private AdopterForm adopterForm;
        private AdoptionRequestsForm adoptionRequestForm;
        private Form1 loginForm;

        public HomeAdmin()
        {
            // Initialiser la connexion à la base de données
            conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");
            InitializeUI();
            LoadTotals(); // Charger les totaux lors de l'initialisation
        }

        private void InitializeUI()
        {
            // Paramètres du formulaire
            this.Text = "Home Admin";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Barre de navigation supérieure
            topNavBar = new Panel
            {
                Size = new Size(this.Width, 50),
                Dock = DockStyle.Top,
                BackColor = Color.Teal
            };
            this.Controls.Add(topNavBar);

            // Panneau de boutons
            buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                BackColor = Color.Transparent
            };

            for (int i = 0; i < buttonPanel.ColumnCount; i++)
            {
                buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            }

            topNavBar.Controls.Add(buttonPanel);

            // Boutons
            btnShelter = CreateNavButton("Shelter");
            btnPet = CreateNavButton("Pet");
            btnAdopter = CreateNavButton("Adopter");
            btnAdoptionRequest = CreateNavButton("Adoption Request");
            btnLogOut = CreateNavButton("Log Out");

            // Ajouter les boutons au panneau de boutons
            buttonPanel.Controls.Add(btnShelter, 0, 0);
            buttonPanel.Controls.Add(btnPet, 1, 0);
            buttonPanel.Controls.Add(btnAdopter, 2, 0);
            buttonPanel.Controls.Add(btnAdoptionRequest, 3, 0);
            buttonPanel.Controls.Add(btnLogOut, 4, 0);

            // Panneau principal
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            this.Controls.Add(mainPanel);

            // Label de bienvenue
            lblWelcome = new Label
            {
                Text = "Welcome to your home!",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.Teal,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            // Image de l'animal
            petImage = new PictureBox
            {
                Image = Image.FromFile("D:\\PetAdoption\\dog logo.jpg"), // Remplacez par le chemin réel de votre image
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(150, 150),
            };

            // Centrer le label et l'image
            var contentPanel = new Panel
            {
                Size = new Size(300, 300),
                BackColor = Color.Transparent,
                Location = new Point((this.Width - 300) / 2, (this.Height - 300) / 2 - 50)
            };

            contentPanel.Controls.Add(petImage);
            petImage.Location = new Point((contentPanel.Width - petImage.Width) / 2, 20);

            contentPanel.Controls.Add(lblWelcome);
            lblWelcome.Location = new Point((contentPanel.Width - lblWelcome.Width) / 2, petImage.Bottom + 20);

            mainPanel.Controls.Add(contentPanel);

            // Ajouter les rectangles pour les totaux
            AddTotalRectangles();

            // Gestionnaires d'événements pour les boutons
            btnShelter.Click += (s, e) => OpenForm(shelterForm, new ShelterForm());
            btnPet.Click += (s, e) => OpenForm(petForm, new PetForm());
            btnAdopter.Click += (s, e) => OpenForm(adopterForm, new AdopterForm());
            btnAdoptionRequest.Click += (s, e) => OpenForm(adoptionRequestForm, new AdoptionRequestsForm());
            btnLogOut.Click += (s, e) => LogOut();
        }

        private void LoadTotals()
        {
            lblShelterCount.Text = GetTotalShelters().ToString();
            lblPetCount.Text = GetTotalPets().ToString();
            lblAdoptionRequestCount.Text = GetTotalAdoptionRequests().ToString();
        }

        private int GetTotalShelters()
        {
            return GetCount("SELECT COUNT(*) FROM shelter");
        }

        private int GetTotalPets()
        {
            return GetCount("SELECT COUNT(*) FROM pet");
        }

        private int GetTotalAdoptionRequests()
        {
            return GetCount("SELECT COUNT(*) FROM adoptionrequest Where status='En attente' ");
        }

        private int GetCount(string query)
        {
            int count = 0;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des totaux : " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        private void AddTotalRectangles()
        {
            // Panel pour le compte des refuges
            Panel shelterPanel = CreateTotalPanel("Total Shelters", "0");
            lblShelterCount = new Label
            {
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.Teal,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            shelterPanel.Controls.Add(lblShelterCount);
            shelterPanel.Location = new Point(50, 400);
            mainPanel.Controls.Add(shelterPanel);

            // Panel pour le compte des animaux
            Panel petPanel = CreateTotalPanel("Total Pets", "0");
            lblPetCount = new Label
            {
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.Teal,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            petPanel.Controls.Add(lblPetCount);
            petPanel.Location = new Point(300, 400);
            mainPanel.Controls.Add(petPanel);

            // Panel pour le compte des demandes d'adoption
            Panel adoptionRequestPanel = CreateTotalPanel("Total Adoption Requests", "0");
            lblAdoptionRequestCount = new Label
            {
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.Teal,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            adoptionRequestPanel.Controls.Add(lblAdoptionRequestCount);
            adoptionRequestPanel.Location = new Point(550, 400);
            mainPanel.Controls.Add(adoptionRequestPanel);
        }

        private Panel CreateTotalPanel(string title, string count)
        {
            Panel panel = new Panel
            {
                Size = new Size(200, 100),
                BackColor = Color.LightBlue,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Teal,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.Add(lblTitle);
            return panel;
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

        private void OpenForm(Form existingForm, Form newForm)
        {
            if (existingForm == null || existingForm.IsDisposed)
            {
                existingForm = newForm;
                existingForm.FormClosed += (s, e) => { if (s is Form f) { f.FormClosed -= null; } };
                existingForm.TopLevel = false;
                existingForm.Dock = DockStyle.Fill;
                mainPanel.Controls.Clear();
                mainPanel.Controls.Add(existingForm);
                existingForm.Show();
            }
            else
            {
                existingForm.BringToFront();
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
    }
}
