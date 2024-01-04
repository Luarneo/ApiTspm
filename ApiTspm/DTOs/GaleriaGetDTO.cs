using Microsoft.AspNetCore.Mvc;

namespace ApiTspm.DTOs
{
    public class GaleriaGetDTO
    {
        public string image { get; set; }
        public string thumbImage { get; set; }
        //public string title { get; set; }
        //public string alt { get; set; }

        internal Task<ActionResult<List<GaleriaGetDTO>>> ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
