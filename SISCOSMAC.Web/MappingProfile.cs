using AutoMapper;
using SISCOSMAC.DAL.Models;
using SISCOSMAC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISCOSMAC.Web
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
           //CreateMap<source,destination>
            CreateMap<Departamento, DepartamentoViewModel>()
                .ForMember(d => d.NombreDepto, y => y.MapFrom(z => z.NombreDepartamento));
            CreateMap<DepartamentoViewModel, Departamento>()
                .ForMember(d=>d.NombreDepartamento,y=>y.MapFrom(z=>z.NombreDepto));

            CreateMap<Usuario, UsuarioViewModel>()
                .ForMember(x => x.NombreDeptoPer, y => y.MapFrom(z => z.departamento.NombreDepartamento));
            CreateMap<UsuarioViewModel, Usuario>();

            CreateMap<SolicitudMantenimientoCorrectivoVM, SolicitudMantenimientoCorrectivo>();
            CreateMap<SolicitudMantenimientoCorrectivo, SolicitudMantenimientoCorrectivoVM>();


        }
    }
}
