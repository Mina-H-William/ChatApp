namespace ReactApp.Server.Hubs
{
    public class UserConnectionManager
    {
        private readonly IDictionary<string, string> _connections = new Dictionary<string, string>();

        private readonly IDictionary<string, List<string>> _Groupconnections = new Dictionary<string, List<string>>();

        /////////////// Private Methods ////////////////////

        public void AddConnection(string userid, string connectionId)
        {
            _connections[userid] = connectionId;
        }

        public void RemoveConnection(string connectionId)
        {
            var user = _connections.FirstOrDefault(c => c.Value == connectionId).Key;
            if (user != null)
            {
                _connections.Remove(user);
            }
        }

        public string GetConnection(string userid)
        {
            return _connections.TryGetValue(userid, out var connectionId) ? connectionId : null;
        }

        public List<string> GetAllConnections()
        {
            return _connections.Keys.ToList();
        }

        /////////////////////// Group Methods /////////////////////////////

        /*public void AddGroupConnection(string userid, string connectionId)
        {
            _connections[userid] = connectionId;
        }

        public void RemoveGroupConnection(string connectionId)
        {
            var user = _connections.FirstOrDefault(c => c.Value == connectionId).Key;
            if (user != null)
            {
                _connections.Remove(user);
            }
        }

        public string GetGroupConnection(string userid)
        {
            return _connections.TryGetValue(userid, out var connectionId) ? connectionId : null;
        }

        public IDictionary<string, string> GetAllGroupConnections()
        {
            return _connections;
        }*/

    }
}
