namespace Helper
{
    [System.Serializable]
    public class DroneInfoHttpDataType
    {
        public string zoneName;
        public string droneId;
        public bool trusted;
        public bool leader;
        public int energy;
        public bool forced;
    }

    [System.Serializable]
    public class ServerInfoHttpDataType
    {
        public string serverName;
        public string zoneName;
    }

    [System.Serializable]
    public class JsonBodyResponse
    {
        public string error;
        public DroneInfoHttpDataType data;
        public bool canMove;
    }

    [System.Serializable]

    public class ServerBodyResponse
    {
        public string error;
        public ServerInfoHttpDataType info;
    }
}