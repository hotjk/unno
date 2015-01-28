using Grit.Unno.Repository.File;
using Grit.Unno.Repository.Mongodb;
using Grit.Unno.Repository.MySql;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grit.Unno.Web.App_Start
{
    public static class BootStrapper
    {
        public static IKernel Kernel { get; private set; }

        public static void BootStrap()
        {
            AddIocBindings();
        }

        public static void Dispose()
        {
            if (Kernel != null)
            {
                Kernel.Dispose();
            }
        }

        private static void AddIocBindings()
        {
            Kernel = new StandardKernel();

            switch(System.Configuration.ConfigurationManager.AppSettings["Repository"])
            {
                case "File":
                    // Repository.File
                    var fileOptions = new FileOptions(HttpContext.Current.Server.MapPath("files"));
                    Kernel.Bind<IUnitRepository>().To<Grit.Unno.Repository.File.UnitRepository>().InSingletonScope()
                        .WithConstructorArgument("options", fileOptions);
                    Kernel.Bind<INodeRepository>().To<Grit.Unno.Repository.File.NodeRepository>().InSingletonScope()
                        .WithConstructorArgument("options", fileOptions);
                    break;
                case "MongoDB":
                    // Repository.MongoDB
                    var mongoDBOptions = new MongoDBOptions("mongodb://localhost", false);
                    Kernel.Bind<IUnitRepository>().To<Grit.Unno.Repository.Mongodb.UnitRepository>().InSingletonScope()
                        .WithConstructorArgument("options", mongoDBOptions);
                    Kernel.Bind<INodeRepository>().To<Grit.Unno.Repository.Mongodb.NodeRepository>().InSingletonScope()
                        .WithConstructorArgument("options", mongoDBOptions);
                    break;
                case "MySql":
                    // Repository.MySql
                    var sqlOptions = new SqlOptions(System.Configuration.ConfigurationManager.ConnectionStrings["mysql"].ConnectionString);
                    Kernel.Bind<IUnitRepository>().To<Grit.Unno.Repository.MySql.UnitRepository>().InSingletonScope()
                        .WithConstructorArgument("options", sqlOptions);
                    Kernel.Bind<INodeRepository>().To<Grit.Unno.Repository.MySql.NodeRepository>().InSingletonScope()
                        .WithConstructorArgument("options", sqlOptions);
                    Kernel.Bind<IStructureRepository>().To<Grit.Unno.Repository.MySql.StructureRepository>().InSingletonScope()
                        .WithConstructorArgument("options", sqlOptions);
                    break;
                default:
                    throw new Exception("Invalid repository configuration.");
            }

            Kernel.Bind<IUnnoService>().To<UnnoService>();
        }
    }
}