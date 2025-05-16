using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanoRevisaoAPI.Models;

[Table("LINHA")] 
public class Linha
{
    [Key]
    public int IdLinha { get; set; }

    public string? CdLinha { get; set; }

    public string? NmLinha { get; set; }

    public bool InAtivo { get; set; }

    public virtual ICollection<PlanoRevisao> PlanosRevisao { get; set; } = new List<PlanoRevisao>();
}
