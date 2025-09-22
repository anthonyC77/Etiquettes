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

                    foreach (var etiquette in _Etiquettes)
                    {
                        etiquette.EstSelectionne = true;
                    }

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
            _apercu.MouseClick += Apercu_MouseClick;

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

        private void Apercu_MouseClick(object sender, MouseEventArgs e)
        {
            if (_Etiquettes == null || _Etiquettes.Count == 0) return;

            int colonnes = (int)nbColonnes.Value;
            int lignes = (int)nbLignes.Value;

            if (_apercu.Image == null) return;

            Rectangle imageRect = GetImageDisplayRectangle(_apercu);
            if (!imageRect.Contains(e.Location)) return;

            float scaleX = (float)_apercu.Image.Width / imageRect.Width;
            float scaleY = (float)_apercu.Image.Height / imageRect.Height;

            int imageX = (int)((e.X - imageRect.X) * scaleX);
            int imageY = (int)((e.Y - imageRect.Y) * scaleY);

            int largeurEtiquette = _apercu.Image.Width / colonnes;
            int hauteurEtiquette = _apercu.Image.Height / lignes;

            int colonne = imageX / largeurEtiquette;
            int ligne = imageY / hauteurEtiquette;

            if (colonne >= colonnes || ligne >= lignes) return;

            int etiquettesParPage = colonnes * lignes;
            int indexEtiquette = pageActuelle * etiquettesParPage + ligne * colonnes + colonne;

            if (indexEtiquette < _Etiquettes.Count)
            {
                // Toggle la sélection de toute l'étiquette
                _Etiquettes[indexEtiquette].EstSelectionne = !_Etiquettes[indexEtiquette].EstSelectionne;
                GenererApercu(); // Redessiner pour mettre à jour l'affichage
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
            panel.Height = 120; // Augmenter la hauteur pour les nouveaux boutons

            int colonnesDefaut = (int)Math.Floor(21.0 / 10.0);
            int lignesDefaut = (int)Math.Floor(29.7 / 3.0);

            nbColonnes.Value = colonnesDefaut;
            nbLignes.Value = lignesDefaut;

            btnChargerCSV.Click += (s, e) => LoadFileCsv();
            btnPagePrecedente.Click += (s, e) => ChangerPage(-1);
            btnPageSuivante.Click += (s, e) => ChangerPage(1);

            // Ajouter des boutons pour la sélection
            var btnToutSelectionner = new Button();
            btnToutSelectionner.Text = "Tout sélectionner";
            btnToutSelectionner.Location = new Point(10, 90);
            btnToutSelectionner.Size = new Size(120, 25);
            btnToutSelectionner.Click += (s, e) => {
                foreach (var produit in _Etiquettes)
                    produit.EstSelectionne = true;
                GenererApercu();
            };
            panel.Controls.Add(btnToutSelectionner);

            var btnToutDeselectionner = new Button();
            btnToutDeselectionner.Text = "Tout désélectionner";
            btnToutDeselectionner.Location = new Point(140, 90);
            btnToutDeselectionner.Size = new Size(120, 25);
            btnToutDeselectionner.Click += (s, e) => {
                foreach (var produit in _Etiquettes)
                    produit.EstSelectionne = false;
                GenererApercu();
            };
            panel.Controls.Add(btnToutDeselectionner);

            var lblInfo = new Label();
            lblInfo.Text = "Clic simple sur ☐ pour sélectionner/désélectionner, double-clic pour éditer";
            lblInfo.Location = new Point(270, 90);
            lblInfo.Size = new Size(400, 25);
            lblInfo.ForeColor = Color.Gray;
            panel.Controls.Add(lblInfo);
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

            for (int y = 0; y < lignes && currentIndex < _Etiquettes.Count; y++)
            {
                for (int x = 0; x < colonnes && currentIndex < _Etiquettes.Count; x++)
                {
                    Rectangle rectEtiquette = new Rectangle(
                        zone.X + x * largeurEtiquette,
                        zone.Y + y * hauteurEtiquette,
                        largeurEtiquette,
                        hauteurEtiquette);

                    // Toujours dessiner le cadre
                    g.DrawRectangle(Pens.Black, rectEtiquette);

                    // Si étiquette non sélectionnée, dessiner un fond grisé
                    if (!_Etiquettes[currentIndex].EstSelectionne)
                    {
                        using (Brush grayBrush = new SolidBrush(Color.FromArgb(200, Color.LightGray)))
                        {
                            g.FillRectangle(grayBrush, rectEtiquette);
                            g.DrawRectangle(Pens.Black, rectEtiquette); // Redessiner le cadre par-dessus
                        }
                    }

                    var produit = _Etiquettes[currentIndex];
                    DessinerChaqueInformation(g, currentIndex, rectEtiquette, produit);
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
            // Filtrer seulement les étiquettes sélectionnées
            var etiquettesSelectionnees = _Etiquettes.Where(p => p.EstSelectionne).ToList();

            int colonnes = (int)nbColonnes.Value;
            int lignes = (int)nbLignes.Value;

            etiquetteCourante = DessinerEtiquettesImpression(e.Graphics, e.MarginBounds, colonnes, lignes, etiquetteCourante, etiquettesSelectionnees);
            e.HasMorePages = etiquetteCourante < etiquettesSelectionnees.Count;
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
                // AJOUTER UNE CASE À COCHER en haut à droite (seulement pour l'aperçu)
                Rectangle rectCheckbox = new Rectangle(
                    rectEtiquette.X + rectEtiquette.Width - 20,
                    rectEtiquette.Y + 5,
                    15, 15);

                // Dessiner la case
                g.DrawRectangle(Pens.Black, rectCheckbox);

                // Si sélectionné, dessiner une croix verte
                if (produit.EstSelectionne)
                {
                    using (Pen penCroix = new Pen(Color.Green, 2))
                    {
                        g.DrawLine(penCroix, rectCheckbox.Left + 2, rectCheckbox.Top + 2, rectCheckbox.Right - 2, rectCheckbox.Bottom - 2);
                        g.DrawLine(penCroix, rectCheckbox.Right - 2, rectCheckbox.Top + 2, rectCheckbox.Left + 2, rectCheckbox.Bottom - 2);
                    }
                }

                // Ajuster la zone du libellé pour éviter la case à cocher
                Rectangle rectLibelleCourt = new Rectangle(
                    rectEtiquette.X + 5,
                    rectEtiquette.Y + 5,
                    rectEtiquette.Width - 30, // Réduire la largeur pour la checkbox
                    rectEtiquette.Height / 3 - 5);

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

                // Couleur du texte selon la sélection
                Brush textBrush = produit.EstSelectionne ? Brushes.Black : Brushes.Gray;

                int max = produit.LibelleCourt.Length;
                if (max > nombreMaximumCaractereLibelle)
                {
                    max = nombreMaximumCaractereLibelle;
                }

                g.DrawString(produit.LibelleCourt.Substring(0, max), fontLibelle, textBrush, rectLibelleCourt, formatCentre);
                g.DrawString(produit.PrixDeVente.ToString(), fontPrix, textBrush, rectPrixNormal, formatDroite);

                if (!string.IsNullOrEmpty(produit.PrixSolde.ToString()) && produit.PrixSolde != produit.PrixDeVente && produit.PrixSolde > 0)
                {
                    g.DrawString(produit.PrixSolde.ToString(), fontPrix, textBrush, rectPrixSolde, formatGauche);
                }

                string parKilo = produit.LibellePoids;

                if (TestGrammeKg(produit.LibelleCourt))
                {
                    parKilo = string.Empty;
                }

                g.DrawString(parKilo, fontkg, textBrush, rectPrixKg, formatCentre);

                if (!string.IsNullOrEmpty(produit.ReferenceExterne))
                {
                    g.DrawString(produit.ReferenceExterne, font3eLigne, textBrush, rectProvenance, formatDroite);
                }

                indexProduit++;
            }

            return indexProduit;
        }

        private int DessinerEtiquettesImpression(Graphics g, Rectangle zone, int colonnes, int lignes, int indexProduit, List<Produit> etiquettesSelectionnees)
        {
            int largeurEtiquette = zone.Width / colonnes;
            int hauteurEtiquette = zone.Height / lignes;

            int currentIndex = indexProduit;

            for (int y = 0; y < lignes && currentIndex < etiquettesSelectionnees.Count; y++)
            {
                for (int x = 0; x < colonnes && currentIndex < etiquettesSelectionnees.Count; x++)
                {
                    Rectangle rectEtiquette = new Rectangle(
                        zone.X + x * largeurEtiquette,
                        zone.Y + y * hauteurEtiquette,
                        largeurEtiquette,
                        hauteurEtiquette);

                    g.DrawRectangle(Pens.Black, rectEtiquette);

                    var produit = etiquettesSelectionnees[currentIndex];
                    DessinerChaqueInformationImpression(g, currentIndex, rectEtiquette, produit);
                    currentIndex++;
                }
            }

            return currentIndex;
        }

        private int DessinerChaqueInformationImpression(Graphics g, int indexProduit, Rectangle rectEtiquette, Produit produit)
        {
            // Même code que DessinerChaqueInformation mais SANS case à cocher et toujours en noir
            using (Font fontLibelle = new Font("Arial", 11, FontStyle.Regular))
            using (Font fontPrix = new Font("Arial", 13, FontStyle.Bold))
            using (Font fontPrixSolde = new Font("Arial", 13, FontStyle.Bold))
            using (Font fontkg = new Font("Arial", 12, FontStyle.Regular))
            using (Font font3eLigne = new Font("Arial", 8, FontStyle.Regular))
            {
                // PAS DE CASE À COCHER pour l'impression
                Rectangle rectLibelleCourt = new Rectangle(
                    rectEtiquette.X + 5,
                    rectEtiquette.Y + 5,
                    rectEtiquette.Width - 10, // Largeur complète pour l'impression
                    rectEtiquette.Height / 3 - 5);

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

                // Toujours en noir pour l'impression
                Brush textBrush = Brushes.Black;

                int max = produit.LibelleCourt.Length;
                if (max > nombreMaximumCaractereLibelle)
                {
                    max = nombreMaximumCaractereLibelle;
                }

                g.DrawString(produit.LibelleCourt.Substring(0, max), fontLibelle, textBrush, rectLibelleCourt, formatCentre);
                g.DrawString(produit.PrixDeVente.ToString(), fontPrix, textBrush, rectPrixNormal, formatDroite);

                if (!string.IsNullOrEmpty(produit.PrixSolde.ToString()) && produit.PrixSolde != produit.PrixDeVente && produit.PrixSolde > 0)
                {
                    g.DrawString(produit.PrixSolde.ToString(), fontPrix, textBrush, rectPrixSolde, formatGauche);
                }

                string parKilo = produit.LibellePoids;

                if (TestGrammeKg(produit.LibelleCourt))
                {
                    parKilo = string.Empty;
                }

                g.DrawString(parKilo, fontkg, textBrush, rectPrixKg, formatCentre);

                if (!string.IsNullOrEmpty(produit.ReferenceExterne))
                {
                    g.DrawString(produit.ReferenceExterne, font3eLigne, textBrush, rectProvenance, formatDroite);
                }
            }

            return indexProduit + 1;
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
