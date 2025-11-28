using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoFoodAPI.Models
{
    [Table("message")]
    public partial class Message
    {
        public int IdMessage { get; set; }

        public string Texte { get; set; } = null!;

        public DateTime? DateEnvoi { get; set; }

        public int IdSender { get; set; }

        public int IdReceiver { get; set; }

        public int IdProduit { get; set; }

        public virtual Produit IdProduitNavigation { get; set; } = null!;

        public virtual User IdReceiverNavigation { get; set; } = null!;

        public virtual User IdSenderNavigation { get; set; } = null!;
    }
}