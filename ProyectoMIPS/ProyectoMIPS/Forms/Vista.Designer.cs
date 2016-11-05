namespace ProyectoMIPS.Forms
{
    partial class Vista
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
            this.label1 = new System.Windows.Forms.Label();
            this.numeroHilillos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numeroQuantum = new System.Windows.Forms.TextBox();
            this.directorio = new System.Windows.Forms.Button();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.aceptar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Número de hilillos:";
            // 
            // numeroHilillos
            // 
            this.numeroHilillos.Location = new System.Drawing.Point(150, 40);
            this.numeroHilillos.Name = "numeroHilillos";
            this.numeroHilillos.Size = new System.Drawing.Size(110, 20);
            this.numeroHilillos.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Directorio:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Número de quantum:";
            // 
            // numeroQuantum
            // 
            this.numeroQuantum.Location = new System.Drawing.Point(150, 118);
            this.numeroQuantum.Name = "numeroQuantum";
            this.numeroQuantum.Size = new System.Drawing.Size(110, 20);
            this.numeroQuantum.TabIndex = 4;
            // 
            // directorio
            // 
            this.directorio.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.directorio.Location = new System.Drawing.Point(150, 79);
            this.directorio.Name = "directorio";
            this.directorio.Size = new System.Drawing.Size(110, 23);
            this.directorio.TabIndex = 5;
            this.directorio.Text = "Directorio";
            this.directorio.UseVisualStyleBackColor = false;
            this.directorio.Click += new System.EventHandler(this.directorio_Click);
            // 
            // aceptar
            // 
            this.aceptar.BackColor = System.Drawing.Color.MintCream;
            this.aceptar.Location = new System.Drawing.Point(185, 171);
            this.aceptar.Name = "aceptar";
            this.aceptar.Size = new System.Drawing.Size(75, 23);
            this.aceptar.TabIndex = 6;
            this.aceptar.Text = "Aceptar";
            this.aceptar.UseVisualStyleBackColor = false;
            this.aceptar.Click += new System.EventHandler(this.aceptar_Click);
            // 
            // Vista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(291, 225);
            this.Controls.Add(this.aceptar);
            this.Controls.Add(this.directorio);
            this.Controls.Add(this.numeroQuantum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numeroHilillos);
            this.Controls.Add(this.label1);
            this.Name = "Vista";
            this.Text = "Llenar información";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox numeroHilillos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox numeroQuantum;
        private System.Windows.Forms.Button directorio;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.Button aceptar;
    }
}