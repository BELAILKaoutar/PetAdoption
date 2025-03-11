namespace PetAdoption
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            panel2 = new Panel();
            linkLabel1 = new LinkLabel();
            label9 = new Label();
            label8 = new Label();
            label4 = new Label();
            button1 = new Button();
            label2 = new Label();
            label1 = new Label();
            txtmotdepassse = new TextBox();
            txtuser = new TextBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.DarkCyan;
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(0, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(331, 462);
            panel1.TabIndex = 0;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Times New Roman", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label7.Location = new Point(12, 292);
            label7.Name = "label7";
            label7.Size = new Size(211, 16);
            label7.TabIndex = 4;
            label7.Text = "Adoption is the kindest breed of love.";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Times New Roman", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label6.Location = new Point(12, 259);
            label6.Name = "label6";
            label6.Size = new Size(278, 16);
            label6.TabIndex = 3;
            label6.Text = "Be their hero,give a homeless pet a forever home!";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new Font("Times New Roman", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label5.Location = new Point(12, 224);
            label5.Name = "label5";
            label5.Size = new Size(313, 16);
            label5.TabIndex = 2;
            label5.Text = "Your new best friend is waiting ,bring them home today!\r\n";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Showcard Gothic", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(82, 154);
            label3.Name = "label3";
            label3.Size = new Size(103, 15);
            label3.TabIndex = 1;
            label3.Text = "Pets Friends";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.dog_logo;
            pictureBox1.Location = new Point(69, 38);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(142, 131);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Control;
            panel2.Controls.Add(linkLabel1);
            panel2.Controls.Add(label9);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(button1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(txtmotdepassse);
            panel2.Controls.Add(txtuser);
            panel2.Font = new Font("Wide Latin", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panel2.Location = new Point(328, 1);
            panel2.Name = "panel2";
            panel2.Size = new Size(471, 459);
            panel2.TabIndex = 1;
            panel2.Paint += panel2_Paint;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.LinkColor = Color.Teal;
            linkLabel1.Location = new Point(335, 379);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(99, 15);
            linkLabel1.TabIndex = 8;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Register ";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(100, 379);
            label9.Name = "label9";
            label9.Size = new Size(229, 15);
            label9.TabIndex = 7;
            label9.Text = "Don't have an acount?";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Leelawadee UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(100, 67);
            label8.Name = "label8";
            label8.Size = new Size(238, 17);
            label8.TabIndex = 6;
            label8.Text = "use your email and password to sign in";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Showcard Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(129, 22);
            label4.Name = "label4";
            label4.Size = new Size(185, 18);
            label4.TabIndex = 5;
            label4.Text = "Sign in to Pets Friends";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button1.BackColor = Color.DarkCyan;
            button1.BackgroundImage = Properties.Resources.dog_logo;
            button1.Cursor = Cursors.IBeam;
            button1.Location = new Point(227, 224);
            button1.Name = "button1";
            button1.Padding = new Padding(2);
            button1.RightToLeft = RightToLeft.Yes;
            button1.Size = new Size(190, 36);
            button1.TabIndex = 4;
            button1.Text = "Sign in";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(33, 172);
            label2.Name = "label2";
            label2.Size = new Size(140, 15);
            label2.TabIndex = 3;
            label2.Text = "Mot de passe:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(33, 117);
            label1.Name = "label1";
            label1.Size = new Size(188, 15);
            label1.TabIndex = 2;
            label1.Text = "Nom d'utilisateur:";
            // 
            // txtmotdepassse
            // 
            txtmotdepassse.Location = new Point(227, 169);
            txtmotdepassse.Name = "txtmotdepassse";
            txtmotdepassse.Size = new Size(190, 22);
            txtmotdepassse.TabIndex = 1;
            // 
            // txtuser
            // 
            txtuser.Location = new Point(227, 114);
            txtuser.Name = "txtuser";
            txtuser.Size = new Size(190, 22);
            txtuser.TabIndex = 0;
            txtuser.TextChanged += txtuser_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Pet Adoption ";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label3;
        private PictureBox pictureBox1;
        private Label label2;
        private Label label1;
        private TextBox txtmotdepassse;
        private TextBox txtuser;
        private Button button1;
        private Label label4;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label8;
        private LinkLabel linkLabel1;
        private Label label9;
    }
}
