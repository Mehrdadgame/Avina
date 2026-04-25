using Avina.Services;
using Microsoft.AspNetCore.Mvc;

namespace Avina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController(ISearchService searchService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GlobalSearchResultDto>> Search([FromQuery] string q, CancellationToken cancellationToken)
    {
        var result = await searchService.SearchAsync(q, cancellationToken);
        return Ok(result);
    }
}
