namespace DataAccess.Helpers
{
    public class Enum
    {

        public enum PaymentStatusEnum
        {
            //Waiting = 1,
            //Finished = 2,
            //Expired = 3
            Pending = 0,
            PartiallyPaid = 1,
            FullyPaid = 2,
            Expired = 3,
            Refunded = 4,
            Failed = 5
        }
        public enum PaymentTypeEnum
        {
            Cod = 1, Bank = 2,
        }
    }
}
