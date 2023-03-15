using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TSS.Models
{
    public class Login
    {
        [Required(ErrorMessage="Please enter your user name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }

        public int ClientId { get; set; }
        public int employeeId { get; set; }
        public int SessionId { get; set; }
        public int ErrorId { get; set; }
        public int WinResizable { get; set; }
        public string EmployeeName { get; set; }
        public string ErrorDescription { get; set; }
        public string FirstUrl { get; set; }
        public string FirstName { get; set; }
        public string UnitName { get; set; }
    }
}