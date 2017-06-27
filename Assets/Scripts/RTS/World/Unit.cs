using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS.World.UnitBehavior;
using RTS.World.Squads;
using System;

namespace RTS.World
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour, IHittable, IInteractive, IHealth, ISelectionUnit
    {
        [System.Serializable]
        public class Settings
        {
            public UnitAttackHandler.Settings attackSettings;
            public float range;
            public int damage;
            public float MaxHealth = 100;
        }



        public Settings settings;

        public Transform selectionIndicator;
        public UnitAnimationHandler animationHandler;

        [Space()]
        [SerializeField]
        private Team team;

        public event System.Action OnDestroyed;
        public event System.Action<float> OnHealthChanged;
        public event Action OnSelected
        {
            add
            {
                squadHandler.OnSelected += value;
            }
            remove
            {
                squadHandler.OnSelected -= value;
            }
        }
        public event Action OnDeselected
        {
            add
            {
                squadHandler.OnDeselected += value;
            }
            remove
            {
                squadHandler.OnDeselected -= value;
            }
        }

        NavMeshAgent navMeshAgent;
        float health;
        UnitSquadHandler squadHandler;
        UnitAttackHandler attackHandler;

        public UnitAction CurrentAction { get; set; }



        public bool CanTarget { get { return true; } }
        public bool Targetable { get { return true; } }
        public bool Hittable { get { return true; } }

        public float MaxHealth { get { return settings.MaxHealth; } }
        public float Health { get { return health; } }
        public float AttackDamage { get { return settings.damage; } }
        public float Range { get { return settings.range; } }


        public GameObject Owner { get { return gameObject; } }
        public Vector3 position { get { return transform.position; } }
        public Squad Squad { get { return squadHandler.Squad; } }
        public Team Team { get { return team; } }

        public bool IsInRange { get { return CurrentAction != null && 
                    CurrentAction.Target != null && attackHandler.IsInRange(CurrentAction.Target.position); } }

        public bool Selectable
        {
            get
            {
                return squadHandler.Selectable;
            }
        }

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            squadHandler = new UnitSquadHandler(this);
            attackHandler = new UnitAttackHandler(this, animationHandler, settings.attackSettings);
        }
        void Start()
        {
            health = MaxHealth;
        }
        void Update()
        {
            if (CurrentAction == null)
                return;
            if (!CurrentAction.IsValid)
            {
                if (CurrentAction.Mode != ActionMode.Attack && attackHandler.IsAttacking)
                    attackHandler.StopAttacking();
                CurrentAction = null;
                return;
            }
            switch (CurrentAction.Mode)
            {
                case ActionMode.Attack:
                    var inRange = IsInRange;
                    navMeshAgent.SetDestination(CurrentAction.position ?? default(Vector3));
                    if (!inRange && attackHandler.IsAttacking)
                        attackHandler.StopAttacking();
                    else if (inRange && !attackHandler.IsAttacking)
                        attackHandler.StartAttacking(CurrentAction.Target);
                    break;
                case ActionMode.Move:
                    if (attackHandler.IsAttacking)
                        attackHandler.StopAttacking();
                    navMeshAgent.SetDestination(CurrentAction.position ?? default(Vector3));
                    break;
                case ActionMode.Idle:
                    if (attackHandler.IsAttacking)
                        attackHandler.StopAttacking();
                    break;
                default:
                    break;
            }
        }
        void OnDestroy()
        {
            if (OnDestroyed != null)
                OnDestroyed();
        }
        
        public void OnHit(int damage)
        {
            var delta = -damage;
            this.health -= damage;
            if (OnHealthChanged != null)
                OnHealthChanged(health);
            if (this.health <= 0)
                Die();
        }

        void Die()
        {
            GameObject.Destroy(gameObject);
        }
    }
}