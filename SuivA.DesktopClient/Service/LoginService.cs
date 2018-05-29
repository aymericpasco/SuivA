using System;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using SuivA.Data.Utility.Session;
using static System.String;
using SearchScope = System.DirectoryServices.SearchScope;

namespace SuivA.DesktopClient.Service
{
    [DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
    [DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
    public class LoginService
    {
        private const string Server = "EPSI-S8-VM-AD-1";
        private const int Port = 389;

        public static bool Authenticated(string username, string password, string domain = "gsb")
        {
            bool isSuccess;
            try
            {
                var ldi = new LdapDirectoryIdentifier(Server, Port);

                var lc = new LdapConnection(ldi);
                var nc = new NetworkCredential(username, password, domain + ".local");

                lc.Bind(nc);

                isSuccess = true;

                lc.Dispose();
            }
            catch (LdapException)
            {
                isSuccess = false;
                // throw new LdapException(e.ErrorCode, e.Message, e.InnerException);
            }
            catch (Exception)
            {
                isSuccess = false;
                // throw new Exception();
            }

            return isSuccess;
        }

        public static void SaveUserSession(string username, string password, string domain = "gsb")
        {
            var s = new DirectorySearcher(
                new DirectoryEntry("LDAP://" + Server + "/DC=" + domain + ",DC=local", username, password))
            {
                SearchScope = SearchScope.Subtree,
                Filter = Format("(&(objectClass=user)(sAMAccountName=" + username + "))")
            };


            var user = s.FindOne();

            if (user == null) return;

            using (var uc = new UserContext())
            {
                {
                    try
                    {
                        UserSession.Id = (from u in uc.DataContext.Users where u.Username == username select u).First().Id;
                        UserSession.Visitor = (from u in uc.DataContext.Users where u.Username == username select u.Role.Name).First().Equals("visiteur");
                    }
                    catch (InvalidOperationException)
                    {
                        
                    }

                    UserSession.Username = username;
                }
            }
        }
    }
}