using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAppNeworkSecurity.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    [BindProperty]
    public string RandomString { get; set; }

    public IndexModel(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<IndexModel> logger)
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _configuration = configuration;
    }

    public async Task OnGetAsync()
    {
        var client = _clientFactory.CreateClient();
        RandomString = await client.GetStringAsync(_configuration["FunctionsApiUrl"]);
    }
}
