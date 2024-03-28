using Blog_Server.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        //public BlogController(ApplicationDbContext context) {
        //    _context = context;
        //}

        [HttpGet]
        public IActionResult GetAllPosts()
        {
            return Ok("Test");
        }
    }
}
