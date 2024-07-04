using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmallApp.Models;
using System.Text.Json.Nodes;

namespace SmallApp.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly Database context;

        public UsersController(Database context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SendUser>> GetById(long id)
        {
            User? user = context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound("User with specified id not found");

            return user.ToSend();
        }

        [HttpGet()]
        public async Task<ActionResult<ICollection<SendUser>>> GetAll()
        {
            return context.Users.Select(u => u.ToSend()).ToList();
        }

        [HttpPost()]
        public async Task<ActionResult> PostUser([FromBody] JsonObject json)
        {
            if (!json.ContainsKey("name"))
            {
                return BadRequest("\"name\" field not fonud");
            }

            User newUser = new User() { Name = json["name"]!.GetValue<string>() };
            context.Users.Add(newUser);
            context.SaveChanges();

            return Ok(newUser.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditUser(long id, [FromBody] JsonObject json)
        {
            if (!json.ContainsKey("name"))
            {
                return BadRequest("\"name\" field not fonud");
            }

            User? editUser = context.Users.FirstOrDefault(u => u.Id == id);

            if (editUser == null)
            {
                return NotFound("User with specified id not found");
            }

            editUser.Name = json["name"]!.GetValue<string>();

            context.SaveChanges();

            return Ok(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(long id)
        {
            User? deleteUser = context.Users.FirstOrDefault(u => u.Id == id);

            if (deleteUser == null)
            {
                return NotFound("User with specified id not found");
            }

            context.Users.Remove(deleteUser);
            context.SaveChanges();

            return Ok(id);
        }

    }
}
