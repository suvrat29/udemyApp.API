using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using udemyApp.API.Data;
using udemyApp.API.Dtos;
using udemyApp.API.Helpers;

namespace udemyApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;

        private readonly IMapper _mapper;

        private readonly DataContext _context;

        public UsersController(IDatingRepository repo, IMapper mapper, DataContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            try
            {
                if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                {
                    throw new Exception($"The user you are trying to update with id {id} is not authorized to perform this action.");
                }
            }
            catch (Exception error)
            {
                var function = "UpdateUser";
                var page = "UsersController";
                var user = $"{id}";
                Extensions.AddToApplicationLog(error.Message, error.Source, error.StackTrace, function, page, user, _context);

                return Unauthorized();
            }

            try
            {
                var userFromRepo = await _repo.GetUser(id);

                _mapper.Map(userForUpdateDto, userFromRepo);

                if (await _repo.SaveAll())
                {
                    return NoContent();
                }
                else
                {
                    throw new Exception($"Error when updating user {id}. Failed on save.");
                }
            }
            catch (Exception error)
            {
                var function = "UpdateUser";
                var page = "UsersController";
                var user = $"{id}";
                Extensions.AddToApplicationLog(error.Message, error.Source, error.StackTrace, function, page, user, _context);
                throw;
            }
        }

        //// POST: api/Users
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<User>> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return user;
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}