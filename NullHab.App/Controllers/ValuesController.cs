using Microsoft.AspNetCore.Mvc;
using NullHab.DAL.Models;
using NullHab.DAL.Providers.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NullHab.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserTable _userTable;

        public ValuesController(UserTable userTable)
        {
            _userTable = userTable;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task Post()
        {
            await _userTable.CreateAsync(new User
            {
                Email = "testValue@mail.com",
                PasswordHash = "AALj124AF8Asm192AM",
                UserName = "TestValue1"
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
