using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Websity.Models
{
    [Table("[Curso]")]
    public class Curso
    {
        public Curso(Guid id) {}
        public Curso(Guid id, Guid AutorId, Guid CategoriaId) {}
        public Curso() => Categorias = new List<Categoria>();

        [Key]
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int DuracaoEmMinutos { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
        public Guid? AutorId { get; set; }
        public Guid? CategoriaId { get; set; }

        public List<Categoria> Categorias { get; set; }
    }
}