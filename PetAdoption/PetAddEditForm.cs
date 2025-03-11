using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

public class PetAddEditForm : Form
{
    private MySqlConnection conn;
    private int petId; // For storing the PetID when editing
    private TextBox txtName, txtAge, txtBreed, txtDescription;
    private ComboBox cmbSpecies, cmbGender;
    private CheckBox chkVaccinated, chkAdopted;
    private PictureBox petImageBox;
    private Button btnUploadImage, btnSave, btnCancel;
    private DataGridView petGridView;
    private string imagePath;

    public PetAddEditForm(int petId = 0)
    {
        this.petId = petId;
        conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");

        // Form properties
        this.Text = petId == 0 ? "Ajouter un Animal" : "Modifier un Animal";
        this.Size = new Size(400, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Initialize controls
        InitializeControls();

        // If editing an existing pet, load its details
        if (petId != 0)
        {
            LoadPetDetails(petId);
        }
    }

    private void InitializeControls()
    {
        // Pet Image label, PictureBox, and Upload Button
        Label lblImage = new Label { Text = "Image", Location = new Point(20, 20) };
        petImageBox = new PictureBox { Location = new Point(150, 20), Size = new Size(100, 100), BorderStyle = BorderStyle.FixedSingle };
        btnUploadImage = new Button
        {
            Text = "Télécharger",
            Location = new Point(260, 60),
            BackColor = Color.Teal,
            ForeColor = Color.White
        };
        btnUploadImage.Click += (sender, e) => UploadImage();

        // Pet Name label and TextBox
        Label lblName = new Label { Text = "Nom de l'Animal", Location = new Point(20, 140) };
        txtName = new TextBox { Location = new Point(150, 140), Width = 200 };

        // Age label and TextBox
        Label lblAge = new Label { Text = "Âge", Location = new Point(20, 180) };
        txtAge = new TextBox { Location = new Point(150, 180), Width = 200 };

        // Species label and ComboBox
        Label lblSpecies = new Label { Text = "Espèce", Location = new Point(20, 220) };
        cmbSpecies = new ComboBox { Location = new Point(150, 220), Width = 200 };
        cmbSpecies.Items.AddRange(new string[] { "Chat", "Chien", "Lapin", "Oiseau" });

        // Breed label and TextBox
        Label lblBreed = new Label { Text = "Race", Location = new Point(20, 260) };
        txtBreed = new TextBox { Location = new Point(150, 260), Width = 200 };

        // Gender label and ComboBox
        Label lblGender = new Label { Text = "Sexe", Location = new Point(20, 300) };
        cmbGender = new ComboBox { Location = new Point(150, 300), Width = 200 };
        cmbGender.Items.AddRange(new string[] { "Mâle", "Femelle" });

        // Vaccinated checkbox
        Label lblVaccinated = new Label { Text = "Vacciné", Location = new Point(20, 340) };
        chkVaccinated = new CheckBox { Location = new Point(150, 340) };

        // Adopted checkbox
        Label lblAdopted = new Label { Text = "Adopté", Location = new Point(20, 380) };
        chkAdopted = new CheckBox { Location = new Point(150, 380) };

        // Description label and TextBox
        Label lblDescription = new Label { Text = "Description", Location = new Point(20, 420) };
        txtDescription = new TextBox { Location = new Point(150, 420), Width = 200, Height = 60, Multiline = true };

        // Save Button
        btnSave = new Button
        {
            Text = "Enregistrer",
            Location = new Point(150, 500),
            Width = 100,
            BackColor = Color.Teal,
            ForeColor = Color.White
        };
        btnSave.Click += (sender, e) => SavePet();

        // Cancel Button
        btnCancel = new Button
        {
            Text = "Annuler",
            Location = new Point(260, 500),
            Width = 100,
            BackColor = Color.Gray,
            ForeColor = Color.White
        };
        btnCancel.Click += (sender, e) => this.Close();

        // Add controls to the form
        this.Controls.Add(lblImage);
        this.Controls.Add(petImageBox);
        this.Controls.Add(btnUploadImage);
        this.Controls.Add(lblName);
        this.Controls.Add(txtName);
        this.Controls.Add(lblAge);
        this.Controls.Add(txtAge);
        this.Controls.Add(lblSpecies);
        this.Controls.Add(cmbSpecies);
        this.Controls.Add(lblBreed);
        this.Controls.Add(txtBreed);
        this.Controls.Add(lblGender);
        this.Controls.Add(cmbGender);
        this.Controls.Add(lblVaccinated);
        this.Controls.Add(chkVaccinated);
        this.Controls.Add(lblAdopted);
        this.Controls.Add(chkAdopted);
        this.Controls.Add(lblDescription);
        this.Controls.Add(txtDescription);
        this.Controls.Add(btnSave);
        this.Controls.Add(btnCancel);
    }


    private void UploadImage()
    {
        using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp" })
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imagePath = ofd.FileName;
                petImageBox.Image = Image.FromFile(imagePath);
            }
        }
    }


    private void LoadPetDetails(int petId)
    {
        try
        {
            conn.Open();
            string query = "SELECT * FROM pet WHERE PetID = @PetID";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PetID", petId);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                txtName.Text = reader["Name"].ToString();
                txtAge.Text = reader["Age"].ToString();
                cmbSpecies.SelectedItem = reader["Species"].ToString();
                txtBreed.Text = reader["Breed"].ToString();
                cmbGender.SelectedItem = reader["Gender"].ToString();
                chkVaccinated.Checked = Convert.ToBoolean(reader["Vaccinated"]);
                chkAdopted.Checked = Convert.ToBoolean(reader["Adopted"]);
                txtDescription.Text = reader["Description"].ToString();

                // Load the image from the byte array
                if (reader["Image"] != DBNull.Value)
                {
                    byte[] imageData = (byte[])reader["Image"];
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        petImageBox.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    petImageBox.Image = Image.FromFile("D:\\PetAdoption\\cat1.jpg"); // Default image
                }
            }
            reader.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors du chargement des données : " + ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }



    private void SavePet()
    {
        string query = petId == 0
            ? "INSERT INTO pet (Name, Age, Species, Breed, Gender, Vaccinated, Adopted, Description, Image) VALUES (@Name, @Age, @Species, @Breed, @Gender, @Vaccinated, @Adopted, @Description, @Image)"
            : "UPDATE pet SET Name = @Name, Age = @Age, Species = @Species, Breed = @Breed, Gender = @Gender, Vaccinated = @Vaccinated, Adopted = @Adopted, Description = @Description, Image = @Image WHERE PetID = @PetID";

        try
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", txtName.Text);
            cmd.Parameters.AddWithValue("@Age", txtAge.Text);
            cmd.Parameters.AddWithValue("@Species", cmbSpecies.SelectedItem?.ToString());
            cmd.Parameters.AddWithValue("@Breed", txtBreed.Text);
            cmd.Parameters.AddWithValue("@Gender", cmbGender.SelectedItem?.ToString());
            cmd.Parameters.AddWithValue("@Vaccinated", chkVaccinated.Checked);
            cmd.Parameters.AddWithValue("@Adopted", chkAdopted.Checked);
            cmd.Parameters.AddWithValue("@Description", txtDescription.Text);

            // Convert the image to byte array
            byte[] imageToSave = null;
            if (!string.IsNullOrEmpty(imagePath))
            {
                imageToSave = File.ReadAllBytes(imagePath);
            }
            cmd.Parameters.AddWithValue("@Image", imageToSave);

            if (petId != 0)
            {
                cmd.Parameters.AddWithValue("@PetID", petId);
            }

            cmd.ExecuteNonQuery();
            MessageBox.Show(petId == 0 ? "Animal ajouté avec succès !" : "Animal modifié avec succès !");
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de l'enregistrement : " + ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }
}
