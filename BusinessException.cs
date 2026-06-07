using System;

namespace MyStore
{
  
    public class BusinessException : Exception
    {
      
        public BusinessException(string message) : base(message) { }
    }
}