using ApiTspm.DTOs;
using ApiTspm.Entidades;
using ApiTspm.Migrations;
using ApiTspm.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiTspm.Controllers
{
    [ApiController]
    [Route("/api/anuncios")]
    public class AnunciosController: Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;

        public AnunciosController(ApplicationDbContext applicationDbContext, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
                this.almacenadorArchivos = almacenadorArchivos;
                this.mapper = mapper;   
                this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Anuncio>>> Get()
        {
            
            var anunciosVisibles = from a in applicationDbContext.Anuncios
                                    where a.Visible == true
                                    select a;

            return await anunciosVisibles.ToListAsync();
        }

        [HttpGet("idSeccion", Name = "GetAnunciosSeccion")]
        public async Task<ActionResult<List<Anuncio>>> GetSeccion(int idSeccion)
        {


            var anunciosSeccion = from a in applicationDbContext.Anuncios
                                   where a.Visible == true && a.IdSeccion == idSeccion
                                   select a;



            return await anunciosSeccion.ToListAsync();
        }

        [HttpGet("id", Name ="GetAnuncio")]
        public async Task<ActionResult<Anuncio>>Get(int id)
        {
            var anuncio = await applicationDbContext.Anuncios.FirstOrDefaultAsync(a => a.Id == id);

          
            if (anuncio == null || anuncio.Visible==false)
            {
                return NotFound();
            }

            return anuncio;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] AnuncioCreationDTO anuncioPost)
        {
            
            string imagenUrl = "";
            string nombreImagen = "";

            anuncioPost.direccion =
                anuncioPost.direccion + ' ' +
                anuncioPost.numeroext + ' ' +
                anuncioPost.colonia + ' ' +
                anuncioPost.municipio + ' ' +
                anuncioPost.estado;

            List <GaleriaCreationDTO> galeriaImagenes = new List<GaleriaCreationDTO>();    

            var anuncio = mapper.Map<Anuncio>(anuncioPost);

            anuncio.Portada = "Pendiente";
            anuncio.NombrePortada = "Pendiente";
            anuncio.Visible = true;

            applicationDbContext.Add(anuncio);

            await applicationDbContext.SaveChangesAsync();

          

            if (anuncioPost.portada != null)
            {
                imagenUrl = await GuardarImagenGaleria(anuncioPost.portada, anuncioPost.idSeccion, anuncio.Id);                

                string[] valores = imagenUrl.Split('*');

                imagenUrl = valores[0];

                nombreImagen = valores[1];
            }

            

            anuncio.Portada = imagenUrl;
            anuncio.NombrePortada = nombreImagen;
            

            applicationDbContext.Update(anuncio);



            if (anuncioPost.galeria != null)
            {
                foreach (var imagengal in anuncioPost.galeria)
                {
                    var imagenUrlGal = "";
                    var nombreImagenGal = "";
                    imagenUrlGal = await GuardarImagenGaleria(imagengal, anuncioPost.idSeccion, anuncio.Id);
                    

                    string[] valores = imagenUrlGal.Split('*');

                    imagenUrlGal = valores[0];

                    nombreImagenGal = valores[1];

                    galeriaImagenes.Add(new GaleriaCreationDTO
                    {
                        IdSeccion = anuncioPost.idSeccion,                        
                        Ruta = imagenUrlGal,
                        Visible = true,
                        IdAnuncio = anuncio.Id,
                        Nombre = nombreImagenGal,
                    });

                }

            }


            foreach (var galeriaItem in galeriaImagenes)
            {
                var galeria = mapper.Map<Galeria>(galeriaItem);
                applicationDbContext.Add(galeria);
            }


            await applicationDbContext.SaveChangesAsync();

            return new CreatedAtActionResult(nameof(Get), "Secciones", new { id = anuncio.Id }, anuncio);

        }

        [HttpPut]
        public async Task<ActionResult> Put([FromForm]AnuncioCreationDTOPut anuncioPut)
        {

            List<GaleriaCreationDTO> galeriaImagenes = new List<GaleriaCreationDTO>();
            var anuncioFound = await applicationDbContext.Anuncios.FirstOrDefaultAsync(a=>a.Id==anuncioPut.Id);

            var nombrePortadaBD = anuncioFound.NombrePortada;
            var nombrePortadaAr = anuncioPut.portada.FileName.Split('.')[0];


            if (nombrePortadaBD != nombrePortadaAr)
            {
                await almacenadorArchivos.Borrar(anuncioFound.Portada);
            }

            var imagenesGaleria =  from c in  applicationDbContext.Galerias
                                   where c.IdAnuncio == anuncioPut.Id && c.Visible==true
                                   select c;


            //ocultar imagenes eliminadas por usuario

            foreach (var imagegal in imagenesGaleria)
            {
                
                var imgexist = anuncioPut.galeria.Where(x => x.FileName.Split('.')[0] == imagegal.Nombre);

                if (!imgexist.Any())
                {
                    await almacenadorArchivos.BorrarGaleria(imagegal.Ruta, anuncioFound.IdSeccion, anuncioFound.Id); 
                    var idimg = imagegal.Id;

                    var imageGalFound =  applicationDbContext.Galerias.FirstOrDefault(i => i.Id == imagegal.Id);

                    imageGalFound.Visible = false;

                    applicationDbContext.Entry(imageGalFound).State = EntityState.Modified;

                }
            }


            //insertar las imagenes nuevas

            foreach(var img in anuncioPut.galeria)
            {
                var nomimg = img.FileName.Split('.')[0];

                var imgexist = imagenesGaleria.Where(x => x.Nombre == nomimg);

                if (!imgexist.Any())
                {
                   
                    var imagenUrlGal = await GuardarImagenGaleria(img, anuncioPut.idSeccion, anuncioPut.Id);                 

                    string[] valores = imagenUrlGal.Split('*');

                    imagenUrlGal = valores[0];

                    var nombreImagen = valores[1];

                    galeriaImagenes.Add(new GaleriaCreationDTO
                    {
                        IdSeccion = anuncioPut.idSeccion,
                        Ruta = imagenUrlGal,
                        Visible = true,
                        IdAnuncio = anuncioPut.Id,
                        Nombre = nombreImagen
                    });
                }

            }

            foreach (var galeriaItem in galeriaImagenes)
            {
                var galeria = mapper.Map<Galeria>(galeriaItem);
                applicationDbContext.Add(galeria);
            }

            //Evaluar si se actualiza nueva portada de anuncio

            string imagenUrl = "";
            var nombreImagenPortada = "";

            if (nombrePortadaBD != nombrePortadaAr)
            {
                imagenUrl = await GuardarImagenGaleria(anuncioPut.portada,anuncioFound.IdSeccion,anuncioFound.Id);

                string[] valores = imagenUrl.Split('*');

                imagenUrl = valores[0];

                nombreImagenPortada = valores[1];
                
            }

            var anuncioOk = mapper.Map(anuncioPut, anuncioFound);

            if (nombrePortadaBD != nombrePortadaAr)
            {
                anuncioOk.Portada = imagenUrl;
                anuncioOk.NombrePortada = nombreImagenPortada;
            }            

            applicationDbContext.Entry(anuncioOk).State = EntityState.Modified;

            await applicationDbContext.SaveChangesAsync();

            return NoContent();

        }



        private async Task<string> GuardarImagenGaleria(IFormFile imagen, int idSeccion, int idAnuncio)
        {
            using var stream = new MemoryStream();

            await imagen.CopyToAsync(stream);

            var fileBytes = stream.ToArray();

            return await almacenadorArchivos.CrearGaleria(fileBytes, imagen.ContentType, Path.GetExtension(imagen.FileName), ConstanteDeAplicacion.ContenedoresDeAchivos.ContenedoresDeAnuncios, Guid.NewGuid().ToString(), idSeccion, idAnuncio);
        }

    }
}
