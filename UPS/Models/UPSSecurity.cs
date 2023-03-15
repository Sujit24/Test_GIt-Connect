using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS.Models
{
    public class UPSSecurity
    {
        public UsernameToken UsernameToken { get; set; }
        public ServiceAccessToken ServiceAccessToken { get; set; }
    }

    public class UsernameToken
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ServiceAccessToken
    {
        public string AccessLicenseNumber { get; set; }
    }
}
