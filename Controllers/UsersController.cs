using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OptionChain.Models;

namespace OptionChain.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly OptionDbContext _optionDbContext;

        public UsersController(ILogger<UsersController> logger, OptionDbContext optionDbContext)
        {
            _logger = logger;
            _optionDbContext = optionDbContext;
        }

        [HttpPost]
        public async Task<bool> Post(Users user)
        {
            try
            {
                var usersEntry = await _optionDbContext.Users.Where(x => x.Email.ToLower() == user.Email.ToLower()).FirstOrDefaultAsync();

                if(usersEntry == null)
                {
                    await _optionDbContext.Users.AddAsync(user);
                }
                else
                {
                    usersEntry.Name = user.Name;
                    usersEntry.GivenName = user.GivenName;
                    usersEntry.FamilyName = user.FamilyName;
                    usersEntry.ProfileImgeUrl = user.ProfileImgeUrl;
                    usersEntry.LastUpdated = DateTime.Now;
                }

                await _optionDbContext.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
 }
