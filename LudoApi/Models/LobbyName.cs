namespace LudoApi.Models
{
    public class LobbyName
    {
        private const string LobbyPrefix = "lobby-";

        private readonly string _lobbyName;

        public LobbyName(string lobbyName)
        {
            _lobbyName = TransformName(lobbyName);
        }

        public override string ToString()
        {
            return _lobbyName;
        }

        private static string TransformName(string lobbyName)
        {
            return LobbyPrefix + lobbyName.Replace(LobbyPrefix, string.Empty);
        }

        public override bool Equals(object obj)
        {
            if (obj is string rawLobbyName)
            {
                return Equals(new LobbyName(rawLobbyName));
            }

            if (obj is LobbyName lobbyName)
            {
                return Equals(lobbyName);
            }

            return false;
        }

        protected bool Equals(LobbyName other)
        {
            return string.Equals(_lobbyName, other.ToString());
        }

        public override int GetHashCode()
        {
            return _lobbyName != null ? _lobbyName.GetHashCode() : 0;
        }
    }
}