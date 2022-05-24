using Microsoft.AspNetCore.Mvc;
using redrift.DataClass;
using redrift.DB;

namespace RedRift.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
	{
		
        [HttpPost]
        [Route("sign_in")]
        public ActionResult Authorize(string login, string password)
            {

            var db = new Context();

            var row = db.Users.Where(u => (u.Login == login && u.Password == password)).FirstOrDefault();
            if (row is null)
            {
                return NotFound();
            }
            else
            {
                row.Token = Guid.NewGuid().ToString();
                db.Users.Update(row);
                db.SaveChanges();
                return new JsonResult(new AuthResult(row.UserId, row.Token));
            }

        }

        [HttpPost]
        [Route("sign_up")]
        public ActionResult Registration(string login, string password)
        {
            var db = new Context();
            db.Users.Add(new User(login, password));
            db.SaveChanges();
            return new OkResult();
        }
        
	}
}

