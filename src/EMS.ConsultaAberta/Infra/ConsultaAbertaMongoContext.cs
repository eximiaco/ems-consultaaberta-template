using EMS.ConsultaAberta.Crosscutting;
using EMS.ConsultaAberta.QueryModel;
using EMS.ConsultaAberta.SeedWork;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EMS.ConsultaAberta.Infra;

public class ConsultaAbertaMongoContext : IService<ConsultaAbertaMongoContext>
{
    private const string _materiaisCollection = "materiais";
    private readonly IMongoDatabase _database = null;
    
    public ConsultaAbertaMongoContext(IOptions<DatabaseOptions> mongoOptions)
    {
        var client = new MongoClient(mongoOptions.Value.Connection);
        _database = client.GetDatabase(mongoOptions.Value.Database);
    }
    
    public IMongoQueryable<Material> MateriaisQuery => _database.GetCollection<Material>(_materiaisCollection).AsQueryable<Material>();
    public IMongoCollection<Material> Materiais => _database.GetCollection<Material>(_materiaisCollection);
}