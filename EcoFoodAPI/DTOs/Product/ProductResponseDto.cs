
namespace EcoFoodAPI.DTOs.Product
{
    public class ProductResponseDto
    {
        public int IdProduit { get; set; }
        public string Titre { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Prix { get; set; }
        public DateOnly? DateExpiration { get; set; }
        public string? ImageUrl { get; set; }
        public string? Statut { get; set; }
        public int IdUser { get; set; }
        public string NomUser { get; set; } = null!;
        public string PrenomUser { get; set; } = null!;
        public string? VilleUser { get; set; }
        public int IdCategorie { get; set; }
        public string NomCategorie { get; set; } = null!;
        public DateTime DateCreation { get; set; }
    }
}