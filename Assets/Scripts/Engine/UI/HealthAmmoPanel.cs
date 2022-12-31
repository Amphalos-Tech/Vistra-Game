using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAmmoPanel : MonoBehaviour
{
    private Animator healthAnimator, ammoAnimator;
    private Image healthBar, ammoBar;

    private float _health, maxHealth = 100f;
    private byte _ammo, maxAmmo = 25;

    public float Health
    {
        get => _health;
        set
        {
            ModifyHealth(value);
            _health = value;
        }
    }

    public byte Ammo 
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
        healthBar = transform.GetChild(0).GetComponentInChildren<Image>();
        ammoBar = transform.GetChild(1).GetComponentInChildren<Image>();
        healthBar.fillMethod = Image.FillMethod.Vertical;
        ammoBar.fillMethod = Image.FillMethod.Vertical;

        var animators = GetComponentsInChildren<Animator>();
        healthAnimator = animators[0];
        ammoAnimator = animators[1];
    }

    void ModifyHealth(float newHealth)
    {
        healthAnimator.SetTrigger("RotateTrigger");
        StartCoroutine("AnimateHealth", newHealth);
        
    }

    IEnumerable AnimateHealth(float newHealth) //private variable acts as past value since this is called before the value is updated
    {
        if (newHealth < _health)
        {
            for (float f = _health; f >= newHealth; f -= .1f)
            {
                healthBar.fillAmount = f / maxHealth;
                yield return null;
            }
        }
        else
        {
            for (float f = _health; f <= newHealth; f += .1f)
            {
                healthBar.fillAmount = f / maxHealth;
                yield return null;
            }
        }
    }

    void ModifyAmmo(byte newAmmo)
    {
        ammoAnimator.SetTrigger("RotateTrigger");
        StartCoroutine("AnimateAmmo", newAmmo);
    }

    IEnumerable AnimateAmmo(byte newAmmo)
    {
        if (newAmmo < _ammo)
        {
            for (float f = _ammo; f >= newAmmo; f -= .1f)
            {
                ammoBar.fillAmount = f / maxAmmo;
                yield return null;
            }
        }
        else
        {
            for (float f = _ammo; f <= newAmmo; f += .1f)
            {
                ammoBar.fillAmount = f / maxAmmo;
                yield return null;
            }
        }
    }
}
