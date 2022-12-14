using System;
using Dapper.Contrib.Extensions;

namespace Websity.Models
{
    [Table("[AlunoCurso]")]
    public class AlunoCurso
    {
        public AlunoCurso(){}
        public AlunoCurso(Guid CoursoId, Guid AlunoId){}
        [Key]
        public Guid CoursoId { get; set; }
        public Guid AlunoId { get; set; }
        public int Progresso { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime UltimaDataAtualizacao { get; set; }
    }
}