using UnityEngine;

public class ServerEndPoint : MonoBehaviour
{
    public static ServerEndPoint instance;

    [SerializeField] string serverIPAdress; // this is the base url which is http://127.0.0.1

    enum ServerType {Dev, Stage, Prod }
    [SerializeField] ServerType serverType;
    enum ServerProtocol {Http, Https }
    [SerializeField] ServerProtocol protocol;

    string Protocol
    {
        get
        {
            switch (protocol)
            {
                case ServerProtocol.Http:
                    return "http";
                case ServerProtocol.Https:
                    return "https";
                default:
                    return "http";
            }
        }
    }
    int GetPort
    {
        get
        {
            switch (serverType)
            {
                case ServerType.Dev:
                    return 3000;
                case ServerType.Stage:
                    return 4000;
                case ServerType.Prod:
                    return 8000;
                default:
                    return 3000;
            }
        }
    }
    public string GetUserEndPoint
    {
        get { return $"{Protocol}://{serverIPAdress}:{GetPort}/get-user"; }
    }
    public string AddUserEndPoint
    {
        get { return $"{Protocol}://{serverIPAdress}:{GetPort}/add-user"; }
    }
    public string GetAllUserEndPoint
    {
        get { return $"{Protocol}://{serverIPAdress}:{GetPort}/get-all-users"; }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }else Destroy(gameObject);
    }
}
