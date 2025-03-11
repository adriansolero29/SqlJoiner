using Autofac;
using SqlJoiner.DataAccess;
using SqlJoiner.Interfaces.DataAccess;
using SqlJoiner.Interfaces.Repository;
using SqlJoiner.Interfaces.Service;
using SqlJoiner.Repository.Column;
using SqlJoiner.Repository.Schema;
using SqlJoiner.Repository.Table;
using SqlJoiner.Services.ColumnService;
using SqlJoiner.Services.SchemaService;
using SqlJoiner.Services.TableService;

namespace SqlJoiner.UI.Winforms
{
    internal static class Program
    {
        private static IContainer? Container { get; set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var builder = new ContainerBuilder();
            builder.RegisterType<PostgresDataConnectionInitializer>().As<IDataConnectionInitializer>();
            builder.RegisterType<SchemaRepository>().As<ISchemaRepository>();
            builder.RegisterType<SchemaService>().As<ISchemaService>();
            builder.RegisterType<TableRepository>().As<ITableRepository>();
            builder.RegisterType<TableService>().As<ITableService>();
            builder.RegisterType<ColumnRepository>().As<IColumnRepository>();
            builder.RegisterType<ColumnService>().As<IColumnService>();

            builder.RegisterType<Form1>().AsSelf();

            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                var mainForm = scope.Resolve<Form1>();
                Application.Run(mainForm);
            }
        }
    }
}