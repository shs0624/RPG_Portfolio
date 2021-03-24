using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public float floatingSpeed;
    public float alphaSpeed;
    public float destroyTime;

    Text text;
    Color firstColor;
    Color alpha;
    
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<Text>();
        firstColor = text.color;
        alpha = text.color;
    }

    private void OnEnable()
    {
        alpha = firstColor;
        StartCoroutine(FadeOutCoroutine());
    }

    public void SetText(float damage)
    {
        text.text = damage.ToString();
    }

    IEnumerator FadeOutCoroutine()
    {
        float Timer = destroyTime;
        while(Timer > 0)
        {
            Timer -= Time.deltaTime;

            transform.Translate(new Vector3(0, floatingSpeed * Time.deltaTime, 0));
            alpha.a -= Time.deltaTime * alphaSpeed;
            text.color = alpha;

            yield return null;
        }
        OffObj();
        yield return null;
    }

    private void OffObj()
    {
        alpha.a = 255f;
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
