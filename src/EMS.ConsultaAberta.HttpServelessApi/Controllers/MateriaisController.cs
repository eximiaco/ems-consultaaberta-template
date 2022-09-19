using EMS.ConsultaAberta.HttpServelessApi.InputModels;
using EMS.ConsultaAberta.HttpServelessApi.ViewModels;
using EMS.ConsultaAberta.Infra;
using EMS.ConsultaAberta.QueryModel;
using EMS.ConsultaAberta.SeedWork;
using EMS.Infrastructure.Web.InputModels;
using EMS.Infrastructure.Web.Services;
using EMS.Infrastructure.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EMS.ConsultaAberta.HttpServelessApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MateriaisController : ControllerBase
{
    private readonly MateriaisQueryService _materiaisQueryService;
    private readonly ILogger<MateriaisController> _logger;

    public MateriaisController(
        MateriaisQueryService materiaisQueryService,   
        ILogger<MateriaisController> logger)
    {
        _materiaisQueryService = materiaisQueryService;
        _logger = logger;
    }

    //[ProducesResponseType(typeof(RESPOSTA), Status200OK)]
    //[ProducesResponseType(typeof(RESPOSTA), Status400BadRequest)]
    [HttpGet("{id}", Name = nameof(GetById))]
    public async Task<IActionResult>  GetById(Guid id, CancellationToken cancellationToken)
    {
        var material = await _materiaisQueryService.RecuperarPorId(id, cancellationToken);
        if (material == null!)
            return NotFound();
        return Ok(material);
    }

    //[ProducesResponseType(typeof(RESPOSTA), Status200OK)]
    //[ProducesResponseType(typeof(RESPOSTA), Status400BadRequest)]
    [HttpGet(Name = nameof(GetAll))]
    public async Task<IActionResult> GetAll(
        [FromQuery]RecuperarMateriaisInputModel queryParam, 
        CancellationToken cancellationToken)
    {
        var materiaisModel = await _materiaisQueryService.RecuperarPorPagina(
            queryParam.CodigoAnvisa,
            new PaginationQueryModel(queryParam.Limit, queryParam.Page), 
            cancellationToken);
        
        var materiaisViewModel = new LinkedPageViewModel<ListagemMateriaisViewModel>()
        {
            CurrentPage = materiaisModel.PaginaAtual,
            TotalItems = materiaisModel.TotalItens,
            TotalPages = materiaisModel.TotalPaginas,
            Items = materiaisModel.Itens.Select(c => new ListagemMateriaisViewModel()
                {
                    Id = c.Id,
                    Descricao = c.Descricao
                })
                .ToList()
        };
        
        return Ok(Url.GeneratePageLinks(materiaisViewModel, queryParam, nameof(GetAll)));
    }
    
    
    [HttpPost(Name = nameof(Create))]
    public async Task<IActionResult> Create([FromBody]NovoMaterialInputModel inputModel, CancellationToken cancellationToken)
    {
        var material = new Material(Guid.NewGuid(), inputModel.CodigoAnvisa, inputModel.NomeTecnico);
        await _materiaisQueryService.Incluir(material, cancellationToken);
        return Ok(material.Id); 
    }
}