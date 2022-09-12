using EMS.ConsultaAberta.Infra;
using EMS.ConsultaAberta.QueryModel;
using Microsoft.AspNetCore.Mvc;

namespace EMS.ConsultaAberta.HttpServelessApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MateriaisController : ControllerBase
{
    private readonly MateriaisRepository _materiaisRepository;
    private readonly ILogger<MateriaisController> _logger;

    public MateriaisController(
        MateriaisRepository materiaisRepository,   
        ILogger<MateriaisController> logger)
    {
        _materiaisRepository = materiaisRepository;
        _logger = logger;
    }

    // GET materiais/2/
    [HttpGet("{id}")]
    public async Task<IActionResult>  GetById(Guid id, CancellationToken cancellationToken)
    {
        var material = await _materiaisRepository.GetById(id, cancellationToken);
        if (material == null!)
            return NotFound();
        return Ok(material);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var material = new Material(Guid.NewGuid(), "33333", "TESTE");
        await _materiaisRepository.Insert(material, cancellationToken);
        return CreatedAtAction(nameof(GetById), material.Id); 
    }
}