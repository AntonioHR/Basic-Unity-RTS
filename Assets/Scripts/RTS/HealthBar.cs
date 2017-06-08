using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour {
    public Transform defaultOwner;

    private IHealth owner;
    private Slider slider;



    public IHealth Owner
    {
        get
        {
            return owner;
        }
        set
        {
            if(owner != null)
                owner.OnHealthChanged -= SetHealth;
            this.owner = value;
            owner.OnHealthChanged += SetHealth;
        }
    }



    void Awake()
    {
        if (defaultOwner != null)
            owner = defaultOwner.GetComponent<IHealth>();
        if(owner != null)
            owner.OnHealthChanged += SetHealth;
        slider = GetComponent<Slider>();
    }

    void SetHealth(float health)
    {
        slider.value = health / owner.MaxHealth;
    }
}
