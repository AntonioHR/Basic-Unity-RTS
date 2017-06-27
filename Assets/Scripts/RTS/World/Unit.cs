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



		//animação
		public Vector3 curPos;
		public Vector3 oldPos;
		public Vector3 velocity;
		public float speed;

        public Settings settings;

        public Transform selectionIndicator;
        public UnitAnimationHandler animationHandler;

        [Space()]
        public Team team;

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
        int health;
        UnitSquadHandler squadHandler;
        UnitAttackHandler attackHandler;

        public ActionInfo CurrentAction { get; set; }



        public bool CanTarget { get { return true; } }
        public bool Targetable { get { return true; } }
        public bool Hittable { get { return true; } }

        public float MaxHealth { get { return settings.MaxHealth; } }
        public float Health { get { return health; } }
        public float AttackDamage { get { return settings.damage; } }
        public float Range { get { return settings.range; } }
        public bool Destroyed { get; private set; }


        public GameObject Owner { get { return gameObject; } }
        public Vector3 position { get { return transform.position; } }
        public Squad Squad { get { return squadHandler.Squad; } }

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
			curPos = transform.position;
			oldPos = curPos;
			//animationHandler.setSpeed (1.0f);
        }
        void Update()
        {

			//cálculos
			curPos = transform.position;
			velocity = (curPos - oldPos) / Time.deltaTime;
			speed = Mathf.Sqrt (Mathf.Pow (velocity.x, 2) + Mathf.Pow (velocity.y, 2) + Mathf.Pow (velocity.z, 2));
			if (curPos != oldPos) {
				animationHandler.SetWalking (true, speed);
			} else {
				animationHandler.SetWalking (false, speed);
			}
			oldPos = curPos;

            if (CurrentAction == null)
                return;
            if (CurrentAction.Target != null && CurrentAction.Target.Destroyed)
            {
                CurrentAction = null;
                return;
            }
            switch (CurrentAction.Mode)
            {
                case ActionMode.Attack:
                    Debug.Log(CurrentAction.position);
                    var inRange = IsInRange;
                    navMeshAgent.SetDestination(CurrentAction.position);
                    if (!inRange && attackHandler.IsAttacking)
                        attackHandler.StopAttacking();
                    else if (inRange && !attackHandler.IsAttacking)
                        attackHandler.StartAttacking(CurrentAction.Target);
                    break;
                case ActionMode.Move:
                    navMeshAgent.SetDestination(CurrentAction.position);
                    break;
                default:
                    break;
            }
        }
        void OnDestroy()
        {
            Destroyed = true;
            if (OnDestroyed != null)
                OnDestroyed();
        }
        
        public void OnHit(int damage)
        {
            this.health -= damage;
            if (this.health <= 0)
                GameObject.Destroy(this);
        }



        
    }
}