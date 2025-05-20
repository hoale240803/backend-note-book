
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
[ApiController]

public class SecuredController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSecuredData()
    {
        // your async logic here
        return Ok("This secured data is available only for Authenticaed Users");
    }
}