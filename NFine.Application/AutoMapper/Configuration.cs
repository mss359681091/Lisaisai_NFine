using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

 namespace NFine.Application.AutoMapper
{
    public class Configuration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
     
            });
        }
    }
}