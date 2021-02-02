
namespace Diploma
{
    partial class AdminControlPanel
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
            this.NameField = new System.Windows.Forms.TextBox();
            this.SurnameField = new System.Windows.Forms.TextBox();
            this.PasswordField = new System.Windows.Forms.TextBox();
            this.MailField = new System.Windows.Forms.TextBox();
            this.PhoneField = new System.Windows.Forms.TextBox();
            this.DateField = new System.Windows.Forms.TextBox();
            this.searchField = new System.Windows.Forms.TextBox();
            this.search = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.KnowledgeComboBox = new System.Windows.Forms.ComboBox();
            this.workingXPComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NameField
            // 
            this.NameField.Location = new System.Drawing.Point(389, 138);
            this.NameField.Name = "NameField";
            this.NameField.Size = new System.Drawing.Size(100, 20);
            this.NameField.TabIndex = 2;
            // 
            // SurnameField
            // 
            this.SurnameField.Location = new System.Drawing.Point(389, 179);
            this.SurnameField.Name = "SurnameField";
            this.SurnameField.Size = new System.Drawing.Size(100, 20);
            this.SurnameField.TabIndex = 1;
            // 
            // PasswordField
            // 
            this.PasswordField.Location = new System.Drawing.Point(389, 222);
            this.PasswordField.Name = "PasswordField";
            this.PasswordField.Size = new System.Drawing.Size(100, 20);
            this.PasswordField.TabIndex = 2;
            // 
            // MailField
            // 
            this.MailField.Location = new System.Drawing.Point(389, 263);
            this.MailField.Name = "MailField";
            this.MailField.Size = new System.Drawing.Size(100, 20);
            this.MailField.TabIndex = 3;
            // 
            // PhoneField
            // 
            this.PhoneField.Location = new System.Drawing.Point(389, 309);
            this.PhoneField.Name = "PhoneField";
            this.PhoneField.Size = new System.Drawing.Size(100, 20);
            this.PhoneField.TabIndex = 4;
            // 
            // DateField
            // 
            this.DateField.Location = new System.Drawing.Point(389, 348);
            this.DateField.Name = "DateField";
            this.DateField.Size = new System.Drawing.Size(100, 20);
            this.DateField.TabIndex = 5;
            // 
            // searchField
            // 
            this.searchField.Location = new System.Drawing.Point(157, 100);
            this.searchField.Name = "searchField";
            this.searchField.Size = new System.Drawing.Size(450, 20);
            this.searchField.TabIndex = 1;
            this.searchField.TextChanged += new System.EventHandler(this.textBox7_TextChanged);
            // 
            // search
            // 
            this.search.AutoSize = true;
            this.search.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.search.Location = new System.Drawing.Point(624, 94);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(33, 25);
            this.search.TabIndex = 7;
            this.search.Text = "🔍";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Title.Location = new System.Drawing.Point(308, 38);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(147, 20);
            this.Title.TabIndex = 8;
            this.Title.Text = "Поиск по логину";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.AliceBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(198, 494);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 50);
            this.button1.TabIndex = 9;
            this.button1.Text = "Изменить";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.White;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.AliceBlue;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.button3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button3.Location = new System.Drawing.Point(431, 494);
            this.button3.Margin = new System.Windows.Forms.Padding(0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(123, 50);
            this.button3.TabIndex = 11;
            this.button3.Text = "Удалить";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(154, 432);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 17);
            this.label10.TabIndex = 24;
            this.label10.Text = "Область знаний";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(154, 392);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 17);
            this.label9.TabIndex = 23;
            this.label9.Text = "Стаж";
            // 
            // KnowledgeComboBox
            // 
            this.KnowledgeComboBox.FormattingEnabled = true;
            this.KnowledgeComboBox.Items.AddRange(new object[] {
            "IT, программирование",
            "Инженер",
            "Медицина",
            "Юриспруденция",
            "Гос. Служба\t"});
            this.KnowledgeComboBox.Location = new System.Drawing.Point(368, 432);
            this.KnowledgeComboBox.Name = "KnowledgeComboBox";
            this.KnowledgeComboBox.Size = new System.Drawing.Size(121, 21);
            this.KnowledgeComboBox.TabIndex = 22;
            // 
            // workingXPComboBox
            // 
            this.workingXPComboBox.FormattingEnabled = true;
            this.workingXPComboBox.Items.AddRange(new object[] {
            "Студент",
            "Преподаватель",
            "Работающий",
            "Стажер",
            "Ученик",
            "Без работы"});
            this.workingXPComboBox.Location = new System.Drawing.Point(368, 392);
            this.workingXPComboBox.Name = "workingXPComboBox";
            this.workingXPComboBox.Size = new System.Drawing.Size(121, 21);
            this.workingXPComboBox.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(154, 351);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = "Дата Рождения";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(154, 310);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 17);
            this.label2.TabIndex = 26;
            this.label2.Text = "Номер телефона";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(154, 263);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 27;
            this.label3.Text = "Почта";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(154, 222);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 17);
            this.label4.TabIndex = 28;
            this.label4.Text = "Пароль";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(154, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 17);
            this.label5.TabIndex = 29;
            this.label5.Text = "Фамилия";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(154, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 17);
            this.label6.TabIndex = 30;
            this.label6.Text = "Имя";
            // 
            // AdminControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 580);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.KnowledgeComboBox);
            this.Controls.Add(this.workingXPComboBox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.search);
            this.Controls.Add(this.searchField);
            this.Controls.Add(this.DateField);
            this.Controls.Add(this.PhoneField);
            this.Controls.Add(this.MailField);
            this.Controls.Add(this.PasswordField);
            this.Controls.Add(this.SurnameField);
            this.Controls.Add(this.NameField);
            this.Name = "AdminControlPanel";
            this.Text = "AdminControlPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NameField;
        private System.Windows.Forms.TextBox SurnameField;
        private System.Windows.Forms.TextBox PasswordField;
        private System.Windows.Forms.TextBox MailField;
        private System.Windows.Forms.TextBox PhoneField;
        private System.Windows.Forms.TextBox DateField;
        private System.Windows.Forms.TextBox searchField;
        private System.Windows.Forms.Label search;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox KnowledgeComboBox;
        private System.Windows.Forms.ComboBox workingXPComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}