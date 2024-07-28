using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Acidmanic.Utilities.MintGum.Test.Functional.Controllers;



/// <summary>
/// Just to make sure works alongside MintGum endpoints
/// </summary>
[ApiController]
[Route("[controller]")]
public class SimpleController:ControllerBase
{


    [HttpGet]
    public IActionResult Beep()
    {
        return Ok(new {Message="Beep!"});
    }
}