using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleUserAPIs.DTOs;
using SimpleUserAPIs.Helpers;
using SimpleUserAPIs.Models;

namespace SimpleUserAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly SimpleUserDbContext _context;

        public UserController(ILogger<UserController> logger, SimpleUserDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Get User API
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public SimpleUserDTO Get(string id)
        {
            if (id != null)
            {
                SimpleUser user = _context.SimpleUsers.FirstOrDefault(l => l.Id == id);
                return new SimpleUserDTO()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MarketingConsent = user.MarketingConsent
                };
            }
            return null;
        }

        /// <summary>
        /// Create User API
        /// </summary>
        /// <param name="simpleUser"></param>
        /// <returns></returns>
        [HttpPost]
        public UserResponseDTO Post([FromBody] SimpleUserDTO simpleUser)
        {
            string userId = "";
            UserResponseDTO userResponseDTO = new UserResponseDTO();

            //Check on parameters
            if (simpleUser.FirstName != null && simpleUser.LastName != null && simpleUser.Email != null)
            {
                if (simpleUser.FirstName != string.Empty && simpleUser.LastName != string.Empty && simpleUser.Email != string.Empty)
                {
                    //generate User Id
                    userId = Helper.GetSHA1Hash(simpleUser.Email);

                    //Save User in DB
                    _context.SimpleUsers.Add(new SimpleUser
                    {
                        Id = userId,
                        FirstName = simpleUser.FirstName,
                        LastName = simpleUser.LastName,
                        Email = simpleUser.Email,
                        MarketingConsent = simpleUser.MarketingConsent
                    });

                    _context.SaveChanges();

                    userResponseDTO.Id = userId;
                    userResponseDTO.Token = Helper.GenerateToken();
                }
            }
            return userResponseDTO;
        }


    }
}