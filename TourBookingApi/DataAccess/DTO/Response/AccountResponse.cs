using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Response
{
    public class AccountResponse
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
