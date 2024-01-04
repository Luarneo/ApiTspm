namespace ApiTspm.DTOs
{
    public class GaleriaCreationDTO
    {
        public string Ruta { get; set; }
        public int IdAnuncio { get; set; }

        public int IdSeccion { get; set; }
        public bool Visible { get; set; }

        public string Nombre { get; set; }
    }
}
