using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanoRevisaoAPI.Models;

[Table("EMPRESAREGIAO")]
public partial class EmpresaRegiao
{
    [Key]
    public int IdEmpresaRegiao { get; set; }

    public string? NmRegiao { get; set; }

    public string? CdGrupoRegiao { get; set; }

    public virtual ICollection<Empresa> Empresas { get; set; }

    public virtual ICollection<PlanoRevisaoPreco> Planorevisaoprecos { get; set; }
}
