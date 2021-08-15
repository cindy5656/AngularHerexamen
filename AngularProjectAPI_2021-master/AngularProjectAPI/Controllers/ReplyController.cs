using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyController : ControllerBase
    {
        private readonly DataContext _context;

        public ReplyController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Reply
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reply>>> GetReplys()
        {
            return await _context.Replies.ToListAsync();
        }

        
        // GET: api/Reply/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reply>> GetReply(int id)
        {
            var Reply = await _context.Replies.FindAsync(id);

            if (Reply == null)
            {
                return NotFound();
            }

            return Reply;
        }

        [HttpGet("GetRepliesByPost/{postID}")]
        public async Task<ActionResult<IEnumerable<Reply>>> GetRepliesByPost(int postID)
        {
            var Replies = await _context.Replies.Where(x => x.PostID == postID).ToListAsync();

            if (Replies == null)
            {
                return NotFound();
            }

            return Replies;
        }

        // PUT: api/Reply/5
        // To protect from overReplying attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<Reply>> PutReply(int id, Reply Reply)
        {
            if (id != Reply.ReplyID)
            {
                return BadRequest();
            }

            _context.Entry(Reply).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReplyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Reply;
        }

        // Reply: api/Reply
        // To protect from overReplying attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Reply>> Reply(Reply Reply)
        {
            _context.Replies.Add(Reply);
            await _context.SaveChangesAsync();
            return Reply;
            //return CreatedAtAction("GetReply", new { id = Reply.ReplyID }, Reply);
        }

        // DELETE: api/Reply/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Reply>> DeleteReply(int id)
        {
            var Reply = await _context.Replies.FindAsync(id);
            if (Reply == null)
            {
                return NotFound();
            }

            _context.Replies.Remove(Reply);
            await _context.SaveChangesAsync();

            return Reply;
        }

        

        private bool ReplyExists(int id)
        {
            return _context.Replies.Any(e => e.ReplyID == id);
        }

        
    }
}
