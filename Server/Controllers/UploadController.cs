using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

public class UploadController : Controller
{
    // Properties
    private IWebHostEnvironment env;
    
    // Constructor
    public UploadController(IWebHostEnvironment env)
    {
        this.env = env;
    }
    
    // uploadfile
    [HttpPost("{filename}")]
    public async Task<IActionResult> PostUpLoad(string filename)
    {
        var upload = Path.Combine(env.ContentRootPath, "Images");
        var filepath = Path.Combine(upload, filename);
        if (Request.HasFormContentType)
        {
            var form = Request.Form;
            foreach (var file in form.Files)
            {
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
        }
        return Ok(new { Path = filepath });
    }
}