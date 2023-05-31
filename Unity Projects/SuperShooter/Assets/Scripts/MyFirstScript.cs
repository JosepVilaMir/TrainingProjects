using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFirstScript : MonoBehaviour
{
    [SerializeField]
    private int speed;
    // Start is called before the first frame update
    void Start()
    {
        print("Start");
    }

    // Update is called once per frame
    void Update()
    {
        print(speed);
    }

    void OnDestroy()
    {
        print("Destroyed");
    }
}