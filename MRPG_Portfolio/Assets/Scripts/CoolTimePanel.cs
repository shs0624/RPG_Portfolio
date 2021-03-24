using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimePanel : MonoBehaviour
{
    public bool canUse = false;

    private float coolTime;
    private float Timer = 0f;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 1f;
    }

    public void timeSetting(float cool)
    {
        canUse = false;
        coolTime = cool;
        Timer = coolTime;
    }

    private void Update()
    {
        if (!canUse)
        {
            Timer -= Time.smoothDeltaTime;
            image.fillAmount = Timer / coolTime;
            if (Timer <= 0)
            {
                canUse = true;
                image.fillAmount = 0f;
            }
        }
    }
}
