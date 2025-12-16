using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging.Abstractions;

namespace LockyLuke.AuthAPI.Configuration.DtoMapper
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMapper>();    
            }, new NullLoggerFactory());

            return config.CreateMapper();
        });

        public static IMapper Mapper => lazy.Value;
    }
}
