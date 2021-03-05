using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    private Image HPBar;

    // Start is called before the first frame update
    void Start()
    {
        HPBar = GameObject.Find("HPBar").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();   
    }

    public void OnDamage(float damage)
    {
        base.OnDamage(damage);
    }

    public void UpdateUI()
    {
        HPBar.fillAmount = _health / startingHealth;
    }
}
