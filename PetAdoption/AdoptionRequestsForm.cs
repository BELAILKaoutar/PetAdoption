using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using PetAdoption;
using iTextSharp.text;
using iTextSharp.text.pdf;
public class AdoptionRequestsForm : Form
{
    private DataGridView dgvAdoptionRequests;
    private MySqlConnection conn;

    public AdoptionRequestsForm()
    {
        conn = new MySqlConnection("server=localhost;port=3306;username=root;password=;database=pet");

        // Form properties
        this.Text = "Gestion des Demandes d'Adoption";
        this.Size = new Size(600, 600);
        this.BackColor = Color.DarkCyan;
        this.StartPosition = FormStartPosition.CenterScreen;

        // Initialize controls
        InitializeControls();
        LoadAdoptionRequests();
    }

    private void InitializeControls()
    {


        // DataGridView setup
        dgvAdoptionRequests = new DataGridView
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
        dgvAdoptionRequests.Columns.Add("RequestID", "ID");
        dgvAdoptionRequests.Columns["RequestID"].Visible = false;
        dgvAdoptionRequests.Columns.Add("AdopterName", "Nom Adoptant");
        dgvAdoptionRequests.Columns.Add("PetName", "Nom de l'Animal");
        dgvAdoptionRequests.Columns.Add("RequestDate", "Date de la Demande");
        dgvAdoptionRequests.Columns.Add("Status", "Statut");

        // Action columns
        DataGridViewButtonColumn btnApprove = new DataGridViewButtonColumn
        {
            HeaderText = "Approuver",
            Text = "Approuver",
            UseColumnTextForButtonValue = true
        };
        dgvAdoptionRequests.Columns.Add(btnApprove);

        DataGridViewButtonColumn btnReject = new DataGridViewButtonColumn
        {
            HeaderText = "Rejeter",
            Text = "Rejeter",
            UseColumnTextForButtonValue = true
        };
        dgvAdoptionRequests.Columns.Add(btnReject);

        dgvAdoptionRequests.CellClick += OnTableButtonClick;

        this.Controls.Add(dgvAdoptionRequests);

        // Add AdoptionRequest button
        Button btnAddRequest = new Button
        {
            Text = "Ajouter Demande d'Adoption",
            BackColor = Color.CadetBlue,
            ForeColor = Color.White,
            Font = new System.Drawing.Font("Arial", 12),
            Width = 200,
            Location = new Point(20, 550)
        };
        btnAddRequest.Click += (sender, e) => OpenAddAdoptionRequestPage();
        this.Controls.Add(btnAddRequest);
        // Save to PDF Button
        Button btnSaveToPdf = new Button
        {
            Text = "Enregistrer en PDF",
            BackColor = Color.CadetBlue,
            ForeColor = Color.White,
            Font = new System.Drawing.Font("Arial", 12),
            Width = 200,
            Location = new Point(250, 550) // Adjust the location as needed
        };
        btnSaveToPdf.Click += (sender, e) => SaveAdoptionRequestsToPdf();
        this.Controls.Add(btnSaveToPdf);

    }
    private void SaveAdoptionRequestsToPdf()
    {
        string filePath = @"D:\PetAdoption\adoptionRequests.pdf"; // Specify the path where you want to save the PDF

        try
        {
            // Fetch adoption request data from the database
            DataTable adoptionRequestsData = GetAdoptionRequestsData();

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, fs);

                document.Open();
                document.Add(new Paragraph("Liste des Demandes d'Adoption"));
                document.Add(new Paragraph(" ")); // Add some space

                PdfPTable table = new PdfPTable(adoptionRequestsData.Columns.Count);
                foreach (DataColumn column in adoptionRequestsData.Columns)
                {
                    table.AddCell(column.ColumnName); // Add column headers
                }

                foreach (DataRow row in adoptionRequestsData.Rows)
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

    private DataTable GetAdoptionRequestsData()
    {
        DataTable dt = new DataTable();
        try
        {
            conn.Open();
            string query = "SELECT * FROM adoptionrequest"; // Adjust the query as needed
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



    private void LoadAdoptionRequests()
    {
        try
        {
            conn.Open();
            string query = @"SELECT 
                            ar.RequestID, 
                            a.user AS AdopterName, 
                            p.Name AS PetName, 
                            ar.RequestDate, 
                            ar.Status
                         FROM adoptionrequest ar
                         JOIN adopter a ON ar.AdopterID = a.AdopterID
                         JOIN pet p ON ar.PetID = p.PetID";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dgvAdoptionRequests.Rows.Clear();

            foreach (DataRow row in dt.Rows)
            {
                dgvAdoptionRequests.Rows.Add(
                    row["RequestID"],
                    row["AdopterName"],  // Display adopter's name
                    row["PetName"],      // Display pet's name
                    row["RequestDate"],
                    row["Status"]
                );
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading adoption requests: {ex.Message}");
        }
        finally
        {
            conn.Close();
        }
    }


    private void OpenAddAdoptionRequestPage()
    {
        AdoptionRequestAddEditForm addForm = new AdoptionRequestAddEditForm();
        addForm.ShowDialog();
        LoadAdoptionRequests();
    }

    private void OpenEditAdoptionRequestPage(int requestId)
    {
        AdoptionRequestAddEditForm editForm = new AdoptionRequestAddEditForm(requestId);
        editForm.ShowDialog();
        LoadAdoptionRequests();
    }

    private void OnTableButtonClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        {
            var column = dgvAdoptionRequests.Columns[e.ColumnIndex];

            // Check if the clicked column is one of the buttons
            if (column is DataGridViewButtonColumn)
            {
                int requestId = Convert.ToInt32(dgvAdoptionRequests.Rows[e.RowIndex].Cells["RequestID"].Value);

                if (column.HeaderText == "Approuver")  // If the "Approuver" button was clicked
                {
                    ApproveAdoptionRequest(requestId);
                }
                else if (column.HeaderText == "Rejeter")  // If the "Rejeter" button was clicked
                {
                    RejectAdoptionRequest(requestId);
                }
            }
        }
    }
    private void ApproveAdoptionRequest(int requestId)
    {
        string query = "UPDATE adoptionrequest SET Status = 'Approved' WHERE RequestID = @RequestID";

        try
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@RequestID", requestId);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Demande d'adoption approuvée avec succès !");
            }
            else
            {
                MessageBox.Show("Demande d'adoption introuvable !");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de l'approbation : " + ex.Message);
        }
        finally
        {
            conn.Close();
        }

        LoadAdoptionRequests();
    }

    private void RejectAdoptionRequest(int requestId)
    {
        string query = "UPDATE adoptionrequest SET Status = 'Rejected' WHERE RequestID = @RequestID";

        try
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@RequestID", requestId);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Demande d'adoption rejetée avec succès !");
            }
            else
            {
                MessageBox.Show("Demande d'adoption introuvable !");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors du rejet : " + ex.Message);
        }
        finally
        {
            conn.Close();
        }

        LoadAdoptionRequests();
    }



    /* [STAThread]
     public static void Main()
     {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new AdoptionRequestsForm());
     }*/
}
