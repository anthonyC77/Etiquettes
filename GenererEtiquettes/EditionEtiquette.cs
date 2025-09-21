using System;
using System.Windows.Forms;

namespace GenererEtiquettes
{
    public partial class EditionEtiquette : Form
    {
        public Produit Produit { get; set; }
        public EditionEtiquette(Produit produit)
        {
            Produit = produit;
            InitializeComponent();
            ChargerDonnees();
        }

        private void FormEditionEtiquette_Load(object sender, EventArgs e)
        {

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
    }
}
