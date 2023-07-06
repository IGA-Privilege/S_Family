using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_FacingCam : MonoBehaviour
{
    public Camera facingCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camDir = facingCam.transform.forward;
        camDir.y = 0;
        transform.rotation = Quaternion.LookRotation(camDir);
    }
}
