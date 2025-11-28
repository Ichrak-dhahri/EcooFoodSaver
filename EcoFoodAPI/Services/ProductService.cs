using EcoFoodAPI.Data;
using EcoFoodAPI.DTOs.Product;
using EcoFoodAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoFoodAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly EcoFoodDbContext _context;

        public ProductService(EcoFoodDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductResponseDto>> GetAllProductsAsync(ProductFilterDto? filter = null)
        {
            var query = _context.Produits
                .Include(p => p.IdUserNavigation)
                .Include(p => p.IdCategorieNavigation)
                .AsQueryable();

            // Appliquer les filtres
            if (filter != null)
            {
                if (filter.IdCategorie.HasValue)
                    query = query.Where(p => p.IdCategorie == filter.IdCategorie.Value);

                if (!string.IsNullOrEmpty(filter.Ville))
                    query = query.Where(p => p.IdUserNavigation.Ville == filter.Ville);

                if (filter.PrixMin.HasValue)
                    query = query.Where(p => p.Prix >= filter.PrixMin.Value);

                if (filter.PrixMax.HasValue)
                    query = query.Where(p => p.Prix <= filter.PrixMax.Value);

                if (!string.IsNullOrEmpty(filter.Statut))
                    query = query.Where(p => p.Statut == filter.Statut);

                if (filter.ProcheExpiration == true)
                {
                    var dateLimit = DateOnly.FromDateTime(DateTime.Now.AddDays(3));
                    query = query.Where(p => p.DateExpiration.HasValue && p.DateExpiration <= dateLimit);
                }

                if (!string.IsNullOrEmpty(filter.Search))
                {
                    var searchLower = filter.Search.ToLower();
                    query = query.Where(p =>
                        p.Titre.ToLower().Contains(searchLower) ||
                        (p.Description != null && p.Description.ToLower().Contains(searchLower))
                    );
                }
            }

            var products = await query
                .OrderByDescending(p => p.IdProduit)
                .Select(p => new ProductResponseDto
                {
                    IdProduit = p.IdProduit,
                    Titre = p.Titre,
                    Description = p.Description,
                    Prix = p.Prix,
                    DateExpiration = p.DateExpiration,
                    ImageUrl = p.ImageUrl,
                    Statut = p.Statut,
                    IdUser = p.IdUser,
                    NomUser = p.IdUserNavigation.Nom,
                    PrenomUser = p.IdUserNavigation.Prenom,
                    VilleUser = p.IdUserNavigation.Ville,
                    IdCategorie = p.IdCategorie,
                    NomCategorie = p.IdCategorieNavigation.NomCategorie,
                    DateCreation = DateTime.Now // Ajoutez une colonne DateCreation dans votre DB si nécessaire
                })
                .ToListAsync();

            return products;
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Produits
                .Include(p => p.IdUserNavigation)
                .Include(p => p.IdCategorieNavigation)
                .FirstOrDefaultAsync(p => p.IdProduit == id);

            if (product == null)
                return null;

            return new ProductResponseDto
            {
                IdProduit = product.IdProduit,
                Titre = product.Titre,
                Description = product.Description,
                Prix = product.Prix,
                DateExpiration = product.DateExpiration,
                ImageUrl = product.ImageUrl,
                Statut = product.Statut,
                IdUser = product.IdUser,
                NomUser = product.IdUserNavigation.Nom,
                PrenomUser = product.IdUserNavigation.Prenom,
                VilleUser = product.IdUserNavigation.Ville,
                IdCategorie = product.IdCategorie,
                NomCategorie = product.IdCategorieNavigation.NomCategorie,
                DateCreation = DateTime.Now
            };
        }

        public async Task<List<ProductResponseDto>> GetMyProductsAsync(int userId)
        {
            var products = await _context.Produits
                .Include(p => p.IdUserNavigation)
                .Include(p => p.IdCategorieNavigation)
                .Where(p => p.IdUser == userId)
                .OrderByDescending(p => p.IdProduit)
                .Select(p => new ProductResponseDto
                {
                    IdProduit = p.IdProduit,
                    Titre = p.Titre,
                    Description = p.Description,
                    Prix = p.Prix,
                    DateExpiration = p.DateExpiration,
                    ImageUrl = p.ImageUrl,
                    Statut = p.Statut,
                    IdUser = p.IdUser,
                    NomUser = p.IdUserNavigation.Nom,
                    PrenomUser = p.IdUserNavigation.Prenom,
                    VilleUser = p.IdUserNavigation.Ville,
                    IdCategorie = p.IdCategorie,
                    NomCategorie = p.IdCategorieNavigation.NomCategorie,
                    DateCreation = DateTime.Now
                })
                .ToListAsync();

            return products;
        }

        public async Task<ProductResponseDto?> CreateProductAsync(int userId, CreateProductDto createDto)
        {
            // Vérifier que la catégorie existe
            var categoryExists = await _context.Categories.AnyAsync(c => c.IdCategorie == createDto.IdCategorie);
            if (!categoryExists)
                return null;

            var newProduct = new Produit
            {
                Titre = createDto.Titre,
                Description = createDto.Description,
                Prix = createDto.Prix,
                DateExpiration = createDto.DateExpiration,
                ImageUrl = createDto.ImageUrl,
                Statut = "Disponible", // Par défaut
                IdUser = userId,
                IdCategorie = createDto.IdCategorie
            };

            _context.Produits.Add(newProduct);
            await _context.SaveChangesAsync();

            return await GetProductByIdAsync(newProduct.IdProduit);
        }

        public async Task<ProductResponseDto?> UpdateProductAsync(int userId, int productId, UpdateProductDto updateDto)
        {
            var product = await _context.Produits
                .FirstOrDefaultAsync(p => p.IdProduit == productId);

            if (product == null)
                return null;

            // Vérifier que l'utilisateur est le propriétaire du produit
            if (product.IdUser != userId)
                return null;

            // Vérifier que la catégorie existe
            var categoryExists = await _context.Categories.AnyAsync(c => c.IdCategorie == updateDto.IdCategorie);
            if (!categoryExists)
                return null;

            product.Titre = updateDto.Titre;
            product.Description = updateDto.Description;
            product.Prix = updateDto.Prix;
            product.DateExpiration = updateDto.DateExpiration;
            product.ImageUrl = updateDto.ImageUrl;
            product.IdCategorie = updateDto.IdCategorie;

            if (!string.IsNullOrEmpty(updateDto.Statut))
                product.Statut = updateDto.Statut;

            await _context.SaveChangesAsync();

            return await GetProductByIdAsync(product.IdProduit);
        }

        public async Task<bool> DeleteProductAsync(int userId, int productId, bool isAdmin = false)
        {
            var product = await _context.Produits
                .FirstOrDefaultAsync(p => p.IdProduit == productId);

            if (product == null)
                return false;

            // Vérifier que l'utilisateur est le propriétaire ou est admin
            if (!isAdmin && product.IdUser != userId)
                return false;

            _context.Produits.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}