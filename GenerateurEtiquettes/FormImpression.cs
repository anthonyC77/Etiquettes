using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerateurEtiquettes
{
    public partial class FormImpression : Form
    {
        private void FormImpression_Load(object sender, EventArgs e)
        {
            
        }

        private int nombreMaximumCaractereLibelle = 30;
        private int etiquetteCourante = 0;
        private List<Produit> _Etiquettes;
        private PictureBox _apercu;
        private Button _btnImprimer;
        private NumericUpDown _nbColonnes;
        private NumericUpDown _nbLignes;
        private PrintDocument _docImprimer;

        public FormImpression()
        {
            LoadFileCsv();
            InitialiserComposants();            
        }

        private void LoadFileCsv()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Fichiers CSV (*.csv)|*.csv";
                openFileDialog.Title = "Sélectionner un fichier CSV";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Ici vous pouvez traiter le fichier CSV sélectionné
                    string cheminFichier = openFileDialog.FileName;

                    // Chargement des données du CSV
                    _Etiquettes = LireFichier.Parcourir(cheminFichier);
                }
            }
        }

        private void InitialiserComposants()
        {
            this.Text = "Générateur d'étiquettes";
            this.Size = new Size(800, 600);

            var panel = new Panel();
            panel.Dock = DockStyle.Top;

            TailleEtiquettes_10_3(panel);
            _btnImprimer.Click += BtnImprimer_Click;

            _apercu = new PictureBox();
            _apercu.Dock = DockStyle.Fill;
            _apercu.BorderStyle = BorderStyle.FixedSingle;
            _apercu.SizeMode = PictureBoxSizeMode.Zoom;

            this.Controls.Add(_apercu);
            this.Controls.Add(panel);

            _docImprimer = new PrintDocument();
            _docImprimer.PrintPage += ImpressionMultiplePages;

            // Générer l'aperçu initial
            _nbLignes.ValueChanged += (s, e) => GenererApercu();
            _nbColonnes.ValueChanged += (s, e) => GenererApercu();

            GenererApercu();
        }
        private void TailleEtiquettes_10_3(Panel panel)
        {
            Label lblColonnes, lblLignes;
            panel.Height = 80;

            // Calculer le nombre d'étiquettes qui peuvent tenir sur une page A4
            // A4 = 21 x 29,7 cm
            // Étiquette = 10 x 3 cm
            // Donc environ 2 colonnes et 9 lignes par défaut
            int colonnesDefaut = (int)Math.Floor(21.0 / 10.0);
            int lignesDefaut = (int)Math.Floor(29.7 / 3.0);

            lblColonnes = new Label { Text = "Colonnes:", Left = 10, Top = 15 };
            _nbColonnes = new NumericUpDown { Left = 80, Top = 12, Minimum = 1, Maximum = 10, Value = colonnesDefaut };

            lblLignes = new Label { Text = "Lignes:", Left = 150, Top = 15 };
            _nbLignes = new NumericUpDown { Left = 200, Top = 12, Minimum = 1, Maximum = 20, Value = lignesDefaut };

            var lblTaille = new Label { Text = "Taille étiquette: 10cm x 3cm", Left = 10, Top = 45, Width = 200 };

            _btnImprimer = new Button { Text = "Imprimer", Left = 500, Top = 25, Width = 100 };

            panel.Controls.AddRange(new Control[] { lblColonnes, _nbColonnes, lblLignes, _nbLignes, lblTaille, _btnImprimer });
        }

        private void GenererApercu()
        {
            int colonnes = (int)_nbColonnes.Value;
            int lignes = (int)_nbLignes.Value;

            // Créer une image pour l'aperçu
            Bitmap bmp = new Bitmap(827, 1169); // A4 à 100 DPI
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                DessinerEtiquettes(g, new Rectangle(0, 0, bmp.Width, bmp.Height), colonnes, lignes);
            }

            // Afficher l'aperçu
            _apercu.Image = bmp;
        }

        private int DessinerEtiquettes(Graphics g, Rectangle zone, int colonnes, int lignes, int indexProduit = 0)
        {
            int largeurEtiquette = zone.Width / colonnes;
            int hauteurEtiquette = zone.Height / lignes;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            for (int y = 0; y < lignes; y++)
            {
                for (int x = 0; x < colonnes; x++)
                {
                    indexProduit = CreerEtiquette(g, zone, x, y, indexProduit, largeurEtiquette, hauteurEtiquette);
                }
            }

            return indexProduit;
        }

        private int CreerEtiquette(Graphics g, Rectangle zone, int x, int y, int indexProduit, int largeurEtiquette, int hauteurEtiquette)
        {
            if (indexProduit < _Etiquettes.Count)
            {
                Rectangle rectEtiquette = new Rectangle(
                    zone.X + x * largeurEtiquette,
                    zone.Y + y * hauteurEtiquette,
                    largeurEtiquette,
                    hauteurEtiquette);

                // Dessiner un cadre
                g.DrawRectangle(Pens.Black, rectEtiquette);

                // Récupérer le produit actuel
                var produit = _Etiquettes[indexProduit];

                // Dessiner le contenu de l'étiquette
                Rectangle rectLibelle = new Rectangle(
                    rectEtiquette.X + 5,
                    rectEtiquette.Y + 5,
                    rectEtiquette.Width - 10,
                    rectEtiquette.Height / 2 - 15);

                Rectangle rectPrix = new Rectangle(
                    rectEtiquette.X + 5,
                    rectEtiquette.Y + rectEtiquette.Height / 2,
                    rectEtiquette.Width - 10,
                    30);

                Rectangle rectNumero = new Rectangle(
                    rectEtiquette.X + 5,
                    rectEtiquette.Y + rectEtiquette.Height / 2 + 35,
                    rectEtiquette.Width - 10,
                    20);

                indexProduit = DessinerChaqueInformation(g, indexProduit, rectEtiquette, produit);

            }

            return indexProduit;
        }

        private void BtnImprimer_Click(object sender, EventArgs e)
        {
            using (PrintDialog dlg = new PrintDialog())
            {
                dlg.Document = _docImprimer;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _docImprimer.Print();
                }
            }
        }

        private void DocImprimer_PrintPage(object sender, PrintPageEventArgs e)
        {
            int colonnes = (int)_nbColonnes.Value;
            int lignes = (int)_nbLignes.Value;

            DessinerEtiquettes(e.Graphics, e.MarginBounds, colonnes, lignes);

            // Indiquer s'il y a d'autres pages à imprimer
            e.HasMorePages = false; // Pour cet exemple, une seule page
        }

        private void ImpressionMultiplePages(object sender, PrintPageEventArgs e)
        {
            int colonnes = (int)_nbColonnes.Value;
            int lignes = (int)_nbLignes.Value;            
            etiquetteCourante = DessinerEtiquettes(e.Graphics, e.MarginBounds, colonnes, lignes, etiquetteCourante);
            e.HasMorePages = etiquetteCourante < _Etiquettes.Count;
        }


        private int DessinerChaqueInformation(Graphics g, int indexProduit, Rectangle rectEtiquette, Produit produit)
        {
            using (Font fontLibelle = new Font("Arial",11, FontStyle.Regular))
            using (Font fontPrix = new Font("Arial", 13, FontStyle.Bold))
            using (Font fontPrixSolde = new Font("Arial", 13, FontStyle.Bold))
            using (Font fontkg = new Font("Arial", 12, FontStyle.Regular))
            using (Font font3eLigne = new Font("Arial", 8, FontStyle.Regular))
            {
                // Ligne 1: Libellé court centré en haut
                Rectangle rectLibelleCourt = new Rectangle(
                    rectEtiquette.X + 5,
                    rectEtiquette.Y + 5,
                    rectEtiquette.Width - 10,
                    rectEtiquette.Height / 3 - 5);

                // Ligne 2: Deux prix (normal et soldé) et prix/kg
                Rectangle rectPrixNormal = new Rectangle(
                    rectEtiquette.X,
                    rectEtiquette.Y + rectEtiquette.Height / 3,
                    rectEtiquette.Width / 2 - 1,
                    rectEtiquette.Height / 3);

                Rectangle rectPrixSolde = new Rectangle(
                    rectEtiquette.X + rectEtiquette.Width / 2 + 20,
                    rectEtiquette.Y + rectEtiquette.Height / 3,
                    rectEtiquette.Width / 4,
                    rectEtiquette.Height / 3);

                Rectangle rectPrixKg = new Rectangle(
                    rectEtiquette.X + 2 * rectEtiquette.Width / 3 + 10,
                    rectEtiquette.Y + rectEtiquette.Height / 3,
                    rectEtiquette.Width / 3 - 5,
                    rectEtiquette.Height / 3);

                // Ligne 3: Provenance à gauche et code-barre à droite
                Rectangle rectProvenance = new Rectangle(
                    rectEtiquette.X + 5,
                    rectEtiquette.Y + 2 * rectEtiquette.Height / 3,
                    rectEtiquette.Width / 2 + 25,
                    rectEtiquette.Height / 3 - 5);

                Rectangle rectCodeBarre = new Rectangle(
                    rectEtiquette.X + rectEtiquette.Width / 2,
                    rectEtiquette.Y + 2 * rectEtiquette.Height / 3,
                    rectEtiquette.Width / 2 - 5,
                    rectEtiquette.Height / 3 - 5);

                // Formatage pour différents éléments
                StringFormat formatCentre = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                StringFormat formatGauche = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };

                StringFormat formatDroite = new StringFormat
                {
                    Alignment = StringAlignment.Far,
                    LineAlignment = StringAlignment.Center
                };

                // Dessiner le libellé court en haut, centré

                
                int max = produit.LibelleCourt.Length;
                if (max > nombreMaximumCaractereLibelle)
                {
                    max = nombreMaximumCaractereLibelle;
                }

                g.DrawString(produit.LibelleCourt.Substring(0, max), fontLibelle, Brushes.Black, rectLibelleCourt, formatCentre);

                // Dessiner les informations de prix (prix normal, soldé et par kg)
                g.DrawString(produit.PrixDeVente.ToString(), fontPrix, Brushes.Black, rectPrixNormal, formatDroite);

                // Prix soldé (en rouge et barré pour le prix normal)
                if (!string.IsNullOrEmpty(produit.PrixSolde.ToString()) && produit.PrixSolde != produit.PrixDeVente && produit.PrixSolde > 0)
                {
                    g.DrawString(produit.PrixSolde.ToString(), fontPrix, Brushes.Black, rectPrixSolde, formatGauche);
                }

                string parKilo = produit.LibellePoids;
                //string parKilo = " /kg";

                if (TestGrammeKg(produit.LibelleCourt))
                {
                    parKilo = string.Empty;
                }

                g.DrawString(parKilo, fontkg, Brushes.Black, rectPrixKg, formatCentre);

                // Dessiner la provenance en bas à gauche
                if (!string.IsNullOrEmpty(produit.ReferenceExterne))
                {
                    g.DrawString(produit.ReferenceExterne, font3eLigne, Brushes.Black, rectProvenance, formatDroite);
                }

                //if (!string.IsNullOrEmpty(produit.CodeBarre))
                //{
                //    g.DrawString(produit.CodeBarre, font3eLigne, Brushes.Black, rectCodeBarre, formatDroite);
                //}

                indexProduit++;
            }

            return indexProduit;
        }

        bool TestGrammeKg(string input)
        {
            string pattern = @"\d+(\.\d+)?\s*(kg|g|mg|l|ml|cl|dl)(\s|$|[^\w])";

            // Recherche du motif dans le libellé
            Match match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

            return match.Success;
        }
    }
}
