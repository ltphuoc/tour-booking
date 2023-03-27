using BusinessObject.Models;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Authorize(Policy = "AdminOnly")]
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
            if (ModelState.IsValid)
            {
                var result = _accountServices.GetAll(request);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            var result = _accountServices.Get(id);
            return StatusCode((int)result.Status.Code, result);
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutAccount(int id, AccountUpdateResquest account)
        {
            if (ModelState.IsValid)
            {
                var result = _accountServices.Update(id, account).Result;
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountResponse>> PostAccount(AccountCreateRequest account)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountServices.Create(account);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            var result = _accountServices.Delete(id).Result;
            return StatusCode((int)result.Status.Code, result);
        }
    }
}
