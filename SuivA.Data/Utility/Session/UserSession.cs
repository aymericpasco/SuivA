using SuivA.Data.Context;

namespace SuivA.Data.Utility.Session
{
    public sealed class UserContext : GlobalAppContext
    {
    }

    public class UserSession
    {
        public static long Id;
        public static string Username;
        public static bool Visitor;
    }
}