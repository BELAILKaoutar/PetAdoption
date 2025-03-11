using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

public class AdopterAddEditForm : Form
{
    private TextBox txtName, txtAge, txtContact;
    private ComboBox cmbGender;
    private Button btnSave, btnCancel;
    private Label lblName, lblAge, lblGender, lblContact;
    private MySqlConnection conn;
    private int adopterId;

    public AdopterAddEditForm(int adopterId = 0)
    {
        this.adopterId = adopterId;
        conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");

        // Form properties
        this.Text = adopterId == 0 ? "Ajouter un Adoptant" : "Modifier un Adoptant";
        this.Size = new Size(400, 300);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Initialize controls
        InitializeControls();

        // If editing, load adopter details
        if (adopterId != 0)
        {
            LoadAdopterDetails(adopterId);
        }
    }

    private void InitializeControls()
    {
        // Name Label and TextBox
        lblName = new Label { Text = "Nom", Location = new Point(20, 20) };
        txtName = new TextBox { Location = new Point(150, 20), Width = 200 };

        // Age Label and TextBox
        lblAge = new Label { Text = "Âge", Location = new Point(20, 60) };
        txtAge = new TextBox { Location = new Point(150, 60), Width = 200 };

        // Gender Label and ComboBox
        lblGender = new Label { Text = "Sexe", Location = new Point(20, 100) };
        cmbGender = new ComboBox { Location = new Point(150, 100), Width = 200 };
        cmbGender.Items.AddRange(new string[] { "Mâle", "Femelle" });

        // Contact Label and TextBox
        lblContact = new Label { Text = "Contact", Location = new Point(20, 140) };
        txtContact = new TextBox { Location = new Point(150, 140), Width = 200 };

        // Save Button
        btnSave = new Button
        {
            Text = "Enregistrer",
            Location = new Point(150, 200),
            Width = 100,
            BackColor = Color.Teal,
            ForeColor = Color.White
        };
        btnSave.Click += (sender, e) => SaveAdopter();

        // Cancel Button
        btnCancel = new Button
        {
            Text = "Annuler",
            Location = new Point(260, 200),
            Width = 100,
            BackColor = Color.Gray,
            ForeColor = Color.White
        };
        btnCancel.Click += (sender, e) => this.Close();

        // Add controls to form
        this.Controls.Add(lblName);
        this.Controls.Add(txtName);
        this.Controls.Add(lblAge);
        this.Controls.Add(txtAge);
        this.Controls.Add(lblGender);
        this.Controls.Add(cmbGender);
        this.Controls.Add(lblContact);
        this.Controls.Add(txtContact);
        this.Controls.Add(btnSave);
        this.Controls.Add(btnCancel);
    }

    private void LoadAdopterDetails(int adopterId)
    {
        try
        {
            conn.Open();
            string query = "SELECT * FROM adopter WHERE AdopterID = @AdopterID";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AdopterID", adopterId);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                txtName.Text = reader["user"].ToString();
                txtAge.Text = reader["Age"].ToString();
                cmbGender.SelectedItem = reader["Gender"].ToString();
                txtContact.Text = reader["Contact"].ToString();
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

    private void SaveAdopter()
    {
        string query = adopterId == 0
            ? "INSERT INTO adopter (user, Age, Gender, Contact) VALUES (@Name, @Age, @Gender, @Contact)"
            : "UPDATE adopter SET user = @user, Age = @Age, Gender = @Gender, Contact = @Contact WHERE AdopterID = @AdopterID";

        try
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@user", txtName.Text);
            cmd.Parameters.AddWithValue("@Age", txtAge.Text);

            // Handle gender: Ensure it has a value or use a default
            string gender = cmbGender.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Veuillez sélectionner le sexe de l'adoptant.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop execution if gender is empty
            }
            cmd.Parameters.AddWithValue("@Gender", gender);

            cmd.Parameters.AddWithValue("@Contact", txtContact.Text);

            if (adopterId != 0)
            {
                cmd.Parameters.AddWithValue("@AdopterID", adopterId);
            }

            cmd.ExecuteNonQuery();
            MessageBox.Show(adopterId == 0 ? "Adoptant ajouté avec succès !" : "Adoptant modifié avec succès !");
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
