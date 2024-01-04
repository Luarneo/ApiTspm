using System.ComponentModel.DataAnnotations;

namespace ApiTspm.DTOs
{
    public class SeccionCreationDTOPut
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string nombreSeccion { get; set; }


        public IFormFile imagen { get; set; }


        public bool Visible { get; set; }
    }
}
