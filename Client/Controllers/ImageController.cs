using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

public class ImageController : Controller
{
    IHttpClientFactory factory;

    private const string uri = "http://localhost:7284/Image";
    
    //contructor
    public ImageController(IHttpClientFactory factory)
    {
        this.factory = factory;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }

    //cach 1 laf luu file len server
    //cach 2 la luu file lên cloud
    //cach 3  QRCode
    
    // cach 1
    [HttpPost]
    public async Task<IActionResult> Index(IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return Content("File is not Selected!");
        }

        HttpClient client = new HttpClient();
        byte[] data;
        using (var br = new BinaryReader(image.OpenReadStream()))
        {
            data = br.ReadBytes((int)image.OpenReadStream().Length);
        }

        ByteArrayContent bytes = new ByteArrayContent(data);
        MultipartFormDataContent multipart = new MultipartFormDataContent()
        {
            { bytes, "file", image.FileName }
        };
        var result = await client.PostAsync(uri + image.FileName, multipart);
        return RedirectToAction("Index");
    }
}