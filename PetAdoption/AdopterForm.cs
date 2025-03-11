using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
public class AdopterForm : Form
{
    private DataGridView dgvAdopters;
    private MySqlConnection conn;

    public AdopterForm()
    {
        conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");

        // Form properties
        this.Text = "Gestion des Adoptants";
        this.Size = new Size(600, 600);
        this.BackColor = Color.DarkCyan;
        this.StartPosition = FormStartPosition.CenterScreen;

        // Initialize controls
        InitializeControls();
        LoadAdopters();
    }

    private void InitializeControls()
    {

        // Title label
      /*  Label lblTitle = new Label
        {
            Text = "Gestion des Adoptants",
            Font = new Font("Arial", 18, FontStyle.Bold),
            ForeColor = Color.DarkBlue,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 50
        };
        this.Controls.Add(lblTitle);*/

        // DataGridView setup
        dgvAdopters = new DataGridView
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
        dgvAdopters.Columns.Add("AdopterID", "ID");
        dgvAdopters.Columns["AdopterID"].Visible = false;
        dgvAdopters.Columns.Add("user", "user");
        dgvAdopters.Columns.Add("Age", "Age");
        dgvAdopters.Columns.Add("Gender", "Gender");
        dgvAdopters.Columns.Add("Contact", "Contact");

        // Action columns
        DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn
        {
            HeaderText = "Modifier",
            Text = "Modifier",
            UseColumnTextForButtonValue = true
        };
        dgvAdopters.Columns.Add(btnEdit);

        DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
        {
            HeaderText = "Supprimer",
            Text = "Supprimer",
            UseColumnTextForButtonValue = true
        };
        dgvAdopters.Columns.Add(btnDelete);

        dgvAdopters.CellClick += OnTableButtonClick;

        this.Controls.Add(dgvAdopters);

        // Add Adopter button
        Button btnAdd = new Button
        {
            Text = "Ajouter Adoptant",
            BackColor = Color.CadetBlue,
            ForeColor = Color.White,
            Font = new System.Drawing.Font("Arial", 12),
            Width = 150,
            Location = new Point(20, 550)
        };
        btnAdd.Click += (sender, e) => OpenAddAdopterPage();
        this.Controls.Add(btnAdd);
        // Save to PDF Button
        Button btnSaveToPdf = new Button
        {
            Text = "Enregistrer en PDF",
            BackColor = Color.CadetBlue,
            ForeColor = Color.White,
            Font = new System.Drawing.Font("Arial", 12),
            Width = 150,
            Location = new Point(200, 550) // Adjust the location as needed
        };
        btnSaveToPdf.Click += (sender, e) => SaveAdoptersToPdf();
        this.Controls.Add(btnSaveToPdf);

    }
    private void SaveAdoptersToPdf()
    {
        string filePath = @"D:\PetAdoption\adopters.pdf"; // Specify the path where you want to save the PDF

        try
        {
            // Fetch adopter data from the database
            DataTable adoptersData = GetAdoptersData();

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, fs);

                document.Open();
                document.Add(new Paragraph("Liste des Adoptants"));
                document.Add(new Paragraph(" ")); // Add some space

                PdfPTable table = new PdfPTable(adoptersData.Columns.Count);
                foreach (DataColumn column in adoptersData.Columns)
                {
                    table.AddCell(column.ColumnName); // Add column headers
                }

                foreach (DataRow row in adoptersData.Rows)
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

    private DataTable GetAdoptersData()
    {
        DataTable dt = new DataTable();
        try
        {
            conn.Open();
            string query = "SELECT * FROM adopter"; // Adjust the query as needed
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





    private void LoadAdopters()
    {
        try
        {
            conn.Open();
            string query = "SELECT * FROM adopter";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dgvAdopters.Rows.Clear();

            foreach (DataRow row in dt.Rows)
            {
                // Retrieve AdoptionHistory (you can modify this logic as per the structure of AdoptionHistory)
                dgvAdopters.Rows.Add(
                    row["AdopterID"],
                    row["user"],
                    row["Age"],
                    row["Gender"],
                    row["Contact"]
                );
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading adopters: {ex.Message}");
        }
        finally
        {
            conn.Close();
        }
    }

    private void OpenAddAdopterPage()
    {
        AdopterAddEditForm addForm = new AdopterAddEditForm();
        addForm.ShowDialog();
        LoadAdopters();
    }

    private void OpenEditAdopterPage(int adopterId)
    {
        AdopterAddEditForm editForm = new AdopterAddEditForm(adopterId);
        editForm.ShowDialog();
        LoadAdopters();
    }

    private void OnTableButtonClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        {
            var column = dgvAdopters.Columns[e.ColumnIndex];

            if (column is DataGridViewButtonColumn && column.HeaderText == "Modifier")
            {
                int adopterId = Convert.ToInt32(dgvAdopters.Rows[e.RowIndex].Cells["AdopterID"].Value);
                OpenEditAdopterPage(adopterId);
            }
            else if (column is DataGridViewButtonColumn && column.HeaderText == "Supprimer")
            {
                int adopterId = Convert.ToInt32(dgvAdopters.Rows[e.RowIndex].Cells["AdopterID"].Value);
                var confirmResult = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet adoptant ?", "Confirmation", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    DeleteAdopter(adopterId);
                }
            }
        }
    }

    private void DeleteAdopter(int adopterId)
    {
        string query = "DELETE FROM adopter WHERE AdopterID = @AdopterID";

        try
        {
            // Open connection
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AdopterID", adopterId);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Adoptant supprimé avec succès !");
            }
            else
            {
                MessageBox.Show("Adoptant introuvable !");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
        }
        finally
        {
            // Ensure the connection is closed
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        // Reload the adopters after deleting
        LoadAdopters();
    }


    /*[STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new AdopterForm());
    }*/
}
