using ApiTspm.Entidades;
using AutoMapper;
using ApiTspm.DTOs;

namespace ApiTspm.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Seccion,SeccionCreationDTO>()
                    .ReverseMap();

            CreateMap<Seccion, SeccionCreationDTOPut>()
                   .ReverseMap();

            CreateMap<Galeria, GaleriaCreationDTO>()
                  .ReverseMap();

            CreateMap<Anuncio, AnuncioCreationDTO>()
                   .ReverseMap();

            CreateMap<Anuncio, AnuncioCreationDTOPut>()
                  .ReverseMap();


            CreateMap<SeccionCreationDTO, Seccion>()
                .ForMember(m => m.Imagen, options => options.Ignore());

            CreateMap<SeccionCreationDTOPut, Seccion>()
               .ForMember(m => m.Imagen, options => options.Ignore());

            CreateMap<GaleriaCreationDTO, Galeria>();

            CreateMap<AnuncioCreationDTO, Anuncio>()
               .ForMember(m => m.Portada, options => options.Ignore());

            CreateMap<AnuncioCreationDTOPut, Anuncio>()
            .ForMember(m => m.Portada, options => options.Ignore());


        }
    }
}
