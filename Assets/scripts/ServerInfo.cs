using System.Collections;
using System.Collections.Generic;
using Helper;
using UnityEngine;


public class ServerInfo : MonoBehaviour
{
    public string url = "http://localhost:7071/api/addServerInfo";
    private Rigidbody _rb;

    private readonly Methods.Methods _methods = new Methods.Methods();

    public string serverName;
    public string zoneName;
    void Start()
    {
        var responce = StartCoroutine(_methods.GetterMethod(url));
        _methods.OnGetterComplete += HandleSenderMethodComplete;
    }
    
    private void HandleSenderMethodComplete(string responseJson)
    {
        var parsedData = JsonUtility.FromJson<ServerBodyResponse>(responseJson);
        serverName = parsedData.info.serverName;
        zoneName = parsedData.info.zoneName;

    }
}
