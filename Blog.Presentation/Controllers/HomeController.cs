using Microsoft.AspNetCore.Mvc;

namespace Blog.Presentation.Controllers;

[Route("/")]
public class HomeController: Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Hello, World!");
    }
}