using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class Building : MonoBehaviour, IBuilding
    {
        [System.Serializable]
        public class Settings
        {
            public int MaxHealth = 10;
        }



        public Settings settings;

        int Health;



        public event System.Action OnDestroyed;
        public bool Targetable  {  get { return true; } }
        public bool Hittable { get { return true; } }
        public GameObject Owner { get { return gameObject; } }
        public Vector3 position { get { return transform.position; } }



        void Awake()
        {
            Health = settings.MaxHealth;
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
            this.Health -= damage;
            if (this.Health <= 0)
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