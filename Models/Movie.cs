using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace MvcMovie.Models

{

    public class Movie
    {
        public int MovieId { get; set; }

        [DisplayName("Genre")]
        public int GenreId { get; set; }   // foreign key property

        public MovieGenre? Genre { get; set; }

        public string Title { get; set; }


        public string? FileName { get; set; }

        [DisplayName("Image")]
        public byte[]? File { get; set; }

        [NotMapped]
        public IFormFile FileForm { get; set; }
        


    }
}