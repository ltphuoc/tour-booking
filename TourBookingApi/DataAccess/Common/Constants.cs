namespace DataAccess.Common
{
    public class Constants
    {
        public const int DefaultPaging = 50;
        public const int LimitPaging = 500;


        public class Role
        {
            public const int INT_ROLE_ADMIN = 1;
            public const string STRING_ROLE_ADMIN = "admin";
            public const int INT_ROLE_USER = 2;
            public const string STRING_ROLE_USER = "user";
        }

        public class Status
        {
            public const int INT_ACTIVE_STATUS = 1;
            public const string STRING_ACTIVE_STATUS = "active";

            public const int INT_DELETED_STATUS = 2;
            public const string STRING_DELETED_STATUS = "deleted";
        }
    }

   
}
