using EMS.ConsultaAberta.QueryModel;
using EMS.ConsultaAberta.SeedWork;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace EMS.ConsultaAberta.Infra;

public class EntitiesConfiguration
{
    public static void ApplyMongoEntitiesConfiguration()
    {
        BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

        BsonClassMap.RegisterClassMap<Entity<Guid>>(x =>
        {
            x.MapIdProperty(x => x.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
        });

        BsonClassMap.RegisterClassMap<Material>(x =>
        {
            x.AutoMap();
        });
    }
}