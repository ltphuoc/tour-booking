using BusinessObject.Models;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountServices _accountServices;

        public AccountsController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        // GET: api/Accounts
        [HttpGet]
        public ActionResult<IEnumerable<Account>> GetAccounts([FromQuery] PagingRequest request)
        {
            var result = _accountServices.GetAll(request);
            return StatusCode((int)result.Status.Code, result);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var result = _accountServices.Get(id);
            return StatusCode((int)result.Status.Code, result);
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, AccountUpdateResquest account)
        {
            var result = _accountServices.Update(id, account).Result;
            return StatusCode((int)result.Status.Code, result);
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountResponse>> PostAccount(AccountCreateRequest account)
        {
            var result = await _accountServices.Create(account);
            return StatusCode((int)result.Status.Code, result);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var result = _accountServices.Delete(id).Result;
            return StatusCode((int)result.Status.Code, result);
        }
    }
}
