using System;

namespace GenererEtiquettes
{
    public class Produit
    {
        public string Libelle { get; set; }
        public string LibelleCourt { get; set; }
        public string ReferenceExterne { get; set; }
        public decimal PrixDeVente { get; set; }
        public decimal PrixSolde { get; set; }
        public string CodeBarre { get; set; }
        public int ChiffreProduit { get; set; }
        public string LibellePoids { get; set; }
        public bool EstSelectionne { get; set; } = true;

        
    }

    public static class ExtensionsProduit
    {
        public static string RecupereNomColonne(eProduit colonne)
        {
            switch (colonne)
            {
                case eProduit.Libelle:
                    return "Libellé";
                case eProduit.LibelleCourt:
                    return "Libellé court";
                case eProduit.ReferenceExterne:
                    return "Référence externe";
                case eProduit.PrixDeVente:
                    return "Prix de vente";
                case eProduit.PrixSolde:
                    return "Prix soldé";
                case eProduit.CodeBarre:
                    return "Code-barres";
                case eProduit.LibellePoids:
                    return "Poids";
                default:
                    return string.Empty;
            }
        }

        public static int GetChiffreProduit(string[] enTetes, eProduit eProduit)
        {
            string nomColonne = $@"{RecupereNomColonne(eProduit)}";
            return Array.IndexOf(enTetes, nomColonne) ;
        }
    }

    public enum eProduit
    {
         Libelle,
         LibelleCourt,
         ReferenceExterne,
         PrixDeVente,
         PrixSolde,
         CodeBarre,
         LibellePoids,
    }
}
