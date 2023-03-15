using System.Data;
using NetTrackDBContext;

namespace NetTrackRepository
{
    public class UserSessionRepository
    {
        private DBUserSession _dbUserSession;

        // default constructor
        public UserSessionRepository()
        {
            _dbUserSession = new DBUserSession();
        }

        public string GetUserSessionStatus(int sessionId)
        {
            string sessionAlive = "No";
            DataTable dtUserSession = _dbUserSession.GetUserSessionStatus(sessionId);
            if (dtUserSession.Rows.Count > 0)
            {
                sessionAlive = "Yes";
            }
            return sessionAlive;
        }
    }
}
