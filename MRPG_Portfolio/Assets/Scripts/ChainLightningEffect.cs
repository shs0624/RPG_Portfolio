using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningEffect : MonoBehaviour
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

        OffEffect();
        yield return null;
    }

    private void OffEffect()
    {
        Timer = 0f;
        gameObject.SetActive(false);
    }
}
