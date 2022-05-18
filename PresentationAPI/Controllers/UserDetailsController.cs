using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;
using Microsoft.AspNetCore.Cors;
using LogicLayer;
using PresentationAPI.Models;

namespace PresentationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]

    public class UserDetailsController : ControllerBase
    {
        private readonly InsurewaveContext _context;
        IUser obj;
        public UserDetailsController(InsurewaveContext context , IUser obj)
        {
            _context = context;
            this.obj = obj; 
        }
        //Login Page calls this
        [HttpGet("{userId}+{password}")]
        public ActionResult<bool> Login(string userId, string password)
        {
            return obj.LoginUser(userId, password);
        }
        //UserModel page
        [HttpPost]
        public ActionResult<string> Register(UserModel register)
        {
            if (obj.GetAllUserIds().Contains(register.UserId))
                return "unavailable";
            else
            {
                UserDetail userDetail = new UserDetail()
                {
                    UserId = register.UserId,
                    Password = register.Password,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Gender = register.Gender,
                    Role = register.Role,
                    LicenseId = register.LicenseId
                };
                if (userDetail.Role.Equals("insurer"))
                {

                    InsurerDetail insert = new InsurerDetail
                    {
                        InsurerId = userDetail.UserId
                    };
                    obj.AddInsurerDetails(insert);
                }
                else if (userDetail.Role.Equals("broker"))
                {
                    BrokerDetail insert = new BrokerDetail
                    {
                        BrokerId = userDetail.UserId
                    };
                    obj.AddBrokerDetails(insert);
                }
                obj.AddUser(userDetail);

                //return CreatedAtAction("GetUserDetail", new { id = userDetail.UserId }, userDetail);
                return "successful";
            }
        }
        //change password redirect to another page
        [HttpPut("{UserId}")]
        public ActionResult<string> ForgotPassword(string UserId, string pwd)
        {
            UserDetail ud = obj.GetUserById(UserId);
            if (ud != null)
                obj.ChangePassword(UserId, pwd);
            else
                return "invalidUserId";
            return "passwordChanged";
        }
        //userdetails
        [HttpGet("{id}")]
        public ActionResult<UserDetail> Details(string id)
        {
            if (!UserDetailExists(id))
            {
                return null;
            }
            var userDetail = _context.UserDetails.FirstOrDefault(m => m.UserId == id);
            var u = UserDetailExists(id);
            userDetail.Password = null;
            return userDetail;
        }
        [HttpPut("{id}")]
        public ActionResult<string> Edit(string id, UserModel r)
        {
            if (id != r.UserId)
            {
                return "not found";
            }

            if (ModelState.IsValid)
            {
                UserDetail u = new UserDetail()
                {
                    UserId = r.UserId,
                    Password = r.Password,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Gender = r.Gender,
                    Role = r.Role,
                    LicenseId = r.LicenseId

                };
                try
                {
                    _context.Update(u);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDetailExists(r.UserId))
                    {
                        return "not found";
                    }
                    else
                    {
                        throw;
                    }
                }
                return "success";
            }
            return "not found";
        }


        /*[HttpPost]
        public async Task<ActionResult<UserDetail>> PostUserDetail(UserDetail userDetail)
        {
          if (_context.UserDetails == null)
          {
              return Problem("Entity set 'InsurewaveContext.UserDetails'  is null.");
          }
            _context.UserDetails.Add(userDetail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserDetailExists(userDetail.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserDetail", new { id = userDetail.UserId }, userDetail);
        }
        // GET: api/UserDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetail>>> GetUserDetails()
        {
          if (_context.UserDetails == null)
          {
              return NotFound();
          }
            return await _context.UserDetails.ToListAsync();
        }

        // GET: api/UserDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetail>> GetUserDetail(string id)
        {
          if (_context.UserDetails == null)
          {
              return NotFound();
          }
            var userDetail = await _context.UserDetails.FindAsync(id);

            if (userDetail == null)
            {
                return NotFound();
            }

            return userDetail;
        }

        // PUT: api/UserDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutUserDetail(string id, UserDetail userDetail)
        {
            if (id != userDetail.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDetailExists(id))
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

        

        // DELETE: api/UserDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDetail(string id)
        {
            if (_context.UserDetails == null)
            {
                return NotFound();
            }
            var userDetail = await _context.UserDetails.FindAsync(id);
            if (userDetail == null)
            {
                return NotFound();
            }

            _context.UserDetails.Remove(userDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool UserDetailExists(string id)
        {
            return (_context.UserDetails?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
