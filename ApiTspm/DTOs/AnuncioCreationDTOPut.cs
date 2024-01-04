using System.ComponentModel.DataAnnotations;

namespace ApiTspm.DTOs
{
    public class AnuncioCreationDTOPut
    {
        public int Id { get; set; }
        public string titulo { get; set; }

        public string descripcion { get; set; }

        public IFormFile portada { get; set; }

        public string telefono { get; set; }

        public int idSeccion { get; set; }

        public string direccion { get; set; }

        public int valoracion { get; set; }

        public int visitas { get; set; }

        public List<IFormFile> galeria { get; set; }

        public bool visible { get; set; }
    }
}
