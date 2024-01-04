namespace ApiTspm.Utilidades
{
    public interface IAlmacenadorArchivos
    {
        public Task<string> Crear(byte[] file, string contentType, string extension, string container, string nombre);

        public Task<string> CrearGaleria(byte[] file, string contentType, string extension, string container, string nombre, int idSeccion, int idAnuncio);

        public Task Borrar(string ruta);


        public Task BorrarGaleria(string ruta, int idSeccion, int idAnuncio);
        
        }
}
