using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanoRevisaoAPI.Models;

[Table("PLANOREVISAOPRECO")]
public partial class PlanoRevisaoPreco
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_PLANO_REVISAO_PRECO { get; set; }

    public int ID_PLANO_REVISAO_TIPO { get; set; }

    public int ID_EMPRESA_REGIAO { get; set; }

    public DateTime DT_VIGENCIA_INICIAL { get; set; }

    public DateTime DT_VIGENCIA_FINAL { get; set; }

    [ForeignKey("ID_EMPRESA_REGIAO")]
    public virtual EmpresaRegiao EmpresaRegiao { get; set; }

    [ForeignKey("ID_PLANO_REVISAO_TIPO")]
    public virtual PlanoRevisaoTipo PlanoRevisaoTipo { get; set; }
}
