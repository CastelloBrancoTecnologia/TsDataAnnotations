using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.Mapping;
using TsDataAnnotations.Models;
using TsDataAnnotations.Server.Database;

namespace TsDataAnnotations.Server;

public class TsDataAnnotationsDb : DataConnection
{
    public TsDataAnnotationsDb(DataOptions<TsDataAnnotationsDb> options)
        : base(options.Options)
    {
        AddMappingSchema(new TsDataAnnotationsMappingSchema());
    }

    public ITable<Usuario> Usuarios => this.GetTable<Usuario>();
    public ITable<UsuariosRole> UsuariosRoles => this.GetTable<UsuariosRole>();
}
