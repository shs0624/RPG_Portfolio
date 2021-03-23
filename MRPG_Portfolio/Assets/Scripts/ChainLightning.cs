using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    public float effectTime;

    LineRenderer _lineRenderer;
    float Timer = 0f;

    private void Awake()
    {
        Timer = 0f;
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        Timer = 0f;
        Invoke("OffEffect", effectTime);
        StartCoroutine(EffectCoroutine());
    }

    public void PosSetting(Vector3 start, Vector3 end)
    {
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }

    IEnumerator EffectCoroutine()
    {
        while(Timer < effectTime - 0.2f)
        {
            _lineRenderer.SetWidth(0.2f, 0.2f);

            yield return new WaitForSeconds(0.1f);

            Timer += 0.1f;

            _lineRenderer.SetWidth(0.5f, 0.5f);

            yield return new WaitForSeconds(0.1f);

            Timer += 0.1f;
        }

        yield return null;
    }

    private void OffEffect()
    {
        Timer = 0f;
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
