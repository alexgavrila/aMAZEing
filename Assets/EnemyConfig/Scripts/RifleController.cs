using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleController : MonoBehaviour
{
    // Different positions and rotations for the different states of the character holding this rifle
    private readonly Vector3 walkFirePosition = new Vector3(0.368f, -0.059f, -0.024f);
    private readonly Vector3 walkFireRotation = new Vector3(-46.456f, 93.266f, -196.42f);
    
    private readonly Vector3 walkPosition = new Vector3(0.394f, -0.0358f, 0.0005f);
    private readonly Vector3 walkRotation = new Vector3(-27.094f, 65.321f, -151.58f);
        
    private readonly Vector3 idleFirePosition = new Vector3(0.3955f, -0.0364f, -0.0003f);
    private readonly Vector3 idleFireRotation = new Vector3(-4.414f, 59.651f, -184.337f);  
    
    private readonly Vector3 idlePosition = new Vector3(0.364f, -0.056f, -0.021f);
    private readonly Vector3 idleRotation = new Vector3(-14.422f, 96.594f, -166.348f);


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
