namespace PetAdoption
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            button1 = new Button();
            label1 = new Label();
            txtOccupation = new TextBox();
            txtPhysicalAddress = new TextBox();
            txtEmailAddress = new TextBox();
            txtPhoneNumber = new TextBox();
            txtDateOfBirth = new TextBox();
            txtPassword = new TextBox();
            txtUser = new TextBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            txt = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.DarkCyan;
            panel1.BackgroundImage = Properties.Resources.dog_logo;
            panel1.Controls.Add(button1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtOccupation);
            panel1.Controls.Add(txtPhysicalAddress);
            panel1.Controls.Add(txtEmailAddress);
            panel1.Controls.Add(txtPhoneNumber);
            panel1.Controls.Add(txtDateOfBirth);
            panel1.Controls.Add(txtPassword);
            panel1.Controls.Add(txtUser);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txt);
            panel1.Location = new Point(1, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(803, 463);
            panel1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(553, 332);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 15;
            button1.Text = "Register";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(474, 124);
            label1.Name = "label1";
            label1.Size = new Size(79, 15);
            label1.TabIndex = 14;
            label1.Text = "Email address";
            // 
            // txtOccupation
            // 
            txtOccupation.Location = new Point(583, 55);
            txtOccupation.Name = "txtOccupation";
            txtOccupation.Size = new Size(100, 23);
            txtOccupation.TabIndex = 13;
            // 
            // txtPhysicalAddress
            // 
            txtPhysicalAddress.Location = new Point(583, 193);
            txtPhysicalAddress.Name = "txtPhysicalAddress";
            txtPhysicalAddress.Size = new Size(100, 23);
            txtPhysicalAddress.TabIndex = 12;
            // 
            // txtEmailAddress
            // 
            txtEmailAddress.Location = new Point(583, 121);
            txtEmailAddress.Name = "txtEmailAddress";
            txtEmailAddress.Size = new Size(100, 23);
            txtEmailAddress.TabIndex = 11;
            // 
            // txtPhoneNumber
            // 
            txtPhoneNumber.Location = new Point(369, 248);
            txtPhoneNumber.Name = "txtPhoneNumber";
            txtPhoneNumber.Size = new Size(100, 23);
            txtPhoneNumber.TabIndex = 10;
            // 
            // txtDateOfBirth
            // 
            txtDateOfBirth.Location = new Point(197, 196);
            txtDateOfBirth.Name = "txtDateOfBirth";
            txtDateOfBirth.Size = new Size(100, 23);
            txtDateOfBirth.TabIndex = 9;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(197, 121);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(100, 23);
            txtPassword.TabIndex = 8;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(197, 52);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(100, 23);
            txtUser.TabIndex = 7;
            txtUser.TextChanged += txtUser_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(474, 58);
            label6.Name = "label6";
            label6.Size = new Size(69, 15);
            label6.TabIndex = 5;
            label6.Text = "Occupation";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(474, 196);
            label5.Name = "label5";
            label5.Size = new Size(93, 15);
            label5.TabIndex = 4;
            label5.Text = "Physical address";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(255, 251);
            label4.Name = "label4";
            label4.Size = new Size(86, 15);
            label4.TabIndex = 3;
            label4.Text = "Phone number";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(95, 199);
            label3.Name = "label3";
            label3.Size = new Size(73, 15);
            label3.TabIndex = 2;
            label3.Text = "Date of birth";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(95, 124);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 1;
            label2.Text = "Password";
            // 
            // txt
            // 
            txt.AutoSize = true;
            txt.Location = new Point(95, 55);
            txt.Name = "txt";
            txt.Size = new Size(61, 15);
            txt.TabIndex = 0;
            txt.Text = "Full Name";
            txt.Click += txt_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Name = "Form2";
            Text = "Form2";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label txt;
        private TextBox txtUser;
        private TextBox txtOccupation;
        private TextBox txtPhysicalAddress;
        private TextBox txtEmailAddress;
        private TextBox txtPhoneNumber;
        private TextBox txtDateOfBirth;
        private TextBox txtPassword;
        private Button button1;
        private Label label1;
    }
}