using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GenererEtiquettes
{
    public static class LireFichier
    {
        static int tailleMaxLibelleCourt = 30;
        public static List<Produit> Parcourir(string cheminFichier)
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

                int indexLibelle = ExtensionsProduit.GetChiffreProduit(enTetes,eProduit.Libelle);
                int indexLibelleCourt = ExtensionsProduit.GetChiffreProduit(enTetes, eProduit.LibelleCourt);
                int indexReferenceExterne = ExtensionsProduit.GetChiffreProduit(enTetes, eProduit.ReferenceExterne);
                int indexPrixDeVente = ExtensionsProduit.GetChiffreProduit(enTetes, eProduit.PrixDeVente);
                int indexPrixSolde = ExtensionsProduit.GetChiffreProduit(enTetes, eProduit.PrixSolde);
                int indexCodeBarre = ExtensionsProduit.GetChiffreProduit(enTetes, eProduit.CodeBarre);
                int indexPoids = ExtensionsProduit.GetChiffreProduit(enTetes, eProduit.LibellePoids);

                // Vérification que les colonnes existent
                if (indexReferenceExterne == -1
                    || indexPrixDeVente == -1
                    || indexPrixSolde == -1
                    || indexCodeBarre == -1
                    || indexPoids == -1)
                {
                    Console.WriteLine("Colonnes requises non trouvées dans le fichier CSV");
                    return produits;
                }

                // parcourir libellé court et prendre la taille du libellé max
                tailleMaxLibelleCourt = RecupererTailleMaxLibelleCourt(lignes, enTetesTemp, indexLibelleCourt);

                // Parcourir les lignes de données (à partir de la deuxième ligne)
                for (int i = 1; i < lignes.Length; i++)
                {
                    string[] valeursTemp = lignes[i].Split(';');
                    string[] valeurs = new string[valeursTemp.Length];

                    for (int j = 0; j < enTetesTemp.Length; j++)
                    {
                        valeurs[j] = valeursTemp[j].Trim('"', ' ', '\\');
                    }

                    var listIndex = new List<int>() { indexLibelleCourt, indexReferenceExterne, indexPrixDeVente, indexPrixSolde, indexCodeBarre };

                    // Vérifier que la ligne a suffisamment de colonnes
                    if (valeurs.Length > listIndex.Max())
                    {
                        string libelle = valeurs[indexLibelle];
                        string libelleCourt = PrendreLibelleLongSiCourtVide(valeurs, indexLibelle, indexLibelleCourt);

                        string libellePoidLitre = RecupereLibellePoids(libelle, valeurs[indexPoids]);

                        var produit = new Produit
                        {
                            LibelleCourt = libelleCourt,
                            ReferenceExterne = valeurs[indexReferenceExterne],
                            PrixDeVente = ChangePrix(valeurs, indexPrixDeVente),
                            PrixSolde = ChangePrix(valeurs, indexPrixSolde),
                            CodeBarre = valeurs[indexCodeBarre],
                            LibellePoids = libellePoidLitre,
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

        private static string RecupereLibellePoids(string libelle, string poids)
        {
            if (poids.Contains("0.00"))
            {
                return string.Empty;
            }

            libelle = libelle.ToUpper();

            if (libelle.Contains("L)") || libelle.Contains("CL)"))
            {
                return poids + " ML";
            }

            if (libelle.Contains("KG)") || libelle.Contains("G)") || libelle.Contains(" KG") || libelle.Contains(" G"))
            {
                return poids + " G";
            }

            return poids;
        }

        private static int RecupererTailleMaxLibelleCourt(string[] lignes, string[] enTetesTemp, int indexLibelleCourt)
        {
            int tailleMax = 0;
            for (int i = 1; i < lignes.Length; i++)
            {
                string[] valeursTemp = lignes[i].Split(';');
                string[] valeurs = new string[valeursTemp.Length];
                for (int j = 0; j < enTetesTemp.Length; j++)
                {
                    valeurs[j] = valeursTemp[j].Trim('"', ' ', '\\');
                }
                if (valeurs.Length > indexLibelleCourt)
                {
                    if (valeurs[indexLibelleCourt].Length > tailleMax)
                    {
                        tailleMax = valeurs[indexLibelleCourt].Length;
                    }
                }
            }

            if (tailleMax > tailleMaxLibelleCourt) tailleMax = tailleMaxLibelleCourt;

            return tailleMaxLibelleCourt;
        }

        private static string PrendreLibelleLongSiCourtVide(string[] valeurs, int indexLibelle, int indexLibelleCourt)
        {
            string libelleCourt = valeurs[indexLibelleCourt];
            if (string.IsNullOrEmpty(libelleCourt))
            {
                libelleCourt = valeurs[indexLibelle];
                            
            }
            libelleCourt = RaccourcirLibelleSiTropLong(libelleCourt);

            return libelleCourt.ToUpper(); // FARINE GRAND ÉPEAUTRE
        }

        private static string RaccourcirLibelleSiTropLong(string libelle)
        {
            var tabLibelle = libelle.Split(' ');
            string libelleRaccourci = libelle;
            if (libelle.Length > tailleMaxLibelleCourt && tabLibelle.Length > 1)
            {
                string libelleAConstruire = string.Empty;
                int i = 0;
                while ((libelleAConstruire.Length + tabLibelle[i].Length + 1) < tailleMaxLibelleCourt && i < tabLibelle.Length)
                {
                    libelleAConstruire += tabLibelle[i] + " ";
                    i++;
                }
                libelleRaccourci = libelleAConstruire.Trim();
            }

            return libelleRaccourci.ToUpper();
        }


        private static decimal ChangePrix(string[] valeurs, int index)
        {
            // arrondir 2 chiffrs après virgule
            var prixVente = Math.Round(decimal.Parse(valeurs[index].ToString().Replace('.', ',')) * 1000, 2);
            if (prixVente >= 800)
            {
                prixVente = prixVente / 1000;
                prixVente = Math.Round(prixVente, 2);
            }

            return prixVente;
        }
    }
}
