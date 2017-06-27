using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.World
{
    public class Building : MonoBehaviour, ITargetable, IInteractive, IHittable, IHealth
    {
        [System.Serializable]
        public class Settings
        {
            public int MaxHealth = 10;
            public int DeathMoraleLoss = 1;
        }



        private int health;

        public Settings settings;

        [Space]
        [SerializeField]
        private Team team;

        public event System.Action<float> OnHealthChanged;



        public event System.Action OnDestroyed;
        public bool Targetable  {  get { return true; } }
        public bool Hittable { get { return true; } }
        public GameObject Owner { get { return gameObject; } }
        public Team Team { get { return team; } }
        public Vector3 position { get { return transform.position; } }
        public float MaxHealth { get { return settings.MaxHealth; } }
        public float Health { get { return health; } }

        void Awake()
        {
            health = settings.MaxHealth;
        }
        public void OnDestroy()
        {
            if (OnDestroyed != null)
                OnDestroyed();
        }


        public void OnHit(int damage)
        {
            this.health -= damage;
            this.OnHealthChanged(this.health);
            if (this.health <= 0)
                OnHealthZero();
        }
        
        public void OnHealthZero()
        {
            if (OnDestroyed != null)
                OnDestroyed();
            GameObject.Destroy(gameObject);
            team.Morale -= settings.DeathMoraleLoss;
        }

    }
}