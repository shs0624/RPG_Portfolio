using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float maxChaosPoint;
    public float maxHitPoint;
    public GameObject BossHitUI;
    public Image currentChaosGlobe;
    public Image currentHitBar;
    public Text chaosText;

    private float chaosPoint;
    private float hitPoint;

    // Start is called before the first frame update
    void Start()
    {
        SetChaosGuage(0f);
        SetHitGuage(maxHitPoint);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChaosGlobe();
    }

    public void SetChaosGuage(float p)
    {
        chaosPoint = p;
        UpdateChaosGlobe();
    }

    public void SetHitGuage(float p)
    {
        hitPoint = p;
        UpdateHitBar();
    }

    public void ShowBossHitBar()
    {
        BossHitUI.SetActive(!BossHitUI.activeSelf);
    }

    private void UpdateHitBar()
    {
        float ratio = hitPoint / maxHitPoint;
        currentHitBar.fillAmount = ratio;
    }

    private void UpdateChaosGlobe()
    {
        float ratio = chaosPoint / maxChaosPoint;
        currentChaosGlobe.rectTransform.localPosition = new Vector3(0, currentChaosGlobe.rectTransform.rect.height * ratio - currentChaosGlobe.rectTransform.rect.height, 0);
        chaosText.text = chaosPoint.ToString("0");
    }
}
