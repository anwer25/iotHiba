using System;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using Methods;
using Random = UnityEngine.Random;

public class DroneInfo : MonoBehaviour
{
    
    private Rigidbody _rb;
    private readonly Methods.Methods _methods = new Methods.Methods();
    public string droneId;
    public string zoneName;
    public bool trusted;
    public bool canMove;
    public bool leader;
    public string error;
    public int energy;
    private MeshRenderer _meshRenderer;
    public Material[] newMaterial;
    


    private void Start()
    {
        string[] zones = { "A", "B", "Any" };
        droneId = _methods.GenerateUniqueId();
        zoneName = _methods.GenerateRandomZone(zones);
        energy = UnityEngine.Random.Range(1000, 9999999);
        trusted = false;
        canMove = true;
        leader = false;
        
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    
    

    private void HandleSenderMethodComplete(string responseJson)
    {
        var parsedData = JsonUtility.FromJson<JsonBodyResponse>(responseJson);
        droneId = parsedData.data.droneId;
        zoneName = parsedData.data.zoneName;
        trusted = parsedData.data.trusted;
        canMove = parsedData.canMove;
        error = parsedData.error;
        leader = parsedData.data.leader;

    }
    
    public void MaterialChanger(bool value)
    {
        if (_meshRenderer == null) return;
        // check if drone are trusted and not leader to add blue color to it
        if(value)
        {
            _meshRenderer.material = newMaterial[0];
            trusted = value;
            // check if drone are leader to add green color to it
        }else if (leader)
        {
            
            _meshRenderer.material = newMaterial[1];
        }
        // add red color to drone wish mains not trusted
        else
        {
            trusted = value;
            _meshRenderer.material = newMaterial[2];
        }
    }

    public void DroneIdChange(string id)
    {
        droneId = id;
    }


}
