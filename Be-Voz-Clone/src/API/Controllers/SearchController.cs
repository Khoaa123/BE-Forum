using Be_Voz_Clone.src.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Be_Voz_Clone.src.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }
        // GET: api/<SearchController>
        [HttpGet("search")]
        public async Task<IActionResult> Search(string? keyword, string? forum, string? tag, int pageNumber = 1, int pageSize = 5)
        {
            var result = await _searchService.SearchAsync(keyword, forum, tag, pageNumber, pageSize);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
