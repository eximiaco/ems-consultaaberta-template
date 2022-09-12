namespace EMS.ConsultaAberta.Crosscutting;

public sealed class MongoOptions
{
    public const string Name = "Mongo";

    public string Connection { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
}