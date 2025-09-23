using System.Drawing;

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
            this.lblFontLibelleCourt = new System.Windows.Forms.Label();
            this.lblTaillePrix = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblProvenance = new System.Windows.Forms.Label();
            this.lblPoids = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtLibelleCourt
            // 
            this.txtLibelleCourt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibelleCourt.Location = new System.Drawing.Point(241, 30);
            this.txtLibelleCourt.Margin = new System.Windows.Forms.Padding(4);
            this.txtLibelleCourt.Name = "txtLibelleCourt";
            this.txtLibelleCourt.Size = new System.Drawing.Size(388, 26);
            this.txtLibelleCourt.TabIndex = 0;
            // 
            // txtPrixVente
            // 
            this.txtPrixVente.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrixVente.Location = new System.Drawing.Point(241, 77);
            this.txtPrixVente.Margin = new System.Windows.Forms.Padding(4);
            this.txtPrixVente.Name = "txtPrixVente";
            this.txtPrixVente.Size = new System.Drawing.Size(388, 26);
            this.txtPrixVente.TabIndex = 1;
            // 
            // txtPrixSolde
            // 
            this.txtPrixSolde.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrixSolde.Location = new System.Drawing.Point(241, 122);
            this.txtPrixSolde.Margin = new System.Windows.Forms.Padding(4);
            this.txtPrixSolde.Name = "txtPrixSolde";
            this.txtPrixSolde.Size = new System.Drawing.Size(388, 26);
            this.txtPrixSolde.TabIndex = 2;
            // 
            // txtReference
            // 
            this.txtReference.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReference.Location = new System.Drawing.Point(241, 173);
            this.txtReference.Margin = new System.Windows.Forms.Padding(4);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(388, 26);
            this.txtReference.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(241, 269);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(390, 28);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbxPoids
            // 
            this.tbxPoids.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxPoids.Location = new System.Drawing.Point(241, 222);
            this.tbxPoids.Margin = new System.Windows.Forms.Padding(4);
            this.tbxPoids.Name = "tbxPoids";
            this.tbxPoids.Size = new System.Drawing.Size(388, 26);
            this.tbxPoids.TabIndex = 5;
            // 
            // lblFontLibelleCourt
            // 
            this.lblFontLibelleCourt.AutoSize = true;
            this.lblFontLibelleCourt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFontLibelleCourt.Location = new System.Drawing.Point(24, 36);
            this.lblFontLibelleCourt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFontLibelleCourt.Name = "lblFontLibelleCourt";
            this.lblFontLibelleCourt.Size = new System.Drawing.Size(94, 20);
            this.lblFontLibelleCourt.TabIndex = 16;
            this.lblFontLibelleCourt.Text = "Libellé court";
            // 
            // lblTaillePrix
            // 
            this.lblTaillePrix.AutoSize = true;
            this.lblTaillePrix.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaillePrix.Location = new System.Drawing.Point(24, 80);
            this.lblTaillePrix.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTaillePrix.Name = "lblTaillePrix";
            this.lblTaillePrix.Size = new System.Drawing.Size(135, 20);
            this.lblTaillePrix.TabIndex = 17;
            this.lblTaillePrix.Text = "Libellé Prix normal";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 128);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 20);
            this.label1.TabIndex = 18;
            this.label1.Text = "Libellé Prix adhérent";
            // 
            // lblProvenance
            // 
            this.lblProvenance.AutoSize = true;
            this.lblProvenance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProvenance.Location = new System.Drawing.Point(25, 179);
            this.lblProvenance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProvenance.Name = "lblProvenance";
            this.lblProvenance.Size = new System.Drawing.Size(93, 20);
            this.lblProvenance.TabIndex = 28;
            this.lblProvenance.Text = "Provenance";
            // 
            // lblPoids
            // 
            this.lblPoids.AutoSize = true;
            this.lblPoids.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoids.Location = new System.Drawing.Point(25, 228);
            this.lblPoids.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPoids.Name = "lblPoids";
            this.lblPoids.Size = new System.Drawing.Size(97, 20);
            this.lblPoids.TabIndex = 31;
            this.lblPoids.Text = "Libellé Poids";
            // 
            // EditionEtiquette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 385);
            this.Controls.Add(this.lblPoids);
            this.Controls.Add(this.lblProvenance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTaillePrix);
            this.Controls.Add(this.lblFontLibelleCourt);
            this.Controls.Add(this.tbxPoids);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtReference);
            this.Controls.Add(this.txtPrixSolde);
            this.Controls.Add(this.txtPrixVente);
            this.Controls.Add(this.txtLibelleCourt);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EditionEtiquette";
            this.Text = "Edition Etiquette";
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
        private System.Windows.Forms.Label lblFontLibelleCourt;
        private System.Windows.Forms.Label lblTaillePrix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProvenance;
        private System.Windows.Forms.Label lblPoids;
    }
}