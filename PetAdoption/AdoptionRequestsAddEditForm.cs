using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

public class AdoptionRequestAddEditForm : Form
{
    private MySqlConnection conn;
    private int requestId;
    private TextBox txtAdopterName;
    private TextBox txtPetName;
    private DateTimePicker dtpRequestDate;
    private ComboBox cmbStatus;
    private Button btnSave;
    private Button btnCancel;

    public AdoptionRequestAddEditForm()
    {
        conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");

        // Form properties
        this.Text = "Ajouter une Demande d'Adoption";
        this.Size = new Size(400, 300);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.LightBlue;

        // Initialize controls
        InitializeControls();
    }

    public AdoptionRequestAddEditForm(int requestId)
    {
        conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");
        this.requestId = requestId;

        // Form properties
        this.Text = "Modifier la Demande d'Adoption";
        this.Size = new Size(400, 300);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.LightBlue;

        // Initialize controls
        InitializeControls();
        LoadRequestDetails();
    }

    private void InitializeControls()
    {
        // Adopter Name
        Label lblAdopterName = new Label
        {
            Text = "Nom Adoptant:",
            Location = new Point(20, 20),
            AutoSize = true
        };
        this.Controls.Add(lblAdopterName);

        txtAdopterName = new TextBox
        {
            Location = new Point(150, 20),
            Width = 200
        };
        this.Controls.Add(txtAdopterName);

        // Pet Name
        Label lblPetName = new Label
        {
            Text = "Nom de l'Animal:",
            Location = new Point(20, 60),
            AutoSize = true
        };
        this.Controls.Add(lblPetName);

        txtPetName = new TextBox
        {
            Location = new Point(150, 60),
            Width = 200
        };
        this.Controls.Add(txtPetName);

        // Request Date
        Label lblRequestDate = new Label
        {
            Text = "Date de la Demande:",
            Location = new Point(20, 100),
            AutoSize = true
        };
        this.Controls.Add(lblRequestDate);

        dtpRequestDate = new DateTimePicker
        {
            Location = new Point(150, 100),
            Format = DateTimePickerFormat.Short,
            Width = 200
        };
        this.Controls.Add(dtpRequestDate);

        // Status
        Label lblStatus = new Label
        {
            Text = "Statut:",
            Location = new Point(20, 140),
            AutoSize = true
        };
        this.Controls.Add(lblStatus);

        cmbStatus = new ComboBox
        {
            Location = new Point(150, 140),
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cmbStatus.Items.Add("En attente");
        cmbStatus.Items.Add("Approuvé");
        cmbStatus.Items.Add("Rejeté");
        this.Controls.Add(cmbStatus);

        // Save button
        btnSave = new Button
        {
            Text = "Enregistrer",
            Location = new Point(150, 180),
            Width = 100,
            BackColor = Color.CadetBlue,
            ForeColor = Color.White
        };
        btnSave.Click += (sender, e) => SaveRequest();
        this.Controls.Add(btnSave);

        // Cancel button
        btnCancel = new Button
        {
            Text = "Annuler",
            Location = new Point(260, 180),
            Width = 100,
            BackColor = Color.Gray,
            ForeColor = Color.White
        };
        btnCancel.Click += (sender, e) => this.Close();
        this.Controls.Add(btnCancel);
    }

    private void LoadRequestDetails()
    {
        try
        {
            conn.Open();

            // Récupérer les détails de la demande
            string query = "SELECT RequestID, AdopterID, PetID, RequestDate, Status FROM adoption_request WHERE RequestID = @RequestID";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@RequestID", requestId);

            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                // Récupérer les IDs
                int adopterId = Convert.ToInt32(reader["AdopterID"]);
                int petId = Convert.ToInt32(reader["PetID"]);
                dtpRequestDate.Value = Convert.ToDateTime(reader["RequestDate"]);
                cmbStatus.SelectedItem = reader["Status"].ToString();

                // Fermer le reader pour effectuer des requêtes supplémentaires
                reader.Close();

                // Récupérer le nom de l'adoptant à partir de l'ID
                string adopterName = GetAdopterName(adopterId);

                // Récupérer le nom de l'animal à partir de l'ID
                string petName = GetPetName(petId);

                // Afficher les noms dans les champs de texte
                txtAdopterName.Text = adopterName;
                txtPetName.Text = petName;
            }
            else
            {
                MessageBox.Show("Demande d'adoption introuvable !");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors du chargement des détails de la demande : " + ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }

    private string GetAdopterName(int adopterId)
    {
        string adopterName = string.Empty;
        try
        {
            string query = "SELECT AdopterName FROM adopter WHERE AdopterID = @AdopterID";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AdopterID", adopterId);

            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                adopterName = result.ToString();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la récupération du nom de l'adoptant : " + ex.Message);
        }

        return adopterName;
    }

    private string GetPetName(int petId)
    {
        string petName = string.Empty;
        try
        {
            string query = "SELECT PetName FROM pet WHERE PetID = @PetID";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PetID", petId);

            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                petName = result.ToString();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la récupération du nom de l'animal : " + ex.Message);
        }

        return petName;
    }


    private void SaveRequest()
    {
        string query;

        if (requestId == 0)
        {
            // New request
            query = "INSERT INTO adoptionrequest (AdopterName, PetName, RequestDate, Status) VALUES (@AdopterName, @PetName, @RequestDate, @Status)";
        }
        else
        {
            // Edit existing request
            query = "UPDATE adoptionrequest SET AdopterName = @AdopterName, PetName = @PetName, RequestDate = @RequestDate, Status = @Status WHERE RequestID = @RequestID";
        }

        try
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AdopterName", txtAdopterName.Text);
            cmd.Parameters.AddWithValue("@PetName", txtPetName.Text);
            cmd.Parameters.AddWithValue("@RequestDate", dtpRequestDate.Value);
            cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());

            if (requestId != 0)
                cmd.Parameters.AddWithValue("@RequestID", requestId);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Demande d'adoption enregistrée avec succès !");
                this.Close();
            }
            else
            {
                MessageBox.Show("Erreur lors de l'enregistrement de la demande.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de l'enregistrement de la demande : " + ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }

}
