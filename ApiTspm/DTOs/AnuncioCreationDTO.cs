using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace ApiTspm.DTOs
{
    public class AnuncioCreationDTO
    {
        [Required]
        public string titulo { get; set; }

        public string descripcion { get; set; }

        public IFormFile portada { get; set; }

        public string telefono { get; set; }

        public int idSeccion { get; set; }

        public string direccion { get; set; }
      
        public  IFormFile[] galeria { get; set; }
        public string numeroext { get; set; }
        public string colonia { get; set; }
        public string municipio { get; set; }
        public string estado { get; set; }


    }
}
