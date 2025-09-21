namespace GenererEtiquettes
{
    partial class Impression
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
            this.btnPagePrecedente = new System.Windows.Forms.Button();
            this.btnPageSuivante = new System.Windows.Forms.Button();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.btnImprimer = new System.Windows.Forms.Button();
            this.btnChargerCSV = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(166, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Taille étiquette : 10cm X 3cm";
            // 
            // btnPagePrecedente
            // 
            this.btnPagePrecedente.Location = new System.Drawing.Point(339, 46);
            this.btnPagePrecedente.Name = "btnPagePrecedente";
            this.btnPagePrecedente.Size = new System.Drawing.Size(113, 23);
            this.btnPagePrecedente.TabIndex = 1;
            this.btnPagePrecedente.Text = "< Précédent";
            this.btnPagePrecedente.UseVisualStyleBackColor = true;
            // 
            // btnPageSuivante
            // 
            this.btnPageSuivante.Location = new System.Drawing.Point(468, 46);
            this.btnPageSuivante.Name = "btnPageSuivante";
            this.btnPageSuivante.Size = new System.Drawing.Size(113, 23);
            this.btnPageSuivante.TabIndex = 2;
            this.btnPageSuivante.Text = "Suivant >";
            this.btnPageSuivante.UseVisualStyleBackColor = true;
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Location = new System.Drawing.Point(597, 51);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(52, 13);
            this.lblPageInfo.TabIndex = 3;
            this.lblPageInfo.Text = "Page 0/0";
            // 
            // btnImprimer
            // 
            this.btnImprimer.Location = new System.Drawing.Point(667, 46);
            this.btnImprimer.Name = "btnImprimer";
            this.btnImprimer.Size = new System.Drawing.Size(113, 23);
            this.btnImprimer.TabIndex = 4;
            this.btnImprimer.Text = "Imprimer";
            this.btnImprimer.UseVisualStyleBackColor = true;
            this.btnImprimer.Click += new System.EventHandler(this.BtnImprimer_Click);
            // 
            // btnChargerCSV
            // 
            this.btnChargerCSV.Location = new System.Drawing.Point(12, 46);
            this.btnChargerCSV.Name = "btnChargerCSV";
            this.btnChargerCSV.Size = new System.Drawing.Size(141, 23);
            this.btnChargerCSV.TabIndex = 5;
            this.btnChargerCSV.Text = "Charger CSV";
            this.btnChargerCSV.UseVisualStyleBackColor = true;
            // 
            // Impression
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 450);
            this.Controls.Add(this.btnChargerCSV);
            this.Controls.Add(this.btnImprimer);
            this.Controls.Add(this.lblPageInfo);
            this.Controls.Add(this.btnPageSuivante);
            this.Controls.Add(this.btnPagePrecedente);
            this.Controls.Add(this.label1);
            this.Name = "Impression";
            this.Text = "FormImpression";
            this.Load += new System.EventHandler(this.FormImpression_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPagePrecedente;
        private System.Windows.Forms.Button btnPageSuivante;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Button btnImprimer;
        private System.Windows.Forms.Button btnChargerCSV;
    }
}