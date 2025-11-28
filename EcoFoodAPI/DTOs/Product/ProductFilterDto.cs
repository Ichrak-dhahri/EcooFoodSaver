namespace EcoFoodAPI.DTOs.Product
{
    public class ProductFilterDto
    {
        public int? IdCategorie { get; set; }
        public string? Ville { get; set; }
        public decimal? PrixMin { get; set; }
        public decimal? PrixMax { get; set; }
        public string? Statut { get; set; }
        public bool? ProcheExpiration { get; set; } // Produits qui expirent dans moins de 3 jours
        public string? Search { get; set; } // Recherche dans titre et description
    }
}