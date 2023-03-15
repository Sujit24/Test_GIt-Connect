using NetTrackRepository;

namespace NetTrackBiz
{
    internal class UserSessionBiz
    {
        private UserSessionRepository _UserSessionRepository;

        //default constructor
        public UserSessionBiz()
        {
            _UserSessionRepository = new UserSessionRepository();
        }
        public string GetUserSessionStatus(int sessionId)
        {
            return _UserSessionRepository.GetUserSessionStatus(sessionId);
        }
    }
}
