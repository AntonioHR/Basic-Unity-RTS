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
        }



        private int health;

        public Settings settings;

        [Space]
        public Team team;

        public event System.Action<float> OnHealthChanged;



        public event System.Action OnDestroyed;
        public bool Targetable  {  get { return true; } }
        public bool Hittable { get { return true; } }
        public GameObject Owner { get { return gameObject; } }
        public Vector3 position { get { return transform.position; } }
        public float MaxHealth { get { return settings.MaxHealth; } }
        public float Health { get { return health; } }



        void Awake()
        {
            health = settings.MaxHealth;
        }
        public void OnDestroy()
        {
        }



        public void TargetBy(ITargetReceiver targetReceiver)
        {
            throw new System.NotImplementedException();
        }

        public void Hit(int damage)
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
        }

    }
}