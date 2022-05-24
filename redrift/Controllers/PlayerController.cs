using Microsoft.AspNetCore.Mvc;
using redrift.DB;
using redrift.DataClass;

namespace RedRift.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PlayerController : ControllerBase
	{
        [HttpGet]
        [Route("info")]
        public ActionResult Get(uint userId)
        {
            var db =new Context();
            var row = db.Users.Where(u => u.UserId == userId).FirstOrDefault();

            if (row is null)
            {
                return NotFound();
            }

            return new JsonResult(row);
        }
    }
}

