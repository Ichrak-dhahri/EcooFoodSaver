// Controllers/ProductController.cs
using EcoFoodAPI.DTOs.Product;
using EcoFoodAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcoFoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Récupérer tous les produits avec filtres optionnels (Public)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductFilterDto filter)
        {
            var products = await _productService.GetAllProductsAsync(filter);

            return Ok(new
            {
                success = true,
                message = "Liste des produits récupérée avec succès",
                count = products.Count,
                data = products
            });
        }

        /// <summary>
        /// Récupérer un produit par ID (Public)
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Produit non trouvé"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Produit récupéré avec succès",
                data = product
            });
        }

        /// <summary>
        /// Récupérer mes produits (Authentifié) 
        /// </summary>
        [HttpGet("my-products")]
        [Authorize]
        public async Task<IActionResult> GetMyProducts()
        {
            var userId = GetCurrentUserId();
            var products = await _productService.GetMyProductsAsync(userId);

            return Ok(new
            {
                success = true,
                message = "Vos produits récupérés avec succès",
                count = products.Count,
                data = products
            });
        }

        /// <summary>
        /// Ajouter un nouveau produit (Authentifié) 🔒
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Données invalides",
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            var userId = GetCurrentUserId();
            var product = await _productService.CreateProductAsync(userId, createDto);

            if (product == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Impossible de créer le produit. Vérifiez que la catégorie existe."
                });
            }

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = product.IdProduit },
                new
                {
                    success = true,
                    message = "Produit créé avec succès",
                    data = product
                });
        }

        /// <summary>
        /// Modifier un produit (Authentifié - Propriétaire uniquement) 🔒
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Données invalides",
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            var userId = GetCurrentUserId();
            var product = await _productService.UpdateProductAsync(userId, id, updateDto);

            if (product == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Impossible de modifier le produit. Vous n'êtes pas le propriétaire ou le produit n'existe pas."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Produit modifié avec succès",
                data = product
            });
        }

        /// <summary>
        /// Supprimer un produit (Authentifié - Propriétaire ou Admin) 🔒
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");

            var result = await _productService.DeleteProductAsync(userId, id, isAdmin);

            if (!result)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Impossible de supprimer le produit. Vous n'êtes pas le propriétaire ou le produit n'existe pas."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Produit supprimé avec succès"
            });
        }

        /// <summary>
        /// Rechercher des produits proches de l'expiration (Public)
        /// </summary>
        [HttpGet("proche-expiration")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsProcheExpiration()
        {
            var filter = new ProductFilterDto { ProcheExpiration = true };
            var products = await _productService.GetAllProductsAsync(filter);

            return Ok(new
            {
                success = true,
                message = "Produits proches de l'expiration récupérés",
                count = products.Count,
                data = products
            });
        }

        /// <summary>
        /// Rechercher des produits par ville (Public)
        /// </summary>
        [HttpGet("by-city/{ville}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsByCity(string ville)
        {
            var filter = new ProductFilterDto { Ville = ville };
            var products = await _productService.GetAllProductsAsync(filter);

            return Ok(new
            {
                success = true,
                message = $"Produits à {ville} récupérés",
                count = products.Count,
                data = products
            });
        }

        // Méthode utilitaire pour récupérer l'ID de l'utilisateur connecté
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim!);
        }
    }
}