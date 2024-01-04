using ApiTspm.DTOs;
using ApiTspm.Entidades;
using ApiTspm.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace ApiTspm.Controllers
{
    [ApiController]
    [Route("/api/secciones")]
    public class SeccionesController:Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;

        public SeccionesController(ApplicationDbContext applicationDbContext, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<Seccion>>> Get()
        {

            var seccionesVisibles = from s in applicationDbContext.Secciones
                                    where s.Visible == true
                                    select s;

            return await seccionesVisibles.ToListAsync();
        }

        [HttpGet("{id}",Name ="GetSeccion")]
        public async Task<ActionResult<Seccion>> Get(int id)
        {
            var seccion =  await applicationDbContext.Secciones.FirstOrDefaultAsync(c => c.Id == id);

            if (seccion ==null || seccion.Visible==false)
            {
                return NotFound();
            }

            return seccion;
        }

   

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] SeccionCreationDTO seccionPost)
        {

            string imagenUrl = "";
            string nombreImagen = "";


            if (seccionPost.imagen != null)
            {
                imagenUrl = await GuardarImagen(seccionPost.imagen);

                string[] valores = imagenUrl.Split("*");
                imagenUrl = valores[0];
                nombreImagen = valores[1];
                
            }

            var seccion = mapper.Map<Seccion>(seccionPost);

            seccion.Imagen = imagenUrl;
            seccion.NombreImagen = nombreImagen;
            seccion.Visible = true;

            applicationDbContext.Add(seccion);

            await applicationDbContext.SaveChangesAsync();

            return new CreatedAtActionResult(nameof(Get), "Secciones", new { id = seccion.Id }, seccion);

        }

        [HttpPut]
        public async Task<ActionResult> Put([FromForm] SeccionCreationDTOPut seccionPut)
        {
            

            var seccionFound = await applicationDbContext.Secciones.FirstOrDefaultAsync(s => s.Id == seccionPut.Id);

            if (seccionFound == null)
            {
                return NotFound();
            }

            var nombrePortadaBD = seccionFound.NombreImagen;
            var nombrePortadaAr = seccionPut.imagen.FileName.Split('.')[0];
            string imagenUrl = "";
            string nombreImg = "";


            if (nombrePortadaBD != nombrePortadaAr)
            {
                await almacenadorArchivos.Borrar(seccionFound.Imagen);

                if (seccionPut.imagen != null)
                {
                    imagenUrl = await GuardarImagen(seccionPut.imagen);
                    string[] valores = imagenUrl.Split("*");
                    imagenUrl = valores[0];
                    nombreImg = valores[1];
                }

            }

          

           var seccionOk = mapper.Map(seccionPut, seccionFound);

            if (nombrePortadaBD != nombrePortadaAr)
            {
                seccionOk.Imagen = imagenUrl;
                seccionOk.NombreImagen = nombreImg;

            }

            applicationDbContext.Entry(seccionOk).State=EntityState.Modified;

            await applicationDbContext.SaveChangesAsync();

            return NoContent();

        }



        private async Task<string> GuardarImagen(IFormFile imagen)
        {
            using var stream = new MemoryStream();

            await imagen.CopyToAsync(stream);

            var fileBytes = stream.ToArray();

            return await almacenadorArchivos.Crear(fileBytes, imagen.ContentType, Path.GetExtension(imagen.FileName),ConstanteDeAplicacion.ContenedoresDeAchivos.ContenedoresDeSecciones,Guid.NewGuid().ToString());
        }

  
        
    }
}
