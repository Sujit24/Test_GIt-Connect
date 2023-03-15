using System;
using System.Text;
using System.Web;
using System.Web.Security;
using GSA.Security.Models;

namespace GSA.Security
{
    public class GSAFormsAuthenticationService
    {
        private IMembershipService _MembershipService;

        public GSAFormsAuthenticationService(IMembershipService membershipService)
        {
            _MembershipService = membershipService;
        }
        
        /// <summary>
        /// Validates and signs a user into the system
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="createPersistentCookie">Whether or not to create a persistent cookie</param>
        /// <returns>AuthStatus</returns>
        public AuthStatus SignIn(string userName, string password, bool createPersistentCookie)
        {
            AuthStatus authStatus = _MembershipService.ValidateUser(userName, password);
            if (authStatus.Code == AuthStatusCode.SUCCESS)
            {
                StringBuilder commaSeparatedRolesBuilder = new StringBuilder();
                string[] roles = _MembershipService.GetRoles(userName);
                foreach (string role in roles)
                {
                    commaSeparatedRolesBuilder.Append(role);
                    commaSeparatedRolesBuilder.Append(",");
                }
                if (commaSeparatedRolesBuilder.Length > 0) commaSeparatedRolesBuilder.Remove(commaSeparatedRolesBuilder.Length - 1, 1);
                SetAuthCookie(userName, commaSeparatedRolesBuilder.ToString(), createPersistentCookie);
            }

            return authStatus;
        }
        
        /// <summary>
        /// Signs a user out of the system
        /// </summary>
        public void SignOut()
        {
            HttpContext.Current.Session.Clear();
            FormsAuthentication.SignOut();            
        }

        /// <summary>
        /// Creates and returns the Forms authentication ticket 
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="commaSeparatedRoles">Comma separated roles for the users</param>
        /// <param name="createPersistentCookie">True or false whether to create persistant cookie</param>
        /// <param name="strCookiePath">Path for which the authentication ticket is valid</param>
        private static FormsAuthenticationTicket CreateAuthenticationTicket(string userName, string commaSeparatedRoles, bool createPersistentCookie, string strCookiePath)
        {
            string cookiePath = strCookiePath == null ? FormsAuthentication.FormsCookiePath : strCookiePath;

            //Determine the cookie timeout value from web.config if specified
            TimeSpan expiration = FormsAuthentication.Timeout;

            //Create the authentication ticket
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            1,                      //A dummy ticket version

            userName,               //User name for whome the ticket is issued

            DateTime.Now,           //Current date and time

            //DateTime.Now.AddMinutes(expirationMinutes), //Expiration date and time
            DateTime.Now.Add(FormsAuthentication.Timeout),  //Expiration date and time
            //DateTime.Now.AddDays(30),  //Expiration date and time

            createPersistentCookie, //Whether to persist cookie on client side. If true, 
                //The authentication ticket will be issued for new sessions from
                //the same client PC    

            commaSeparatedRoles,    //Comma separated user roles

            cookiePath);            //Path cookie valid for

            return ticket;
        }

        /// <summary>
        /// Creates a Forms authentication ticket and writes it in Url or embeds it within Cookie. Uses the             
        /// SetAuthCookieMain() private method 
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="commaSeparatedRoles">Comma separated roles for the users</param>
        /// <param name="createPersistentCookie">True or false whether to create persistant cookie</param>
        public static void SetAuthCookie(string userName, string commaSeparatedRoles, bool createPersistentCookie)
        {
            SetAuthCookieMain(userName, commaSeparatedRoles, createPersistentCookie, null);
        }

        /// <summary>
        /// Creates a Forms authentication ticket and writes it in Url or embeds it within Cookie. Uses the             
        /// SetAuthCookieMain() private method 
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="commaSeparatedRoles">Comma separated roles for the users</param>
        /// <param name="createPersistentCookie">True or false whether to create persistant cookie</param>
        /// <param name="strCookiePath">Path for which the authentication ticket is valid</param>
        public static void SetAuthCookie(string userName, string commaSeparatedRoles, bool createPersistentCookie, string strCookiePath)
        {
            SetAuthCookieMain(userName, commaSeparatedRoles, createPersistentCookie, strCookiePath);
        }

        /// <summary>
        /// Creates Forms authentication ticket using the private method CreateAuthenticationTicket() and writes 
        /// it in Url or embeds it within Cookie 
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="commaSeparatedRoles">Comma separated roles for the users</param>
        /// <param name="createPersistentCookie">True or false whether to create persistant cookie</param>
        /// <param name="strCookiePath">Path for which the authentication ticket is valid</param>
        private static void SetAuthCookieMain(string userName, string commaSeparatedRoles, bool createPersistentCookie, string strCookiePath)
        {
            FormsAuthenticationTicket ticket = CreateAuthenticationTicket(userName, commaSeparatedRoles, createPersistentCookie, strCookiePath);
            //Encrypt the authentication ticket
            string encrypetedTicket = FormsAuthentication.Encrypt(ticket);

            if (!FormsAuthentication.CookiesSupported)
            {
                //If the authentication ticket is specified not to use cookie, set it in the Uri
                FormsAuthentication.SetAuthCookie(encrypetedTicket, createPersistentCookie);
            }
            else
            {
                //If the authentication ticket is specified to use a cookie, wrap it within a cookie.
                //The default cookie name is .ASPXAUTH if not specified 
                //in the <forms> element in web.config
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypetedTicket);

                //Set the cookie's expiration time to the tickets expiration time
                if (ticket.IsPersistent) authCookie.Expires = ticket.Expiration;
                ////Set the cookie in the Response
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }
        
        /// <summary>
        /// Adds roles to the current User in HttpContext after forms authentication authenticates the user
        /// so that, the authorization mechanism can authorize user based on the groups/roles of the user
        /// </summary>
        public static void AttachRolesToUser()
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;

                        FormsAuthenticationTicket ticket = (id.Ticket);

                        if (!FormsAuthentication.CookiesSupported)
                        {
                            //If cookie is not supported for forms authentication, then the 
                            //authentication ticket is stored in the Url, which is encrypted.
                            //So, decrypt it
                            ticket = FormsAuthentication.Decrypt(id.Ticket.Name);
                        }

                        // Get the stored user-data, in this case, user roles
                        if (!string.IsNullOrEmpty(ticket.UserData))
                        {
                            string userData = ticket.UserData;

                            string[] roles = userData.Split(',');
                            //Roles were put in the UserData property in the authentication ticket
                            //while creating it

                            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, roles);
                        }
                    }
                }
            }
        }
    }
}
