using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Websity.Models
{
    [Table("[Autor]")]
    public class Autor
    {
        public Autor(Guid id) {}
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
    }
}