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
            this.nbColonnes = new System.Windows.Forms.NumericUpDown();
            this.nbLignes = new System.Windows.Forms.NumericUpDown();
            this.lblNbColonnes = new System.Windows.Forms.Label();
            this.lblNbLignes = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nbColonnes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbLignes)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(166, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Taille étiquette : 10cm X 3cm";
            // 
            // btnPagePrecedente
            // 
            this.btnPagePrecedente.Location = new System.Drawing.Point(339, 47);
            this.btnPagePrecedente.Name = "btnPagePrecedente";
            this.btnPagePrecedente.Size = new System.Drawing.Size(113, 23);
            this.btnPagePrecedente.TabIndex = 1;
            this.btnPagePrecedente.Text = "< Précédent";
            this.btnPagePrecedente.UseVisualStyleBackColor = true;
            // 
            // btnPageSuivante
            // 
            this.btnPageSuivante.Location = new System.Drawing.Point(468, 47);
            this.btnPageSuivante.Name = "btnPageSuivante";
            this.btnPageSuivante.Size = new System.Drawing.Size(113, 23);
            this.btnPageSuivante.TabIndex = 2;
            this.btnPageSuivante.Text = "Suivant >";
            this.btnPageSuivante.UseVisualStyleBackColor = true;
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Location = new System.Drawing.Point(597, 52);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(52, 13);
            this.lblPageInfo.TabIndex = 3;
            this.lblPageInfo.Text = "Page 0/0";
            // 
            // btnImprimer
            // 
            this.btnImprimer.Location = new System.Drawing.Point(667, 47);
            this.btnImprimer.Name = "btnImprimer";
            this.btnImprimer.Size = new System.Drawing.Size(113, 23);
            this.btnImprimer.TabIndex = 4;
            this.btnImprimer.Text = "Imprimer";
            this.btnImprimer.UseVisualStyleBackColor = true;
            this.btnImprimer.Click += new System.EventHandler(this.BtnImprimer_Click);
            // 
            // btnChargerCSV
            // 
            this.btnChargerCSV.Location = new System.Drawing.Point(12, 47);
            this.btnChargerCSV.Name = "btnChargerCSV";
            this.btnChargerCSV.Size = new System.Drawing.Size(141, 23);
            this.btnChargerCSV.TabIndex = 5;
            this.btnChargerCSV.Text = "Charger CSV";
            this.btnChargerCSV.UseVisualStyleBackColor = true;
            // 
            // nbColonnes
            // 
            this.nbColonnes.Location = new System.Drawing.Point(588, 12);
            this.nbColonnes.Name = "nbColonnes";
            this.nbColonnes.Size = new System.Drawing.Size(113, 20);
            this.nbColonnes.TabIndex = 6;
            // 
            // nbLignes
            // 
            this.nbLignes.Location = new System.Drawing.Point(343, 12);
            this.nbLignes.Name = "nbLignes";
            this.nbLignes.Size = new System.Drawing.Size(113, 20);
            this.nbLignes.TabIndex = 7;
            // 
            // lblNbColonnes
            // 
            this.lblNbColonnes.AutoSize = true;
            this.lblNbColonnes.Location = new System.Drawing.Point(472, 14);
            this.lblNbColonnes.Name = "label2";
            this.lblNbColonnes.Size = new System.Drawing.Size(105, 13);
            this.lblNbColonnes.TabIndex = 9;
            this.lblNbColonnes.Text = "Nombre de colonnes";
            // 
            // lblNbLignes
            // 
            this.lblNbLignes.AutoSize = true;
            this.lblNbLignes.Location = new System.Drawing.Point(227, 14);
            this.lblNbLignes.Name = "label3";
            this.lblNbLignes.Size = new System.Drawing.Size(89, 13);
            this.lblNbLignes.TabIndex = 10;
            this.lblNbLignes.Text = "Nombre de lignes";
            // 
            // Impression
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 450);
            this.Controls.Add(this.lblNbLignes);
            this.Controls.Add(this.lblNbColonnes);
            this.Controls.Add(this.nbLignes);
            this.Controls.Add(this.nbColonnes);
            this.Controls.Add(this.btnChargerCSV);
            this.Controls.Add(this.btnImprimer);
            this.Controls.Add(this.lblPageInfo);
            this.Controls.Add(this.btnPageSuivante);
            this.Controls.Add(this.btnPagePrecedente);
            this.Controls.Add(this.label1);
            this.Name = "Impression";
            this.Text = "FormImpression";
            this.Load += new System.EventHandler(this.FormImpression_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nbColonnes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbLignes)).EndInit();
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
        private System.Windows.Forms.NumericUpDown nbColonnes;
        private System.Windows.Forms.NumericUpDown nbLignes;
        private System.Windows.Forms.Label lblNbColonnes;
        private System.Windows.Forms.Label lblNbLignes;
    }
}