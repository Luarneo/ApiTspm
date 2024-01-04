namespace ApiTspm.Entidades
{
    public class Galeria
    {
        public int Id { get; set; }
        public string Ruta { get; set; }
        public int IdSeccion { get; set; }
        public int IdAnuncio { get; set; }        
        public bool Visible { get; set; }
        public string Nombre { get; set; }

    }
}
