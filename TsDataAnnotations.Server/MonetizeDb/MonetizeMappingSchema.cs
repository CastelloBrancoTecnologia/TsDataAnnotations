using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.Mapping;
using TsDataAnnotations.Models;

namespace TsDataAnnotations.Server.Database;

public class TsDataAnnotationsMappingSchema : MappingSchema
{
    public TsDataAnnotationsMappingSchema()
    {
        FluentMappingBuilder builder = new FluentMappingBuilder(this);

        builder.Entity<Usuario>().HasTableName("Usuarios")
               .Property(x => x.Id).HasColumnName(nameof(Usuario.Id)).IsIdentity().IsPrimaryKey().IsNotNull()
               .Property(x => x.Nome).HasColumnName(nameof(Usuario.Nome)).IsNotNull()
               .Property(x => x.Email).HasColumnName(nameof(Usuario.Email)).IsNotNull()
               .Property(x => x.HashSenha).HasColumnName(nameof(Usuario.HashSenha)).IsNotNull()
               .Property(x => x.HasErrors).IsNotColumn();

        builder.Build();
    }
}
