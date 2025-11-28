using System.ComponentModel.DataAnnotations;

namespace EcoFoodAPI.DTOs.Product
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Le titre est requis")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        public string Titre { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
        public string? Description { get; set; }

        
        [Range(0, 999999, ErrorMessage = "Le prix doit être entre 0 et 999999")]
        public decimal Prix { get; set; }

        public DateOnly? DateExpiration { get; set; }

        [Url(ErrorMessage = "L'URL de l'image n'est pas valide")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "La catégorie est requise")]
        public int IdCategorie { get; set; }
    }
}