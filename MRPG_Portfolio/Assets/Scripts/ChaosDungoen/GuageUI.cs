using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuageUI : MonoBehaviour
{
    public float maxChaosPoint;
    public Image currentChaosGlobe;
    public Text chaosText;

    private float chaosPoint;

    // Start is called before the first frame update
    void Start()
    {
        SetGuage(0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChaosGlobe();
    }

    public void SetGuage(float p)
    {
        chaosPoint = p;
        UpdateChaosGlobe();
    }

    private void UpdateChaosGlobe()
    {
        float ratio = chaosPoint / maxChaosPoint;
        currentChaosGlobe.rectTransform.localPosition = new Vector3(0, currentChaosGlobe.rectTransform.rect.height * ratio - currentChaosGlobe.rectTransform.rect.height, 0);
        chaosText.text = chaosPoint.ToString("0");
    }
}
