namespace GenerateurEtiquettes
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormImpression());
        }

        private static List<Produit> LireFichierCSV(string cheminFichier)
        {
            var produits = new List<Produit>();

            try
            {
                // Lecture du fichier CSV
                string[] lignes = File.ReadAllLines(cheminFichier);

                // Déterminer l'index des colonnes désirées (en supposant que la première ligne contient les en-têtes)
                string[] enTetesTemp = lignes[0].Split(';');
                string[] enTetes = new string[enTetesTemp.Length];

                for (int i = 0; i < enTetesTemp.Length; i++)
                {
                    enTetes[i] = enTetesTemp[i].Trim('"', ' ', '\\');
                }

                int indexLibelle = Array.IndexOf(enTetes, @"Libellé");
                int indexLibelleCourt = Array.IndexOf(enTetes, @"Libellé court");
                int indexReferenceExterne = Array.IndexOf(enTetes, "Référence externe");
                int indexPrixDeVente = Array.IndexOf(enTetes, "Prix de vente");
                int indexPrixSolde = Array.IndexOf(enTetes, "Prix soldé");
                int indexCodeBarre = Array.IndexOf(enTetes, "Code-barres");

                // Vérification que les colonnes existent
                if (indexLibelleCourt == -1 
                    || indexReferenceExterne == -1 
                    || indexPrixDeVente == -1
                    || indexPrixSolde == -1
                    || indexCodeBarre == -1)
                {
                    Console.WriteLine("Colonnes requises non trouvées dans le fichier CSV");
                    return produits;
                }

                // Parcourir les lignes de données (à partir de la deuxième ligne)
                for (int i = 1; i < lignes.Length; i++)
                {
                    string[] valeursTemp = lignes[i].Split(';');
                    string[] valeurs = new string[valeursTemp.Length];

                    for (int j = 0; j < enTetesTemp.Length; j++)
                    {
                        valeurs[j] = valeursTemp[j].Trim('"', ' ', '\\');
                    }

                    var listIndex = new List<int>() { indexLibelleCourt, indexReferenceExterne, indexPrixDeVente, indexPrixSolde,indexCodeBarre  };

                    // Vérifier que la ligne a suffisamment de colonnes
                    if (valeurs.Length > listIndex.Max())
                    {

                        string libelleCourt = valeurs[indexLibelleCourt];
                        if (string.IsNullOrEmpty(libelleCourt))
                        {
                            libelleCourt = valeurs[indexLibelle];
                        }

                        var produit = new Produit
                        {
                            LibelleCourt = libelleCourt,
                            ReferenceExterne = valeurs[indexReferenceExterne],
                            PrixDeVente = ChangePrix(valeurs, indexPrixDeVente),
                            PrixSolde = ChangePrix(valeurs, indexPrixSolde),
                            CodeBarre = valeurs[indexCodeBarre],
                        };

                        produits.Add(produit);
                    }
                }

                Console.WriteLine($"{produits.Count} produits chargés avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture du fichier CSV: {ex.Message}");
            }

            return produits;
        }

        private static decimal ChangePrix(string[] valeurs, int index)
        {
            var prixVente = Math.Round(decimal.Parse(valeurs[index].ToString().Replace('.', ',')) * 1000, 2);
            if (prixVente >= 800)
            {
                prixVente = prixVente / 1000;
            }

            return prixVente;
        }
    }
}
