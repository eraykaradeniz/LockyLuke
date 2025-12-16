using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace LockyLuke.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {

        public IActionResult ActionResultInstance<T>(ResponseDto<T> response) where T : class
        {
            if (response == null)
            {
                return NotFound();
            }
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }   
    }
}
