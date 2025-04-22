using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

public class BaseController : Controller
{
    protected readonly HttpClient client;
    protected readonly string baseUri;

    public BaseController(IConfiguration configuration, HttpClient client)
    {
        this.client = client;
        this.baseUri = configuration["ApiSettings:BaseUri"];
        client.BaseAddress = new Uri(baseUri);
    }
}