using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NullHab.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NullHab.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public ValuesController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{name}")]
        public async Task<JsonResult> Get(string name)
        {
            return new JsonResult(await _userManager.FindByNameAsync(name));
        }

        // POST api/values
        [HttpPost]
        public async Task Post()
        {
            await _userManager.CreateAsync(new User
            {
                Email = "testValue4@mail.com",
                PasswordHash = "AALj124AF8Asm192AM",
                UserName = "TestValue2"
            });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
