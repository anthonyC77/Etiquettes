using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateurEtiquettes
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
    }
}
