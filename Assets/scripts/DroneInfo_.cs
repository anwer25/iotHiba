using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneInfo_ : MonoBehaviour
{
    public string zoneName;

    public bool trusted;

    public bool leader;
    public int energy;
    // Start is called before the first frame update
    void Start()
    {
        zoneName = "A";
        trusted = true;
        leader = true;
        energy = UnityEngine.Random.Range(1000, 5000);
    }
}
