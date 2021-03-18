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
    Color alpha;
    
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<Text>();
        alpha = text.color;
        Invoke("OffObj", destroyTime);
    }

    public void SetText(float damage)
    {
        text.text = damage.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, floatingSpeed * Time.deltaTime, 0));

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    private void OffObj()
    {
        alpha.a = 255f;
        gameObject.SetActive(false);
    }
}
