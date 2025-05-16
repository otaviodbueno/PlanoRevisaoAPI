using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanoRevisaoAPI.Models;

[Table("PLANOREVISAOPRECO")]
public partial class PlanoRevisaoPreco
{
    public int IdPlanoRevisaoPreco { get; set; }

    public int IdPlanoRevisaoTipo { get; set; }

    public int IdEmpresaRegiao { get; set; }

    public DateTime DtVigenciaInicial { get; set; }

    public DateTime DtVigenciaFinal { get; set; }

    [ForeignKey("IdEmpresaRegiao")]
    public virtual EmpresaRegiao EmpresaRegiao { get; set; }

    [ForeignKey("IdPlanoRevisaoTipo")]
    public virtual PlanoRevisaoTipo PlanoRevisaoTipo { get; set; }
}
