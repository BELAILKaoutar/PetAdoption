using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

public class ShelterAddEditForm : Form
{
    private MySqlConnection conn;
    private int shelterId; // For storing the ShelterID when editing
    private TextBox txtName, txtAddress, txtPhoneNumber, txtEmail, txtCapacity, txtCurrentAnimals, txtManagerName;
    private DateTimePicker dtpEstablishedDate;
    private Button btnSave, btnCancel;
    private DataGridView shelterGridView; // Define shelterGridView

    public ShelterAddEditForm(int shelterId = 0)
    {
        this.shelterId = shelterId;
        conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");

        // Form properties
        this.Text = shelterId == 0 ? "Ajouter un Refuge" : "Modifier un Refuge";
        this.Size = new Size(400, 450);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Initialize controls
        InitializeControls();

        // If editing an existing shelter, load its details
        if (shelterId != 0)
        {
            LoadShelterDetails(shelterId);
        }
    }

    private void InitializeControls()
    {
        // Refuge Name label and TextBox 
        Label lblName = new Label { Text = "Nom du Refuge", Location = new Point(20, 20) };
        txtName = new TextBox { Location = new Point(150, 20), Width = 200 };

        // Address label and TextBox
        Label lblAddress = new Label { Text = "Adresse", Location = new Point(20, 60) };
        txtAddress = new TextBox { Location = new Point(150, 60), Width = 200 };

        // Phone Number label and TextBox
        Label lblPhoneNumber = new Label { Text = "Téléphone", Location = new Point(20, 100) };
        txtPhoneNumber = new TextBox { Location = new Point(150, 100), Width = 200 };

        // Email label and TextBox
        Label lblEmail = new Label { Text = "Email", Location = new Point(20, 140) };
        txtEmail = new TextBox { Location = new Point(150, 140), Width = 200 };

        // Capacity label and TextBox
        Label lblCapacity = new Label { Text = "Capacité", Location = new Point(20, 180) };
        txtCapacity = new TextBox { Location = new Point(150, 180), Width = 200 };

        // Current Animals label and TextBox
        Label lblCurrentAnimals = new Label { Text = "Animaux Actuels", Location = new Point(20, 220) };
        txtCurrentAnimals = new TextBox { Location = new Point(150, 220), Width = 200 };

        // Established Date label and DateTimePicker
        Label lblEstablishedDate = new Label { Text = "Date de Création", Location = new Point(20, 260) };
        dtpEstablishedDate = new DateTimePicker { Location = new Point(150, 260), Width = 200 };

        // Manager Name label and TextBox
        Label lblManagerName = new Label { Text = "Nom du Gestionnaire", Location = new Point(20, 300) };
        txtManagerName = new TextBox { Location = new Point(150, 300), Width = 200 };


        // Save Button
        btnSave = new Button
        {
            Text = "Enregistrer",
            Location = new Point(150, 340),
            Width = 100,
            BackColor = Color.Teal,
            ForeColor = Color.White
        };
        btnSave.Click += (sender, e) => SaveShelter();

        // Cancel Button
        btnCancel = new Button
        {
            Text = "Annuler",
            Location = new Point(260, 340),
            Width = 100,
            BackColor = Color.Gray,
            ForeColor = Color.White
        };
        btnCancel.Click += (sender, e) => this.Close();

        // Shelter DataGridView
        shelterGridView = new DataGridView
        {
            Location = new Point(20, 380),
            Size = new Size(340, 200),
            Visible = shelterId == 0 // Only show if not editing
        };

        // Add controls to the form
        this.Controls.Add(lblName);
        this.Controls.Add(txtName);
        this.Controls.Add(lblAddress);
        this.Controls.Add(txtAddress);
        this.Controls.Add(lblPhoneNumber);
        this.Controls.Add(txtPhoneNumber);
        this.Controls.Add(lblEmail);
        this.Controls.Add(txtEmail);
        this.Controls.Add(lblCapacity);
        this.Controls.Add(txtCapacity);
        this.Controls.Add(lblCurrentAnimals);
        this.Controls.Add(txtCurrentAnimals);
        this.Controls.Add(lblEstablishedDate);
        this.Controls.Add(dtpEstablishedDate);
        this.Controls.Add(lblManagerName);
        this.Controls.Add(txtManagerName);
        this.Controls.Add(btnSave);
        this.Controls.Add(btnCancel);
        //this.Controls.Add(shelterGridView);
    }

    private void LoadShelterDetails(int shelterId)
    {
        try
        {
            conn.Open();
            string query = "SELECT * FROM shelter WHERE ShelterID = @ShelterID";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ShelterID", shelterId);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                txtName.Text = reader["Name"].ToString();
                txtAddress.Text = reader["Address"].ToString();
                txtPhoneNumber.Text = reader["PhoneNumber"].ToString();
                txtEmail.Text = reader["Email"].ToString();
                txtCapacity.Text = reader["Capacity"].ToString();
                txtCurrentAnimals.Text = reader["CurrentAnimals"].ToString();
                dtpEstablishedDate.Value = Convert.ToDateTime(reader["EstablishedDate"]);
                txtManagerName.Text = reader["ManagerName"].ToString();
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

    private void RefreshShelterList()
    {
        try
        {
            conn.Open();
            string query = "SELECT ShelterID, Name, Address, PhoneNumber, Email, Capacity, CurrentAnimals, EstablishedDate, ManagerName FROM shelter";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable shelterTable = new DataTable();
            adapter.Fill(shelterTable);

            shelterGridView.DataSource = shelterTable;
            shelterGridView.Columns["ShelterID"].Visible = false;
            shelterGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de l'actualisation des données : " + ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }

    private void SaveShelter()
    {
        string query = shelterId == 0
            ? "INSERT INTO shelter (Name, Address, PhoneNumber, Email, Capacity, CurrentAnimals, EstablishedDate, ManagerName) VALUES (@Name, @Address, @PhoneNumber, @Email, @Capacity, @CurrentAnimals, @EstablishedDate, @ManagerName)"
            : "UPDATE shelter SET Name = @Name, Address = @Address, PhoneNumber = @PhoneNumber, Email = @Email, Capacity = @Capacity, CurrentAnimals = @CurrentAnimals, EstablishedDate = @EstablishedDate, ManagerName = @ManagerName WHERE ShelterID = @ShelterID";

        try
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", txtName.Text);
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text);
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@Capacity", txtCapacity.Text);
            cmd.Parameters.AddWithValue("@CurrentAnimals", txtCurrentAnimals.Text);
            cmd.Parameters.AddWithValue("@EstablishedDate", dtpEstablishedDate.Value);
            cmd.Parameters.AddWithValue("@ManagerName", txtManagerName.Text);

            if (shelterId != 0)
                cmd.Parameters.AddWithValue("@ShelterID", shelterId);

            cmd.ExecuteNonQuery();
            MessageBox.Show(shelterId == 0 ? "Refuge ajouté avec succès !" : "Refuge modifié avec succès !");
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de l'enregistrement des données : " + ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }
}
