namespace AccountingService.Exceptions
{
    using System;

    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(String message)
                : base(message)
            {
                
            }
    }
}