namespace EMS.ConsultaAberta.Crosscutting;

public sealed class DatabaseOptions
{
    public const string Name = "DatabaseConfig";

    public string Connection { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
}