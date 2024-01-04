namespace ApiTspm.Entidades
{
    public class Anuncio
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Portada { get; set; }
        public string Telefono { get; set; }
        public int IdSeccion { get; set; }
        public string Direccion { get; set; }
        public int Valoracion { get; set; }
        public int Visitas { get; set; }
        public string NombrePortada { get; set; }
        public bool Visible { get; set; }


        //public List<string> Galeria { get; set; }  //No se pudo incluir en la migración, requeria Primary Key
    }
}
