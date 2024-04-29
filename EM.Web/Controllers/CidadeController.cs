using EM.Domain.Cidade;

using EM.Repository;
using EM.Repository.RepoCidade;
using Microsoft.AspNetCore.Mvc;
namespace EM.Web
{

    public class CidadeController : Controller
    {
        private readonly RepositorioCidade _repositorioCidade;
        private readonly ILogger<CidadeController> _logger;
        private string connectionString = "User=SYSDBA;Password=masterkey;Database=C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\BancoDeDados\\CD_CRUD.fdb;DataSource=CD_ALUNOS.FDB;Port=3054;Charset=NONE;Server=localhost;";


        public CidadeController(ILogger<CidadeController> dbContext, RepositorioCidade repositorioCidade)
        {
            _logger = dbContext;
            _repositorioCidade = repositorioCidade;
        }

        public IActionResult Index()
        {
            var cidade = _repositorioCidade.GetAll();
            return View(cidade);
        }


        public IActionResult EditarCidade(int? id)
        {
            if (id.HasValue)
            {
                var cidade = _repositorioCidade.Get(c => c.Cidade_Id == id).FirstOrDefault();
                ViewData["Title"] = "Editar Cidade";
                return View(cidade);
            }
            else
            {
                ViewData["Title"] = "Cadastrar Cidade";
                return View(new Cidade());
            }
        }

        [HttpPost]
        public IActionResult EditarCidade(Cidade cidade)
        {
            if (ModelState.IsValid)
            {
                if (cidade.Cidade_Id > 0)
                {
                    _repositorioCidade.Update(cidade);
                }
                else
                {
                    _repositorioCidade.Add(cidade);
                }
                return RedirectToAction("Index");
            }
            return View(cidade);
        }

    }


}

