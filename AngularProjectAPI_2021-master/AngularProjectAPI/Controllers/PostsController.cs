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

        [HttpGet("GetPostsByGroup/{groupID}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByGroup(int groupID)
        {
            var posts = await _context.PostGroupUsers.Where(x => x.GroupID == groupID).Select(x => x.post).ToListAsync();
            if (posts == null)
            {
                return NotFound();
            }
            return posts;
        }

        [HttpGet("CheckIfPostLikedByUser/{userID}")]
        public async Task<ActionResult<IEnumerable<Post>>> CheckIfPostLikedByUser(int userID)
        {
            var posts = await _context.PostsLikedBy.Where(x => x.UserID == userID).Select(x => x.post).ToListAsync();
            if (posts == null)
            {
                return NotFound();
            }
            return posts;
        }

        [HttpGet("CheckIfPostDislikedByUser/{userID}")]
        public async Task<ActionResult<IEnumerable<Post>>> CheckIfPostDislikedByUser(int userID)
        {
            var posts = await _context.PostsDislikedBy.Where(x => x.UserID == userID).Select(x => x.post).ToListAsync();
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
        public async Task<ActionResult<Post>> PutPost(int id, Post Post)
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

            return Post;
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

        private bool PostLikedByExists(int id)
        {
            return _context.PostsLikedBy.Any(e => e.PostLikedByID == id);
        }
        private bool PostDislikedByExists(int id)
        {
            return _context.PostsDislikedBy.Any(e => e.PostDislikedByID == id);
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

        [HttpPost("PostLikedBy")]
        public async Task<ActionResult<Post>> PostLikedBy(int postID, int userID)
        {
            PostLikedBy postLikedBy = new PostLikedBy();
            postLikedBy.UserID = userID;
            postLikedBy.PostID = postID;
            _context.PostsLikedBy.Add(postLikedBy);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PostLikedByExists(postLikedBy.PostLikedByID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPost", new { id = postLikedBy.PostID }, postLikedBy.post);
        }

        [HttpPost("PostDislikedBy")]
        public async Task<ActionResult<Post>> PostDislikedBy(int postID, int userID)
        {
            PostDislikedBy postDislikedBy = new PostDislikedBy();
            postDislikedBy.UserID = userID;
            postDislikedBy.PostID = postID;
            _context.PostsDislikedBy.Add(postDislikedBy);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PostDislikedByExists(postDislikedBy.PostDislikedByID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPost", new { id = postDislikedBy.PostID }, postDislikedBy.post);
        }
    }
}
