using System.ComponentModel.DataAnnotations;

namespace ApiTspm.DTOs
{
    public class SeccionCreationDTO
    {
        [Required]
        public string nombreSeccion { get; set; }

     
        public IFormFile imagen { get; set; }

      
    }
}
