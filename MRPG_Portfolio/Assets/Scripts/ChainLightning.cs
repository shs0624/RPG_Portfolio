using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        Invoke("OffEffect", 2f);   
    }

    public void PosSetting(Vector3 start, Vector3 end)
    {
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }

    IEnumerator EffectCoroutine()
    {
        while(gameObject.activeSelf)
        {
            
        }

        yield return null;
    }

    private void OffEffect()
    {
        gameObject.SetActive(false);
    }
}
