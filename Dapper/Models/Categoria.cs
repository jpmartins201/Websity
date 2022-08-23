using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Websity.Models
{
    [Table("[Categoria]")]
    public class Categoria
    {
        public Categoria (Guid Id){}
        public Categoria(){}
        public Guid? Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }
}