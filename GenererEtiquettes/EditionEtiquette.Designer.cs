namespace GenererEtiquettes
{
    partial class EditionEtiquette
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
            this.txtLibelleCourt = new System.Windows.Forms.TextBox();
            this.txtPrixVente = new System.Windows.Forms.TextBox();
            this.txtPrixSolde = new System.Windows.Forms.TextBox();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.tbxPoids = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtLibelleCourt
            // 
            this.txtLibelleCourt.Location = new System.Drawing.Point(47, 61);
            this.txtLibelleCourt.Name = "txtLibelleCourt";
            this.txtLibelleCourt.Size = new System.Drawing.Size(221, 20);
            this.txtLibelleCourt.TabIndex = 0;
            // 
            // txtPrixVente
            // 
            this.txtPrixVente.Location = new System.Drawing.Point(47, 99);
            this.txtPrixVente.Name = "txtPrixVente";
            this.txtPrixVente.Size = new System.Drawing.Size(221, 20);
            this.txtPrixVente.TabIndex = 1;
            // 
            // txtPrixSolde
            // 
            this.txtPrixSolde.Location = new System.Drawing.Point(47, 136);
            this.txtPrixSolde.Name = "txtPrixSolde";
            this.txtPrixSolde.Size = new System.Drawing.Size(221, 20);
            this.txtPrixSolde.TabIndex = 2;
            // 
            // txtReference
            // 
            this.txtReference.Location = new System.Drawing.Point(47, 177);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(221, 20);
            this.txtReference.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(47, 255);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(221, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbxPoids
            // 
            this.tbxPoids.Location = new System.Drawing.Point(47, 217);
            this.tbxPoids.Name = "tbxPoids";
            this.tbxPoids.Size = new System.Drawing.Size(221, 20);
            this.tbxPoids.TabIndex = 5;
            // 
            // EditionEtiquette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 304);
            this.Controls.Add(this.tbxPoids);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtReference);
            this.Controls.Add(this.txtPrixSolde);
            this.Controls.Add(this.txtPrixVente);
            this.Controls.Add(this.txtLibelleCourt);
            this.Name = "EditionEtiquette";
            this.Text = "FormEditionEtiquette";
            this.Load += new System.EventHandler(this.FormEditionEtiquette_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLibelleCourt;
        private System.Windows.Forms.TextBox txtPrixVente;
        private System.Windows.Forms.TextBox txtPrixSolde;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox tbxPoids;
    }
}