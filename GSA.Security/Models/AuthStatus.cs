using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSA.Security.Models
{
    public enum AuthStatusCode
    {
        SUCCESS = 1,
        FAILED = 0
    }

    public enum AuthStatusFailReason
    {
        INVALID_PASSWORD = 1,
        INVALID_USER = 2,
        ACCOUNT_LOCKED = 3
    }
    
    public class AuthStatus
    {
        public AuthStatusCode Code { get; set; }
        public AuthStatusFailReason FailedLoginReason { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
