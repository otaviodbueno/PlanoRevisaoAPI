using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Context;

public partial class PlanoRevisaoApiContext : DbContext
{
    public PlanoRevisaoApiContext()
    {
    }

    public PlanoRevisaoApiContext(DbContextOptions<PlanoRevisaoApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<EmpresaRegiao> EmpresaRegiao { get; set; }

    public virtual DbSet<Linha> Linha { get; set; }

    public virtual DbSet<PlanoRevisao> PlanoRevisao { get; set; }

    public virtual DbSet<PlanoRevisaoPreco> PlanoRevisaoPreco { get; set; }

    public virtual DbSet<PlanoRevisaoTipo> PlanoRevisaoTipo { get; set; }

    public virtual DbSet<PoliticaVenda> PoliticaVenda { get; set; }

    public virtual DbSet<TipoRevisao> Tiporevisaos { get; set; }
}
