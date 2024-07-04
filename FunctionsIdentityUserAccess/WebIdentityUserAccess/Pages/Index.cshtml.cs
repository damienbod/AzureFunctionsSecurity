using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebIdentityUserAccess.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;
    private readonly ITokenAcquisition _tokenAcquisition;


    [BindProperty]
    public string RandomString { get; set; }

    public IndexModel(IHttpClientFactory clientFactory, ITokenAcquisition tokenAcquisition,
        IConfiguration configuration, ILogger<IndexModel> logger)
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _configuration = configuration;
        _tokenAcquisition = tokenAcquisition;
    }

    public async Task OnGetAsync()
    {
        var client = _clientFactory.CreateClient();

        var scope = _configuration["CallApi:ScopeForAccessToken"];
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { scope });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        RandomString = await client.GetStringAsync(_configuration["CallApi:FunctionsApiUrl"]);
    }
}
