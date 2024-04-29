using EM.Domain.Aluno;
using EM.Repository;
using EM.Repository.RepoCidade;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers
{
    public class AlunoController : Controller
    {

        private readonly RepositorioAluno _repositorioAluno;
        private readonly ILogger<AlunoController> _logger;
        private string connectionString = "User=SYSDBA;Password=masterkey;Database=C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\BancoDeDados\\CD_CRUD.fdb;DataSource=CD_ALUNOS.FDB;Port=3054;Charset=NONE;Server=localhost;";


        public AlunoController(ILogger<AlunoController> dbContext, RepositorioAluno repositorioAluno)
        {
            _logger = dbContext;
            _repositorioAluno = repositorioAluno;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditarAluno()
        {
            return View();
        }

         public IActionResult CadastroAluno(Aluno aluno)
        {
            if(ModelState.IsValid)
            {
                _repositorioAluno.Add(aluno);
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
