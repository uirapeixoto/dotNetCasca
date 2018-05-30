using sso.Models.Data;
using sso.ViewModel;
using System.Threading.Tasks;
using System.Web.Http;
using System;

namespace sso.Controllers
{
    [Route("api/robo")]
    public class RoboApiController : ApiController
    {
        private readonly RoboContext _context;

        public RoboApiController(RoboContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostRoboApi([FromBody] RoboExecucaoViewModel roboExecucao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            await _context.SaveChangesAsync();

            return null;
        }

    }
}
