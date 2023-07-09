using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
     public class Publisher
     {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
         public string Name { get; set; }

         public string Email { get; set; }
     }
}
