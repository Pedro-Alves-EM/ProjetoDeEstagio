using EM.Domain.Interface;
using System.ComponentModel.DataAnnotations;

namespace EM.Domain.Aluno
{
    public class Aluno : IEntidade
    {
        public int Matricula { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MinLength(3, ErrorMessage = "O campo Nome deve ter no mínimo 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "O campo Nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }


        [MaxLength(14)]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Insira um CPF no formato válido! 11-Digitos   ")]
        public string? CPF { get; set; }


        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
        public DateTime Nascimento { get; set; }


        [Required(ErrorMessage = "O campo Sexo é obrigatório.")]
        public EnumeradorSexo Sexo { get; set; }

        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        public Cidade.Cidade Cidade { get; set; }
    }
}
