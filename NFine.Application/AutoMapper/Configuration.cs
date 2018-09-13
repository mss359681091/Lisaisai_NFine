using AutoMapper;

namespace NFine.Application.AutoMapper
{
    public class Configuration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<SystemManageProfile>();
            });
        }
    }
}