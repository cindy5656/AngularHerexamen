using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DataContext _context;

        public PostsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }

        [HttpGet("GetPostsByUser/{userID}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUser(int userID)
        {
            var posts =  await _context.PostGroupUsers.Where(x => x.UserID == userID).Select(x => x.post).ToListAsync();
            if (posts == null)
            {
                return NotFound();
            }
            return posts;
        }
        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var Post = await _context.Posts.FindAsync(id);

            if (Post == null)
            {
                return NotFound();
            }

            return Post;
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post Post)
        {
            if (id != Post.PostID)
            {
                return BadRequest();
            }

            _context.Entry(Post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Post
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post Post)
        {
            _context.Posts.Add(Post);
            await _context.SaveChangesAsync();
            return Post;
            //return CreatedAtAction("GetPost", new { id = Post.PostID }, Post);
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            var Post = await _context.Posts.FindAsync(id);
            if (Post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(Post);
            await _context.SaveChangesAsync();

            return Post;
        }

        [HttpDelete("DeletePostFromUserAndGroup/{postID}")]
        public async Task<ActionResult<Post>> DeletePostFromUserAndGroup(int postID)
        {
            var postGroupUser = await _context.PostGroupUsers.Where(x => x.PostID == postID).FirstOrDefaultAsync();
            if (postGroupUser == null)
            {
                return NotFound();
            }

            _context.PostGroupUsers.Remove(postGroupUser);
            await _context.SaveChangesAsync();

            return postGroupUser.post;
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostID == id);
        }

        [HttpPost("AddPostToUserAndGroup")]
        public async Task<ActionResult<Post>> AddPostToUserAndGroup(int postID, int userID, int groupID)
        {
            PostGroupUser postGroupUser = new PostGroupUser();
            postGroupUser.UserID = userID;
            postGroupUser.PostID = postID;
            postGroupUser.GroupID = groupID;
            _context.PostGroupUsers.Add(postGroupUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PostExists(postGroupUser.PostID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPost", new { id = postGroupUser.PostID }, postGroupUser.post);
        }

    }
}
