using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerateurEtiquettes
{
    public partial class FormEditionEtiquette : Form
    {
        public Produit Produit { get; set; }

        private TextBox txtLibelleCourt;
        private TextBox txtPrixVente;
        private TextBox txtPrixSolde;
        private TextBox txtReference;

        public FormEditionEtiquette(Produit produit)
        {
            Produit = produit;
            InitializeComponent();
            ChargerDonnees();
        }

        private void ChargerDonnees()
        {
            txtLibelleCourt.Text = Produit.LibelleCourt;
            txtPrixVente.Text = Produit.PrixDeVente.ToString();
            txtPrixSolde.Text = Produit.PrixSolde.ToString();
            txtReference.Text = Produit.ReferenceExterne;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Produit.LibelleCourt = txtLibelleCourt.Text;
            if (decimal.TryParse(txtPrixVente.Text, out decimal prixVente))
                Produit.PrixDeVente = prixVente;
            if (decimal.TryParse(txtPrixSolde.Text, out decimal prixSolde))
                Produit.PrixSolde = prixSolde;
            Produit.ReferenceExterne = txtReference.Text;

            DialogResult = DialogResult.OK;
        }

        private void FormEditionEtiquette_Load(object sender, EventArgs e)
        {

        }
    }
}
