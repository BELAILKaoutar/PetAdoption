using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharpImage = iTextSharp.text.Image;
public class PetForm : Form
{
    private DataGridView dgvPets;
    private MySqlConnection conn;
    private TextBox txtSearch;
    private Button btnSearch;
    private Button btnSaveToPdf;
    public PetForm()
    {
        conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");

        // Form properties
        this.Text = "Gestion des Animaux";
        this.Size = new Size(1200, 600);
        this.BackColor = Color.DarkCyan;
        this.StartPosition = FormStartPosition.CenterScreen;

        // Initialize controls
        InitializeControls();
        LoadPets(); // Load all pets initially
    }

    private void InitializeControls()
    {

        // Search Box
        txtSearch = new TextBox
        {
            Font = new System.Drawing.Font("Arial", 12),
            Width = 200,
            Location = new Point(20, 30)
        };
        this.Controls.Add(txtSearch);

        // Search Button
        btnSearch = new Button
        {
            Text = "Chercher",
            Font = new System.Drawing.Font("Arial", 12),
            BackColor = Color.CadetBlue,
            ForeColor = Color.White,
            Location = new Point(230, 30),
            Width = 100
        };
        btnSearch.Click += BtnSearch_Click;
        this.Controls.Add(btnSearch);

        // DataGridView setup
        dgvPets = new DataGridView
        {
            Size = new Size(860, 400),
            Location = new Point(20, 70),
            BackgroundColor = Color.White,
            ForeColor = Color.Black,
            ColumnHeadersDefaultCellStyle = { BackColor = Color.Teal, ForeColor = Color.Black },
            EnableHeadersVisualStyles = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };

        // Add columns
        dgvPets.Columns.Add("PetID", "ID");
        dgvPets.Columns["PetID"].Visible = false;
        dgvPets.Columns.Add("Name", "Nom");
        dgvPets.Columns.Add("Age", "Âge");
        dgvPets.Columns.Add("Species", "Espèce");
        dgvPets.Columns.Add("Breed", "Race");
        dgvPets.Columns.Add("Gender", "Sexe");
        dgvPets.Columns.Add("Vaccinated", "Vacciné");
        dgvPets.Columns.Add("Adopted", "Adopté");
        dgvPets.Columns.Add("Description", "Description");
        DataGridViewImageColumn imgColumn = new DataGridViewImageColumn
        {
            HeaderText = "Image",
            Name = "PetImage",
            ImageLayout = DataGridViewImageCellLayout.Zoom, // Ensures the image fits in the cell
            Width = 100 // Adjust the size as needed
        };
        dgvPets.Columns.Add(imgColumn);

        // Action columns
        DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn
        {
            HeaderText = "Modifier",
            Text = "Modifier",
            UseColumnTextForButtonValue = true
        };
        dgvPets.Columns.Add(btnEdit);

        DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
        {
            HeaderText = "Supprimer",
            Text = "Supprimer",
            UseColumnTextForButtonValue = true
        };
        dgvPets.Columns.Add(btnDelete);

        dgvPets.CellClick += OnTableButtonClick;

        this.Controls.Add(dgvPets);

        // Add Pet button
        Button btnAdd = new Button
        {
            Text = "Ajouter Animal",
            BackColor = Color.CadetBlue,
            ForeColor = Color.White,
            Font = new System.Drawing.Font("Arial", 12),
            Width = 150,
            Location = new Point(20, 550)
        };
        btnAdd.Click += (sender, e) => OpenAddPetPage();
        this.Controls.Add(btnAdd);
        // Save to PDF button
        btnSaveToPdf = new Button
        {
            Text = "Sauvegarder en PDF",
            BackColor = Color.CadetBlue,
            ForeColor = Color.White,
            Font = new System.Drawing.Font("Arial", 12), 
            Width = 150,
            Location = new Point(200, 550) 
        };
        btnSaveToPdf.Click += BtnSaveToPdf_Click;
        this.Controls.Add(btnSaveToPdf);
    }
    private void BtnSaveToPdf_Click(object sender, EventArgs e)
    {
        SavePetsToPdf();
    }

    private void SavePetsToPdf()
    {
        string filePath = @"D:\PetAdoption\pets.pdf"; //path of file PDF

        try
        {
            DataTable petsData = GetPetsData();

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, fs);

                document.Open();
                document.Add(new Paragraph("Liste des Animaux"));
                document.Add(new Paragraph(" ")); 

                PdfPTable table = new PdfPTable(petsData.Columns.Count);
                foreach (DataColumn column in petsData.Columns)
                {
                    table.AddCell(column.ColumnName); 
                }

                foreach (DataRow row in petsData.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        if (item is byte[] imageData)
                        {
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(ms.ToArray());
                                pdfImage.ScaleToFit(40, 40); 
                                PdfPCell cell = new PdfPCell(pdfImage);
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                table.AddCell(cell); 
                            }
                        }
                        else
                        {
                            table.AddCell(item.ToString()); 
                        }
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


    private DataTable GetPetsData()
    {
        DataTable dt = new DataTable();
        try
        {
            conn.Open();
            string query = "SELECT * FROM pet";
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

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        string searchName = txtSearch.Text; 

        LoadPets(searchName);
    }

    private void LoadPets(string searchName = "")
    {
        string placeholderImagePath = @"D:\PetAdoption\cat2.jpg"; // Default image path

        try
        {
            conn.Open();
            string query = "SELECT * FROM pet";

            // Apply search filter if searchName or searchSpecies is provided
            bool hasNameFilter = !string.IsNullOrEmpty(searchName);

            if (hasNameFilter )
            {
                query += " WHERE";

                // Add the filters
                if (hasNameFilter)
                    query += " Name LIKE @Name";
            }

            MySqlCommand cmd = new MySqlCommand(query, conn);

            // Add parameters for both searchName and searchSpecies
            if (hasNameFilter)
            {
                cmd.Parameters.AddWithValue("@Name", $"%{searchName}%");
            }


            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dgvPets.Rows.Clear();

            foreach (DataRow row in dt.Rows)
            {
                // Retrieve the BLOB image data from the database
                byte[] imageBytes = row["Image"] as byte[];

                if (imageBytes != null && imageBytes.Length > 0)
                {
                    try
                    {
                        // Convert the BLOB to an Image
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            System.Drawing.Image petImage = System.Drawing.Image.FromStream(ms);

                            // Add the pet details to DataGridView
                            dgvPets.Rows.Add(
                                row["PetID"],
                                row["Name"],
                                row["Age"],
                                row["Species"],
                                row["Breed"],
                                row["Gender"],
                                Convert.ToBoolean(row["Vaccinated"]) ? "Oui" : "Non",
                                Convert.ToBoolean(row["Adopted"]) ? "Oui" : "Non",
                                row["Description"],
                                petImage
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error converting image: {ex.Message}");
                        dgvPets.Rows.Add(
                            row["PetID"],
                            row["Name"],
                            row["Age"],
                            row["Species"],
                            row["Breed"],
                            row["Gender"],
                            Convert.ToBoolean(row["Vaccinated"]) ? "Oui" : "Non",
                            Convert.ToBoolean(row["Adopted"]) ? "Oui" : "Non",
                            row["Description"],
                            LoadPlaceholderImage(placeholderImagePath)
                        );
                    }
                }
                else
                {
                    // If no image data is found, use the placeholder
                    dgvPets.Rows.Add(
                        row["PetID"],
                        row["Name"],
                        row["Age"],
                        row["Species"],
                        row["Breed"],
                        row["Gender"],
                        Convert.ToBoolean(row["Vaccinated"]) ? "Oui" : "Non",
                        Convert.ToBoolean(row["Adopted"]) ? "Oui" : "Non",
                        row["Description"],
                        LoadPlaceholderImage(placeholderImagePath)
                    );
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading pets: {ex.Message}");
        }
        finally
        {
            conn.Close();
        }
    }

    private System.Drawing.Image LoadPlaceholderImage(string placeholderImagePath)
    {
        if (File.Exists(placeholderImagePath))
        {
            return System.Drawing.Image.FromFile(placeholderImagePath);
        }
        else
        {
            MessageBox.Show($"Placeholder image not found: {placeholderImagePath}");
            return new System.Drawing.Bitmap(100, 100); // Default placeholder image
        }
    }

    private void OpenAddPetPage()
    {
        PetAddEditForm addForm = new PetAddEditForm();
        addForm.ShowDialog();
        LoadPets();
    }

    private void OpenEditPetPage(int petId)
    {
        PetAddEditForm editForm = new PetAddEditForm(petId);
        editForm.ShowDialog();
        LoadPets();
    }

    private void OnTableButtonClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        {
            var column = dgvPets.Columns[e.ColumnIndex];

            if (column is DataGridViewButtonColumn && column.HeaderText == "Modifier")
            {
                int petId = Convert.ToInt32(dgvPets.Rows[e.RowIndex].Cells["PetID"].Value);
                OpenEditPetPage(petId);
            }
            else if (column is DataGridViewButtonColumn && column.HeaderText == "Supprimer")
            {
                int petId = Convert.ToInt32(dgvPets.Rows[e.RowIndex].Cells["PetID"].Value);
                var confirmResult = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet animal ?", "Confirmation", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    DeletePet(petId);
                }
            }
        }
    }

    private void DeletePet(int petId)
    {
        try
        {
            string query = "DELETE FROM pet WHERE PetID = @PetID";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@PetID", petId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Animal supprimé avec succès !");
                LoadPets();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }



    /*[STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new PetForm());
    }*/
}
