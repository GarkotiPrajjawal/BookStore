using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Dto
{
    public class BooksDto
    {
        public String Title;
        [Required]
        public String Author;
        [Required]
        public String Publisher;
        public String ISBN;
        public String Yearofpublication;
    }
}
