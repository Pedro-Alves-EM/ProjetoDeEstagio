using EM.Domain.Aluno;
using EM.Repository.RepoCidade;
using EM.Repository;
using Microsoft.AspNetCore.Mvc;
using EM.Domain.Cidade;
using FirebirdSql.Data.FirebirdClient;

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

    //public IActionResult Index(int? matricula)
    //{
    //    if (matricula.HasValue)
    //    {
    //        var aluno = _repositorioAluno.GetByMatricula(matricula.Value);

    //        if (aluno == null)
    //        {
    //            TempData["ErrorMessage"] = "Aluno não encontrado.";
    //            return RedirectToAction("Index");
    //        }

    //        var alunoPorMatricula = new List<Aluno> { aluno }; 
    //        return View(alunoPorMatricula);
    //    }

    //    var alunos = _repositorioAluno.GetAll();
    //    return View(alunos);
    //}
    public IActionResult Index(int? matricula, string nome, string uf)
    {
        if (matricula.HasValue)
        {
            var aluno = _repositorioAluno.GetByMatricula(matricula.Value);
            if (aluno == null)
            {
                TempData["ErrorMessage"] = "Aluno não encontrado.";
                return RedirectToAction("Index");
            }
            var alunoPorMatricula = new List<Aluno> { aluno };
            return View("Index", alunoPorMatricula);
        }
        else if (!string.IsNullOrEmpty(nome))
        {
            var alunosPorNome = _repositorioAluno.GetByNome(nome);
            return View("Index", alunosPorNome);
        }
        else if (!string.IsNullOrEmpty(uf)) 
        {
            var alunosPorUf = _repositorioAluno.GetByUf(uf);
            return View("Index", alunosPorUf);
        }

        var todosAlunosDefault = _repositorioAluno.GetAll();
        return View(todosAlunosDefault);
    }



    public IActionResult AchePorMatricula(int id)
    {
        return RedirectToAction("Index", new { matricula = id });
    }

    public IActionResult AchePorNome(string id)
    {
        return RedirectToAction("Index", new { nome = id });
    }
    public IActionResult AchePorUf(string id)
    {
        return RedirectToAction("Index", new { uf = id });
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
   


    [HttpPost]
    public IActionResult RemoverAluno(int matricula)
    {
        var aluno = _repositorioAluno.Get(c => c.Matricula == matricula).FirstOrDefault();

        if (aluno == null)
        {
            return NotFound();
        }
        _repositorioAluno.Remove(aluno);

        return RedirectToAction("Index");
    }



    

}
