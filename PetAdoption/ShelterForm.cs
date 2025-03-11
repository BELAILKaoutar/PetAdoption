using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
public class ShelterForm : Form
{
    private DataGridView dgvShelters;
    private MySqlConnection conn;

    public ShelterForm()
    {
        conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");

        // Form properties
        this.Text = "Gestion des Refuges";
        this.Size = new Size(900, 600);
        this.BackColor = Color.DarkCyan;
        this.StartPosition = FormStartPosition.CenterScreen;

        // Initialize controls
        InitializeControls();
        LoadShelters();
    }

    private void InitializeControls()
    {
        // DataGridView setup
        dgvShelters = new DataGridView
        {
            Size = new Size(1200, 400), // Ajustez la taille si nécessaire
            Location = new Point(20, 70),
            BackgroundColor = Color.White,
            ForeColor = Color.Black,
            ColumnHeadersDefaultCellStyle = { BackColor = Color.Teal, ForeColor = Color.White },
            EnableHeadersVisualStyles = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };

        // Add columns
        dgvShelters.Columns.Add("ShelterID", "ID");
        dgvShelters.Columns["ShelterID"].Visible = false;

        dgvShelters.Columns.Add("Name", "Nom du Refuge");
        dgvShelters.Columns.Add("Address", "Adresse");
        dgvShelters.Columns.Add("PhoneNumber", "Téléphone");
        dgvShelters.Columns.Add("Email", "Email");
        dgvShelters.Columns.Add("Capacity", "Capacité");
        dgvShelters.Columns.Add("CurrentAnimals", "Animaux Actuels");
        dgvShelters.Columns.Add("EstablishedDate", "Date de Création");
        dgvShelters.Columns.Add("ManagerName", "Nom du Gestionnaire");

        // Set header text alignment to right
        foreach (DataGridViewColumn column in dgvShelters.Columns)
        {
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;  // Align header
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // Auto size columns based on content
        }

        // Action columns
        DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn
        {
            HeaderText = "Modifier",
            Text = "Modifier",
            UseColumnTextForButtonValue = true
        };
        dgvShelters.Columns.Add(btnEdit);

        DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
        {
            HeaderText = "Supprimer",
            Text = "Supprimer",
            UseColumnTextForButtonValue = true
        };
        dgvShelters.Columns.Add(btnDelete);

        dgvShelters.CellClick += OnTableButtonClick;

        this.Controls.Add(dgvShelters);

        // Add Shelter button
        Button btnAdd = new Button
        {
            Text = "Ajouter Refuge",
            BackColor = Color.Teal,
            ForeColor = Color.White,
            Font = new System.Drawing.Font("Arial", 12),
            Width = 150,
            Location = new Point(20, 500)
        };
        btnAdd.Click += (sender, e) => OpenAddShelterPage();
        this.Controls.Add(btnAdd);

        // Save to PDF Button
        Button btnSaveToPdf = new Button
        {
            Text = "Enregistrer en PDF",
            BackColor = Color.Teal,
            ForeColor = Color.White,
            Font = new System.Drawing.Font("Arial", 12),
            Width = 150,
            Location = new Point(200, 500)
        };
        btnSaveToPdf.Click += (sender, e) => SaveSheltersToPdf();
        this.Controls.Add(btnSaveToPdf);
    }


    private void SaveSheltersToPdf()
    {
        string filePath = @"D:\PetAdoption\shelters.pdf"; // Specify the path where you want to save the PDF

        try
        {
            // Fetch shelter data from the database
            DataTable sheltersData = GetSheltersData();

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, fs);

                document.Open();
                document.Add(new Paragraph("Liste des Refuges"));
                document.Add(new Paragraph(" ")); // Add some space

                PdfPTable table = new PdfPTable(sheltersData.Columns.Count);
                foreach (DataColumn column in sheltersData.Columns)
                {
                    table.AddCell(column.ColumnName); // Add column headers
                }

                foreach (DataRow row in sheltersData.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        table.AddCell(item.ToString()); // Add cell data for non-image items
                    }
                }

                document.Add(table);
                document.Close();
            }

            MessageBox.Show("PDF sauvegardé avec succès à : " + filePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la sauvegarde en PDF : " + ex.Message);
        }
    }

    private DataTable GetSheltersData()
    {
        DataTable dt = new DataTable();
        try
        {
            conn.Open();
            string query = "SELECT * FROM shelter"; // Adjust the query as needed
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la récupération des données : " + ex.Message);
        }
        finally
        {
            conn.Close();
        }
        return dt;
    }




    private void LoadShelters()
    {
        try
        {
            conn.Open();
            string query = "SELECT * FROM shelter";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dgvShelters.Rows.Clear();
            foreach (DataRow row in dt.Rows)
            {
                dgvShelters.Rows.Add(
                    row["ShelterID"],
                    row["Name"],
                    row["Address"],
                    row["PhoneNumber"],
                    row["Email"],
                    row["Capacity"],
                    row["CurrentAnimals"],
                    Convert.ToDateTime(row["EstablishedDate"]).ToString("yyyy-MM-dd"),
                    row["ManagerName"]
                );
            }
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

    private void OpenAddShelterPage()
    {
        ShelterAddEditForm addForm = new ShelterAddEditForm();
        addForm.ShowDialog();
        LoadShelters();
    }

    private void OpenEditShelterPage(int shelterId)
    {
        ShelterAddEditForm editForm = new ShelterAddEditForm(shelterId);
        editForm.ShowDialog();
        LoadShelters();
    }


    private void OnTableButtonClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        {
            // Get the column that was clicked
            var column = dgvShelters.Columns[e.ColumnIndex];

            // Check if the clicked column is the "Modifier" button column
            if (column is DataGridViewButtonColumn && column.HeaderText == "Modifier")
            {
                // Retrieve the ShelterID from the clicked row
                var shelterIdCell = dgvShelters.Rows[e.RowIndex].Cells["ShelterID"];
                if (shelterIdCell == null || shelterIdCell.Value == null)
                {
                    MessageBox.Show("Erreur : ID du refuge non valide.");
                    return;
                }

                int shelterId = Convert.ToInt32(shelterIdCell.Value);

                // Open the edit form (you can customize this to fit your form for editing shelters)
                OpenEditShelterPage(shelterId);
            }

            // Check if the clicked column is the "Supprimer" button column
            else if (column is DataGridViewButtonColumn && column.HeaderText == "Supprimer")
            {
                // Retrieve the ShelterID from the clicked row
                var shelterIdCell = dgvShelters.Rows[e.RowIndex].Cells["ShelterID"];
                if (shelterIdCell == null || shelterIdCell.Value == null)
                {
                    MessageBox.Show("Erreur : ID du refuge non valide.");
                    return;
                }

                int shelterId = Convert.ToInt32(shelterIdCell.Value);

                // Show the delete confirmation dialog
                var confirmResult = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce refuge ?", "Confirmation", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    // If confirmed, delete the shelter
                    DeleteShelter(shelterId);
                }
            }
        }
    }





    private void DeleteShelter(int shelterId)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet"))
            {
                conn.Open();

                string query = "DELETE FROM shelter WHERE ShelterID = @ShelterID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ShelterID", shelterId);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Refuge supprimé avec succès !");
                    LoadShelters(); // Reload the shelters list after deletion
                }
                else
                {
                    MessageBox.Show("Erreur : Aucun refuge trouvé avec cet ID.");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
        }
    }
}
  /*     [STAThread]
 public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new ShelterForm());
    }
}*/


