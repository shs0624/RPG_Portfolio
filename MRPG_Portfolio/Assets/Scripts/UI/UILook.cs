using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILook : MonoBehaviour
{
    Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.back,
            mainCam.transform.rotation * Vector3.down);
    }
}
