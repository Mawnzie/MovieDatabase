using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;


namespace MvcMovie.Models

{

    public class MovieGenre
    {
        public int Id { get; set; }

        [DisplayName("Genre")]
        public string Type { get; set; }


        
    }
}