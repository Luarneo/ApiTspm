namespace ApiTspm.Utilidades
{
    public class AlmacenadorDeArchivosEnLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AlmacenadorDeArchivosEnLocal(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }
        //public Task Borrar(string ruta, string container)
        public Task Borrar(string ruta)
        {
            string wwwrootPath = webHostEnvironment.ContentRootPath;

            wwwrootPath = wwwrootPath + "wwwroot\\imagenes\\secciones\\";

            if (string.IsNullOrEmpty(wwwrootPath))
            {
                throw new Exception();
            }

            var nombreArchivo = Path.GetFileName(ruta);

            string pathFinal = Path.Combine(wwwrootPath, nombreArchivo);

            if(File.Exists(pathFinal))
            {
                File.Delete(pathFinal);
            }

            return Task.CompletedTask;
        }

        public Task BorrarGaleria(string ruta, int idSeccion, int idAnuncio)
        {
            string wwwrootPath = webHostEnvironment.ContentRootPath;

            wwwrootPath = wwwrootPath + "wwwroot\\imagenes\\anuncios\\" + idSeccion + "\\" + idAnuncio + "\\";

            if (string.IsNullOrEmpty(wwwrootPath))
            {
                throw new Exception();
            }

            var nombreArchivo = Path.GetFileName(ruta);

            string pathFinal = Path.Combine(wwwrootPath, nombreArchivo);

            if (File.Exists(pathFinal))
            {
                File.Delete(pathFinal);
            }

            return Task.CompletedTask;
        }

        public async Task<string> Crear(byte[] file, string contentType, string extension, string container, string nombre)
        {
            string wwwrootPath = webHostEnvironment.ContentRootPath;

            if(string.IsNullOrEmpty(wwwrootPath))
            {
                throw new Exception();
            }

            string wwwrootPathFile = wwwrootPath + "wwwroot\\";

            string carpetaArchivo = Path.Combine(wwwrootPathFile, container);

            if(!Directory.Exists(carpetaArchivo))
            {
                Directory.CreateDirectory(carpetaArchivo);  
            }

            string nombreFinal = $"{nombre}{extension}";

            string rutaFinal = Path.Combine(carpetaArchivo, nombreFinal);

            await File.WriteAllBytesAsync(rutaFinal, file);

            string urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";

            string dbUrl = Path.Combine(urlActual, container, nombreFinal).Replace("\\", "/");

            return  dbUrl+"*"+nombre;
        }

        public async Task<string> CrearGaleria(byte[] file, string contentType, string extension, string container, string nombre, int idSeccion, int idAnuncio)
        {
            string wwwrootPath = webHostEnvironment.ContentRootPath;

            if (string.IsNullOrEmpty(wwwrootPath))
            {
                throw new Exception();
            }

            container = container +"\\"+ idSeccion + "\\" + idAnuncio;

            string wwwrootPathFile = wwwrootPath + "wwwroot\\";

            string carpetaArchivo = Path.Combine(wwwrootPathFile, container);

            if (!Directory.Exists(carpetaArchivo))
            {
                Directory.CreateDirectory(carpetaArchivo);
            }

            string nombreFinal = $"{nombre}{extension}";

            string rutaFinal = Path.Combine(carpetaArchivo, nombreFinal);

            await File.WriteAllBytesAsync(rutaFinal, file);

            string urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";

            string dbUrl = Path.Combine(urlActual, container, nombreFinal).Replace("\\", "/");

            return dbUrl+"*"+ nombre;
        }
    }
}
