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
            public float MaxHealth = 10;
        }



        public Settings settings;

        public int Health;



        public bool Targetable  {  get { return true; } }
        public bool Hittable { get { return true; } }
        public GameObject Owner { get { return gameObject; } }
        public Vector3 position { get { return transform.position; } }



        public void Target(ITargetReceiver targetReceiver)
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
            GameObject.Destroy(gameObject);
        }


    }
}