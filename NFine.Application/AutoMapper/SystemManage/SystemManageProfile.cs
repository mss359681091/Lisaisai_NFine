using AutoMapper;
using NFine.Domain;
using NFine.Domain.Entity.SystemManage;

namespace NFine.Application.AutoMapper
{
    class SystemManageProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ModuleButtonEntity, ModuleButtonDto>();
        }
    }
}
