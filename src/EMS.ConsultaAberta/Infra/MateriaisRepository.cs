using EMS.ConsultaAberta.Crosscutting;
using EMS.ConsultaAberta.QueryModel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EMS.ConsultaAberta.Infra;

public sealed class MateriaisRepository
{
    private readonly IMongoCollection<Material> _collection;
    
    public MateriaisRepository(IOptions<MongoOptions> mongoOptions)
    {
        var client = new MongoClient(mongoOptions.Value.Connection);
        var database = client.GetDatabase(mongoOptions.Value.Database);
        _collection = database.GetCollection<Material>("materiais");
    }

    public async Task<Material> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task Insert(Material material, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(material, cancellationToken: cancellationToken);
    }
}