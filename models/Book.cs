using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [Required]
        public int AuthorId { get; set; }
        
        [Required]
        public int GenreId { get; set; }
        
        public int PublishYear { get; set; }
        
        [MaxLength(20)]
        public string ISBN { get; set; }
        
        public int QuantityInStock { get; set; }
        
        public virtual Author Author { get; set; }
        public virtual Genre Genre { get; set; }
    }
}