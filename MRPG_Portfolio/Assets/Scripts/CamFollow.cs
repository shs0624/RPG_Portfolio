using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Vector3 offset;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position - target.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos = target.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 4f);
    }
}
