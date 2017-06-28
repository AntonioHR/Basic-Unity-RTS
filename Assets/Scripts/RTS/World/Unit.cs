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
    public class Unit : MonoBehaviour, IHittable, IInteractive, IHealth, ISelectionUnit, IHighlightable
    {
        public enum ClassType { Infantry, Artillary, Cavalry, Siege}
        [System.Serializable]
        public class Settings
        {
            public UnitAttackHandler.Settings attackSettings;
            public ClassType Type;
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

        //public Transform selectionIndicator;
        public UnitAnimationHandler animationHandler;

        [Space()]
        [SerializeField]
        private Team startTeam;

        public event System.Action OnDestroyed;
        public event System.Action<float> OnHealthChanged;
        public event System.Action OnHighlightOn;
        public event System.Action OnHighlightOff;
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
        Team team;

        public UnitAction CurrentAction { get; set; }



        public bool CanTarget { get { return true; } }
        public bool Targetable { get { return true; } }
        public bool Highlightable { get { return true; } }
        public bool Initialized { get; private set; }

        public float MaxHealth { get { return settings.MaxHealth; } }
        public float Health { get { return health; } }
        public float AttackDamage { get { return settings.damage; } }
        public float Range { get { return settings.range; } }
        public ClassType Type { get { return settings.Type; } }

        public GameObject Owner { get { return gameObject; } }
        public Vector3 position { get { return transform.position; } }
        public Squad Squad { get { return squadHandler.Squad; } }
        public Team Team { get { return team; } }
        public Team StartTeam { set { Debug.Assert(!Initialized); startTeam = value; } }

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
            this.team = startTeam;
            Initialized = true;
            navMeshAgent = GetComponent<NavMeshAgent>();
            squadHandler = new UnitSquadHandler(this);
            attackHandler = new UnitAttackHandler(this, animationHandler, settings.attackSettings);
        }
        void Start()
        {
			curPos = transform.position;
			oldPos = curPos;
			//animationHandler.setSpeed (1.0f);
            health = MaxHealth;
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
            if (!CurrentAction.IsValid)
            {
                if (CurrentAction.Mode != ActionMode.Attack && attackHandler.IsAttacking)
                    attackHandler.StopAttacking();
                CurrentAction = null;
                return;
            }
            switch (CurrentAction.Mode)
            {
                //Nota de Rodrigo pra ele mesmo. Refatorar todo esse trecho!
                case ActionMode.Attack:
                    var inRange = IsInRange;
                    navMeshAgent.SetDestination(CurrentAction.position ?? default(Vector3));
                    if (!inRange && attackHandler.IsAttacking)
                    {
                        attackHandler.StopAttacking();
                    }
                    else if (inRange && !attackHandler.IsAttacking)
                    {
                        if (CurrentAction.Target != null)
                        {
                            attackHandler.StartAttacking(CurrentAction.Target);
                        }
                    }
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

        public void HighlightOn()
        {
            if (OnHighlightOn != null)
                OnHighlightOn();
        }
        public void HighlightOff()
        {
            if (OnHighlightOff != null)
                OnHighlightOff();
        }


        void Die()
        {
            GameObject.Destroy(gameObject);
        }
    }
}