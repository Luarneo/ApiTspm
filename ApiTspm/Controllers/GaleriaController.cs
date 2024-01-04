using ApiTspm.DTOs;
using ApiTspm.Entidades;
using ApiTspm.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ApiTspm.Controllers
{
    [ApiController]
    [Route("/api/galeria")]
    public class GaleriaController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public GaleriaController(ApplicationDbContext applicationDbContext)
        {
                this.applicationDbContext = applicationDbContext;
        }
       
        [HttpGet]
        public ActionResult<List<GaleriaGetDTO>> Get(int idanuncio)
        {
            List<GaleriaGetDTO> GaleriaDTO = new List<GaleriaGetDTO>();

            var imagenesVisibles = from a in applicationDbContext.Galerias
                                   where a.Visible == true && a.IdAnuncio== idanuncio
                                   select a;

            foreach (var imagen in imagenesVisibles)
            {
                GaleriaGetDTO elementGal = new GaleriaGetDTO();

                //elementGal.title=imagen.Id.ToString();
                elementGal.thumbImage = imagen.Ruta;
                elementGal.image = imagen.Ruta;
                //elementGal.alt= imagen.Nombre;

                GaleriaDTO.Add(elementGal);

            }

            return  GaleriaDTO.ToList();
        }
    }
}
