using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    public class Enum
    {

        public enum PaymentStatusEnum
        {
            Waiting = 1,
            Finished = 2,
            Expired = 3
        }
        public enum PaymentTypeEnum
        {
            Cod = 2, Momo = 1,
        }
    }
}
