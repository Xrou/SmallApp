using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmallApp.Models;
using System.Text.Json.Nodes;

namespace SmallApp.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly Database context;

        public CommentController(Database context)
        {
            this.context = context;
        }

        [HttpGet()]
        public async Task<ActionResult<ICollection<SendComment>>> GetAll()
        {
            return context.Comments.Select(c => c.ToSend()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SendComment>> GetById(long id)
        {
            Comment? comment = context.Comments.FirstOrDefault(comment => comment.Id == id);

            if (comment == null)
                return NotFound("Comment with specified id not found");

            return comment.ToSend();
        }

        [HttpGet("AtUser/{id}")]
        public async Task<ActionResult<ICollection<SendComment>>> GetAtUser(long id)
        {
            return context.Comments.Where(c => c.UserId == id).Select(c => c.ToSend()).ToList();
        }

        [HttpPost()]
        public async Task<ActionResult> PostComment([FromBody] JsonObject json)
        {
            if (!(
                json.ContainsKey("userId") &&
                json.ContainsKey("text")
                ))
            {
                return BadRequest("\"text\" or \"userId\" field not fonud");
            }

            long userId = json["userId"]!.GetValue<long>();
            string text = json["text"]!.GetValue<string>();

            if (!context.Users.Any(u => u.Id == userId))
            {
                return BadRequest("User with specified id not found");
            }

            Comment newComment = new Comment() { UserId = userId, Text = text };
            context.Comments.Add(newComment);
            context.SaveChanges();

            return Ok(newComment.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditComment(long id, [FromBody] JsonObject json)
        {
            if (!json.ContainsKey("text"))
            {
                return BadRequest("\"text\" field not fonud");
            }

            Comment? editComment = context.Comments.FirstOrDefault(u => u.Id == id);

            if (editComment == null)
            {
                return NotFound("User with specified id not found");
            }

            editComment.Text = json["text"]!.GetValue<string>();

            context.SaveChanges();

            return Ok(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(long id)
        {
            Comment? deleteComment = context.Comments.FirstOrDefault(u => u.Id == id);

            if (deleteComment == null)
            {
                return NotFound("User with specified id not found");
            }

            context.Comments.Remove(deleteComment);
            context.SaveChanges();

            return Ok(id);
        }

    }
}
