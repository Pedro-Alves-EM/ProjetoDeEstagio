using EM.Domain.Aluno;
using EM.Repository.RepoCidade;
using EM.Repository;
using Microsoft.AspNetCore.Mvc;

public class AlunoController : Controller
{
    private readonly RepositorioAluno _repositorioAluno;
    private readonly RepositorioCidade _repositorioCidade;
    private readonly ILogger<AlunoController> _logger;
    private string connectionString = "User=SYSDBA;Password=masterkey;Database=C:\\Users\\Escolar Manager\\Desktop\\projetosDeEstudo\\ProjetoEstagio\\BancoDeDados\\CD_CRUD.fdb;DataSource=CD_ALUNOS.FDB;Port=3054;Charset=NONE;Server=localhost;";

    public AlunoController(ILogger<AlunoController> dbContext, RepositorioAluno repositorioAluno, RepositorioCidade repositorioCidade)
    {
        _logger = dbContext;
        _repositorioAluno = repositorioAluno;
        _repositorioCidade = repositorioCidade;
    }

    public IActionResult Index()
    {
        var alunos = _repositorioAluno.GetAll();
        return View(alunos);
    }

    public IActionResult CadastroAluno(int? id)
    {
        ViewBag.OpcoesCidades = _repositorioCidade.GetAll(); 

        if (id != null)
        {
            var aluno = _repositorioAluno.Get(c => c.Matricula == id).FirstOrDefault();
            ViewData["Title"] = "Editar Aluno";
            return View(aluno);
        }
        ViewData["Title"] = "Cadastrar Aluno";
        ViewBag.OpcoesCidades = _repositorioCidade.GetAll();
        return View(new Aluno());
    }



    [HttpPost]
    public IActionResult CadastroAluno(Aluno aluno)
    {
        if (ModelState.IsValid)
        {
            if (aluno.Matricula > 0) 
            {
                _repositorioAluno.Update(aluno);
                return RedirectToAction("Index");
            }

            _repositorioAluno.Add(aluno);
            return RedirectToAction("Index");
        }

        ViewBag.OpcoesCidades = _repositorioCidade.GetAll();
        return View(aluno); 
    }



}
