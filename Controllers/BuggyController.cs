using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebShopApi.Data;

namespace WebShopApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : Controller
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<string> GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            if (thing == null) return NotFound();

            return Json(thing);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }


        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}
