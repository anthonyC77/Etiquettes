using GenererEtiquettes;
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

namespace GenererEtiquettes
{
    public partial class Impression : Form
    {
        private int nombreMaximumCaractereLibelle = 30;
        private int etiquetteCourante = 0;
        private List<Produit> _Etiquettes;
        private PictureBox _apercu;
        private PrintDocument _docImprimer;
        private int pageActuelle = 0;
        private int totalPages = 0;
        private NumericUpDown _taillePoliceLibelle;
        private NumericUpDown _taillePoliceprix;
        private NumericUpDown _maxCaracteres;

        public Impression()
        {
            InitializeComponent();
            _Etiquettes = new List<Produit>();
            InitialiserComposants();
        }

        private void FormImpression_Load(object sender, EventArgs e)
        {

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

                    pageActuelle = 0;
                    MettreAJourNavigationPage();
                    GenererApercu();
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

            _apercu = new PictureBox();
            _apercu.Dock = DockStyle.Fill;
            _apercu.BorderStyle = BorderStyle.FixedSingle;
            _apercu.SizeMode = PictureBoxSizeMode.Zoom;

            this.Controls.Add(_apercu);
            this.Controls.Add(panel);

            _docImprimer = new PrintDocument();
            _docImprimer.PrintPage += ImpressionMultiplePages;

            // Générer l'aperçu initial
            nbLignes.ValueChanged += (s, e) => { pageActuelle = 0; GenererApercu(); };
            nbColonnes.ValueChanged += (s, e) => { pageActuelle = 0; GenererApercu(); };

            _apercu.MouseDoubleClick += Apercu_MouseDoubleClick;

            MettreAJourNavigationPage();
            GenererApercu();
        }
        private void Apercu_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_Etiquettes == null || _Etiquettes.Count == 0) return;

            // Calculer quelle étiquette a été cliquée
            int colonnes = (int)nbColonnes.Value;
            int lignes = (int)nbLignes.Value;

            // Obtenir les dimensions réelles de l'image affichée dans le PictureBox
            if (_apercu.Image == null) return;

            // Calculer les dimensions de l'image affichée (avec SizeMode.Zoom)
            Rectangle imageRect = GetImageDisplayRectangle(_apercu);

            // Vérifier si le clic est dans l'image
            if (!imageRect.Contains(e.Location)) return;

            // Convertir les coordonnées du clic vers les coordonnées de l'image
            float scaleX = (float)_apercu.Image.Width / imageRect.Width;
            float scaleY = (float)_apercu.Image.Height / imageRect.Height;

            int imageX = (int)((e.X - imageRect.X) * scaleX);
            int imageY = (int)((e.Y - imageRect.Y) * scaleY);

            int largeurEtiquette = _apercu.Image.Width / colonnes;
            int hauteurEtiquette = _apercu.Image.Height / lignes;

            int colonne = imageX / largeurEtiquette;
            int ligne = imageY / hauteurEtiquette;

            // S'assurer que les coordonnées sont valides
            if (colonne >= colonnes || ligne >= lignes) return;

            int etiquettesParPage = colonnes * lignes;
            int indexEtiquette = pageActuelle * etiquettesParPage + ligne * colonnes + colonne;

            if (indexEtiquette < _Etiquettes.Count)
            {
                var formEdition = new EditionEtiquette(_Etiquettes[indexEtiquette]);
                if (formEdition.ShowDialog() == DialogResult.OK)
                {
                    GenererApercu(); // Régénérer après édition
                }
            }
        }

        private Rectangle GetImageDisplayRectangle(PictureBox pictureBox)
        {
            if (pictureBox.Image == null) return Rectangle.Empty;

            // Calculer le rectangle d'affichage pour SizeMode.Zoom
            float imageAspect = (float)pictureBox.Image.Width / pictureBox.Image.Height;
            float containerAspect = (float)pictureBox.Width / pictureBox.Height;

            int displayWidth, displayHeight, displayX, displayY;

            if (imageAspect > containerAspect)
            {
                // L'image est plus large que le conteneur
                displayWidth = pictureBox.Width;
                displayHeight = (int)(pictureBox.Width / imageAspect);
                displayX = 0;
                displayY = (pictureBox.Height - displayHeight) / 2;
            }
            else
            {
                // L'image est plus haute que le conteneur
                displayWidth = (int)(pictureBox.Height * imageAspect);
                displayHeight = pictureBox.Height;
                displayX = (pictureBox.Width - displayWidth) / 2;
                displayY = 0;
            }

            return new Rectangle(displayX, displayY, displayWidth, displayHeight);
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

            nbColonnes.Value = colonnesDefaut;
            nbLignes.Value = lignesDefaut;

            btnChargerCSV.Click += (s, e) => LoadFileCsv();

            btnPagePrecedente.Click += (s, e) => ChangerPage(-1);
            btnPageSuivante.Click += (s, e) => ChangerPage(1);
        }

        private void GenererApercu()
        {
            if (_Etiquettes == null || _Etiquettes.Count == 0)
            {
                // Afficher une image vide si pas d'étiquettes
                Bitmap bmpVide = new Bitmap(827, 1169);
                using (Graphics g = Graphics.FromImage(bmpVide))
                {
                    g.Clear(Color.White);
                }
                _apercu.Image = bmpVide;
                MettreAJourNavigationPage();
                return;
            }

            int colonnes = (int)nbColonnes.Value;
            int lignes = (int)nbLignes.Value;

            int etiquettesParPage = colonnes * lignes;
            int indexDepart = pageActuelle * etiquettesParPage;

            // Créer une image pour l'aperçu
            Bitmap bmp = new Bitmap(827, 1169); // A4 à 100 DPI
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                // CORRECTION : Passer l'indexDepart à DessinerEtiquettes
                DessinerEtiquettes(g, new Rectangle(0, 0, bmp.Width, bmp.Height), colonnes, lignes, indexDepart);
            }

            // Afficher l'aperçu
            _apercu.Image = bmp;
            MettreAJourNavigationPage();
        }

        private void ChangerPage(int direction)
        {
            pageActuelle += direction;
            if (pageActuelle < 0) pageActuelle = 0;
            if (pageActuelle >= totalPages) pageActuelle = totalPages - 1;

            GenererApercu();
            MettreAJourNavigationPage();
        }

        private void MettreAJourNavigationPage()
        {
            if (_Etiquettes == null || _Etiquettes.Count == 0)
            {
                lblPageInfo.Text = "Page 0/0";
                btnPagePrecedente.Enabled = false;
                btnPageSuivante.Enabled = false;
                totalPages = 0;
                return;
            }

            int colonnes = (int)nbColonnes.Value;
            int lignes = (int)nbLignes.Value;
            int etiquettesParPage = colonnes * lignes;

            totalPages = Math.Max(1, (int)Math.Ceiling((double)_Etiquettes.Count / etiquettesParPage));

            // S'assurer que pageActuelle est dans les limites
            if (pageActuelle >= totalPages) pageActuelle = totalPages - 1;
            if (pageActuelle < 0) pageActuelle = 0;

            lblPageInfo.Text = $"Page {pageActuelle + 1}/{totalPages}";
            btnPagePrecedente.Enabled = pageActuelle > 0;
            btnPageSuivante.Enabled = pageActuelle < totalPages - 1;
        }

        private int DessinerEtiquettes(Graphics g, Rectangle zone, int colonnes, int lignes, int indexProduit = 0)
        {
            int largeurEtiquette = zone.Width / colonnes;
            int hauteurEtiquette = zone.Height / lignes;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            int currentIndex = indexProduit;

            for (int y = 0; y < lignes; y++)
            {
                for (int x = 0; x < colonnes; x++)
                {
                    // Dessiner l'étiquette ou un cadre vide
                    Rectangle rectEtiquette = new Rectangle(
                        zone.X + x * largeurEtiquette,
                        zone.Y + y * hauteurEtiquette,
                        largeurEtiquette,
                        hauteurEtiquette);

                    // Toujours dessiner le cadre
                    g.DrawRectangle(Pens.Black, rectEtiquette);

                    // Si on a une étiquette, la dessiner
                    if (currentIndex < _Etiquettes.Count)
                    {
                        var produit = _Etiquettes[currentIndex];
                        DessinerChaqueInformation(g, currentIndex, rectEtiquette, produit);
                    }
                    // Incrémenter même si pas d'étiquette pour garder la position correcte
                    currentIndex++;
                }
            }

            return currentIndex;
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
            int colonnes = (int)nbColonnes.Value;
            int lignes = (int)nbLignes.Value;

            DessinerEtiquettes(e.Graphics, e.MarginBounds, colonnes, lignes);

            // Indiquer s'il y a d'autres pages à imprimer
            e.HasMorePages = false; // Pour cet exemple, une seule page
        }

        private void ImpressionMultiplePages(object sender, PrintPageEventArgs e)
        {
            int colonnes = (int)nbColonnes.Value;
            int lignes = (int)nbLignes.Value;
            etiquetteCourante = DessinerEtiquettes(e.Graphics, e.MarginBounds, colonnes, lignes, etiquetteCourante);
            e.HasMorePages = etiquetteCourante < _Etiquettes.Count;
        }


        private int DessinerChaqueInformation(Graphics g, int indexProduit, Rectangle rectEtiquette, Produit produit)
        {
            int tailleLibelle = (int)(_taillePoliceLibelle?.Value ?? 11);
            int taillePrix = (int)(_taillePoliceprix?.Value ?? 13);

            using (Font fontLibelle = new Font("Arial", 11, FontStyle.Regular))
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
