using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.Web.Controllers;

// Aşama A: sayfalar tasarımdaki işaretlemeyle birebir; veri bağlantısı sonraki adımda
public class HomeController : Controller
{
    [HttpGet("/")]
    public IActionResult Index() => View();
}

public class PaketlerController : Controller
{
    [HttpGet("/paketler")]
    public IActionResult Index() => View();

    [HttpGet("/paketler/{slug}")]
    public IActionResult Detay(string slug) => View();
}

public class BlogController : Controller
{
    [HttpGet("/blog")]
    public IActionResult Index() => View();

    [HttpGet("/blog/{slug}")]
    public IActionResult Detay(string slug) => View();
}

public class DegisimlerController : Controller
{
    [HttpGet("/degisimler")]
    public IActionResult Index() => View();
}

public class SayfalarController : Controller
{
    [HttpGet("/hakkimda")]
    public IActionResult Hakkimda() => View();

    [HttpGet("/iletisim")]
    public IActionResult Iletisim() => View();
}

public class DiyetController : Controller
{
    [HttpGet("/diyet-formu")]
    public IActionResult Form() => View();

    [HttpGet("/diyet-plani")]
    public IActionResult Plan() => View();
}

public class YasalController : Controller
{
    [HttpGet("/gizlilik-politikasi")]
    [HttpGet("/yasal/{tur}")]
    public IActionResult Index(string? tur) => View();
}

public class BultenController : Controller
{
    [HttpGet("/bulten-iptal")]
    public IActionResult Iptal() => View();
}

public class HataController : Controller
{
    [Route("/hata/{code:int}")]
    public IActionResult Index(int code)
    {
        Response.StatusCode = code;
        return code == StatusCodes.Status404NotFound ? View("NotFound") : View("ServerError");
    }
}
