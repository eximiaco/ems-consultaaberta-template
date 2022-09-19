using System.Linq.Expressions;
using System.Text.RegularExpressions;
using EMS.ConsultaAberta.Crosscutting;
using EMS.ConsultaAberta.Infra;
using EMS.ConsultaAberta.SeedWork;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EMS.ConsultaAberta.QueryModel;

public sealed class MateriaisQueryService : IQueryService<MateriaisQueryService>
{
    private readonly ConsultaAbertaMongoContext _context;
    
    public MateriaisQueryService(ConsultaAbertaMongoContext context)
    {
        _context = context;
    }

    public async Task<Material> RecuperarPorId(Guid id, CancellationToken cancellationToken)
    {
        return await _context.MateriaisQuery
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }
    
    public async Task Incluir(Material material, CancellationToken cancellationToken = default)
    {
        await _context.Materiais.InsertOneAsync(material, cancellationToken: cancellationToken);
    }

    public async Task<RecuperarMateriaisPaginadosModel> RecuperarPorPagina(
        string codigoAnvisa,
        PaginationQueryModel filtro,
        CancellationToken cancellationToken)
    {
        try
        {
            var regex = new BsonRegularExpression(new Regex(codigoAnvisa, RegexOptions.IgnoreCase));
            
            var results = await _context.Materiais.AggregateByPage(
                Builders<Material>.Filter.Regex(c=> c.CodigoAnvisa, regex),
                Builders<Material>.Sort.Ascending(x => x.NomeTecnico),
                page: filtro.Page,
                pageSize: filtro.Limit);
            
            var returnItems = results.data.Select(x => new ListagemMateriaisModel()
            {
                Id = x.Id,
                Descricao = x.NomeTecnico
            });

            return new RecuperarMateriaisPaginadosModel()
            {
                TotalPaginas = results.totalPages,
                PaginaAtual = filtro.Page,
                TotalItens = results.totalItens,
                Itens = returnItems.ToList()
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }  
    }
}