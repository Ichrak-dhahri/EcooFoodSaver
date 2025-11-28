using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoFoodAPI.Models
{
    [Table("user")]  // 
    public partial class User
    {
        public int IdUser { get; set; }

        public string Nom { get; set; } = null!;

        public string Prenom { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string MotDePasse { get; set; } = null!;

        public string? Ville { get; set; }

        public decimal? MoyenneRating { get; set; }

        public int? TotalRatingsRecus { get; set; }

        public int? TotalRatingsDonnes { get; set; }

        public string? Role { get; set; }

        public virtual ICollection<Message> MessageIdReceiverNavigations { get; set; } = new List<Message>();

        public virtual ICollection<Message> MessageIdSenderNavigations { get; set; } = new List<Message>();

        public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();

        public virtual ICollection<Rating> RatingIdFromUserNavigations { get; set; } = new List<Rating>();

        public virtual ICollection<Rating> RatingIdToUserNavigations { get; set; } = new List<Rating>();

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
