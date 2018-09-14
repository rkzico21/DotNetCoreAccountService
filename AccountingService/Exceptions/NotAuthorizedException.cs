namespace AccountingService.Exceptions
{
    using System;

    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(String message)
                : base(message)
            {
                
            }
    }
}