using EM.Domain.Cidade;
using EM.Repository;
using EM.Repository.RepoCidade;

namespace EM.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<RepositorioCidade>(provider => new RepositorioCidade("User=SYSDBA;Password=masterkey;Database=C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoDois\\BD\\CD_CRUD.fdb;DataSource=CD_ALUNOS.FDB;Port=3054;Charset=NONE;Server=localhost;"));
            builder.Services.AddTransient<RepositorioAluno>(provider => new RepositorioAluno("User=SYSDBA;Password=masterkey;Database=C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoDois\\BD\\CD_CRUD.fdb;DataSource=CD_ALUNOS.FDB;Port=3054;Charset=NONE;Server=localhost;"));
            builder.Services.AddTransient<Cidade>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "Cidade",
                pattern: "Cidade/{action=Index}/{id}",
                defaults: new { controller = "Cidade", action = "Index" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Aluno}/{action=Index}/{id?}");

           
            app.Run();
        }
    }
}
