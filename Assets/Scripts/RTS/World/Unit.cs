using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS.World.Units;
using RTS.World.Groups;
using System;

namespace RTS.World
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour, IHittable, ITargetReceiver, IInteractive, IHealth, ISelectionUnit
    {
        [System.Serializable]
        public class Settings
        {
            public float range;
            public float attackRangeTolerance = .1f;
            public int damage;
            public float MaxHealth = 100;
        }



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
                selectionHandler.OnSelected += value;
            }
            remove
            {
                selectionHandler.OnSelected -= value;
            }
        }
        public event Action OnDeselected
        {
            add
            {
                selectionHandler.OnDeselected += value;
            }
            remove
            {
                selectionHandler.OnDeselected -= value;
            }
        }

        NavMeshAgent navMeshAgent;
        IHittable hitTarget;
        int health;
        UnitSelectionHandler selectionHandler;

        
        
        public bool CanTarget { get { return true; } }
        public bool Targetable { get { return true; } }
        public bool Hittable { get { return true; } }

        public float MaxHealth { get { return settings.MaxHealth; } }
        public float Health { get { return health; } }

        public GameObject Owner { get { return gameObject; } }
        public Vector3 position { get { return transform.position; } }

        public bool IsInRange
        {
            get
            {
                if (hitTarget == null)
                    return false;
                Vector3 realDistance = hitTarget.position;
                realDistance.y = transform.position.y;
                return (Vector3.Distance(realDistance, transform.position) - settings.attackRangeTolerance) < settings.range;
            }
        }

        public SelectionGroup Group
        {
            get
            {
                return selectionHandler.Group;
            }
        }
        public bool Selectable
        {
            get
            {
                return selectionHandler.Selectable;
            }
        }

        private void Awake()
        {
            selectionHandler = new UnitSelectionHandler(this);
        }
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();

            animationHandler.GetComponent<UnitAnimationHandler>();
            animationHandler.OnHitFrame += HitCurrentTarget;

        }
        void Update()
        {
            var inrange = IsInRange;
            if (inrange)
            {
                Vector3 lookTarget = hitTarget.position;
                lookTarget.y = transform.position.y;
                transform.LookAt(lookTarget);
            }
            animationHandler.SetAttacking(inrange);
        }
        public void OnDestroy()
        {
            if (OnDestroyed != null)
                OnDestroyed();
        }
        
        public void TargetBy(ITargetReceiver targetReceiver)
        {
            throw new System.NotImplementedException();
        }

        public void SetTarget(ITargetable target, Vector3 position)
        {
            if (target != null)
            {
                navMeshAgent.stoppingDistance = 0;
                navMeshAgent.destination = position;
                var hittable = target as IHittable;
                setHitTarget(hittable);
                if (hittable != null)
                {
                    navMeshAgent.stoppingDistance = settings.range;
                }
            }
            else
                setHitTarget(null);
        }

        public void HitCurrentTarget()
        {
            if(hitTarget!= null)
                hitTarget.Hit(settings.damage);
        }

        public void Hit(int damage)
        {
            throw new System.NotImplementedException();
        }



        void clearHitTarget()
        {
            setHitTarget(null);
        }
        void setHitTarget(IHittable target)
        {
            if (hitTarget != null)
                hitTarget.OnDestroyed -= clearHitTarget;
            if (target != null)
                target.OnDestroyed += clearHitTarget;
            hitTarget = target;
        }
    }
}