using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAmmoPanel : MonoBehaviour
{
    private Animator healthAnimator, ammoAnimator;
    private Image healthBar, ammoBar;

    private float _health, maxHealth;
    private int _ammo, maxAmmo;

    public float Health
    {
        get => _health;
        set
        {
            ModifyHealth(value);
            _health = value;
        }
    }

    public int Ammo 
    {
        get => _ammo;
        set
        {
            ModifyAmmo(value);
            _ammo = value;
        }
    }
  
    
    // Start is called before the first frame update
    void Start()
    {
        healthBar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        ammoBar = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        healthBar.type = Image.Type.Filled;
        ammoBar.type = Image.Type.Filled;
        healthBar.fillMethod = Image.FillMethod.Vertical;
        ammoBar.fillMethod = Image.FillMethod.Vertical;

        var animators = GetComponentsInChildren<Animator>();
        healthAnimator = animators[0];
        ammoAnimator = animators[1];
    }

    void ModifyHealth(float newHealth)
    {
        StartCoroutine("AnimateHealth", newHealth);
        
    }

    IEnumerator AnimateHealth(float newHealth) //private variable acts as past value since this is called before the value is updated
    {
        if (newHealth < _health)
        {
            healthAnimator.SetTrigger("RotateTrigger");
            for (float f = _health; f >= newHealth; f -= .1f)
            {
                healthBar.fillAmount = f / maxHealth;
                yield return null;
            }
        }
        else
        {
            healthAnimator.SetTrigger("ReverseRotateTrigger");
            for (float f = _health; f <= newHealth; f += .1f)
            {
                healthBar.fillAmount = f / maxHealth;
                yield return null;
            }
        }
    }

    void ModifyAmmo(int newAmmo)
    {
        StartCoroutine("AnimateAmmo", newAmmo);
    }

    IEnumerator AnimateAmmo(int newAmmo)
    {
        if (newAmmo < _ammo)
        {
            ammoAnimator.SetTrigger("RotateTrigger");
            for (float f = _ammo; f >= newAmmo; f -= .1f)
            {
                ammoBar.fillAmount = f / maxAmmo;
                yield return null;
            }
        }
        else
        {
            ammoAnimator.SetTrigger("ReverseRotateTrigger");
            for (float f = _ammo; f <= newAmmo; f += .1f)
            {
                ammoBar.fillAmount = f / maxAmmo;
                yield return null;
            }
        }
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        _health = newMaxHealth;
    }

    public void SetMaxAmmo(int newMaxAmmo)
    {
        maxAmmo = newMaxAmmo;
        _ammo = newMaxAmmo;
    }
}
