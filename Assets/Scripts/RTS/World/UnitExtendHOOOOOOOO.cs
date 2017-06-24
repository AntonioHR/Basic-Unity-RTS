using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS.World.Units;


namespace RTS.World
{
    [RequireComponent(typeof(NavMeshAgent))]
	public class UnitExtendHOOOOOOOO : MonoBehaviour, ISelectable, IHighlightable, IHittable, ITargetReceiver, IInteractive
    {
        [System.Serializable]
        public class Settings
        {
            public float range;
            public float attackRangeTolerance = .1f;
            public int damage;
            public Material idleMaterial;
            public Material selectedMaterial;
			public float health = 100f;
        }



        public Settings settings;

        public Transform selectionIndicator;
        //public MeshRenderer meshRenderer;
		public LightInfantryAnimationHandler animationHandler;
		public Transform unitTransform;
		private Vector3 curPos;
		private Vector3 oldPos;
		private Vector3 velocity;
		private float speed;

        public event System.Action OnDestroyed;

        NavMeshAgent navMeshAgent;
        IHittable hitTarget;

        

        public bool Selectable { get { return true; } }
        public bool Highlightable { get { return true; } }
        public bool CanTarget { get { return true; } }
        public bool Targetable { get { return true; } }
        public bool Hittable { get { return true; } }
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




        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            //meshRenderer = meshRenderer != null? meshRenderer: GetComponent<MeshRenderer>();
            //meshRenderer.material = settings.idleMaterial;

			animationHandler.GetComponent<LightInfantryAnimationHandler>();
            animationHandler.OnHitFrame += HitCurrentTarget;
			curPos = new Vector3 (unitTransform.position.x, unitTransform.position.y, unitTransform.position.z);
			oldPos = curPos;
			velocity = new Vector3 (0.0f, 0.0f, 0.0f);
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

			//aqui eu descubro se estou andando
			curPos = unitTransform.position;
			string curpos = "curPos: " + curPos.ToString();
			string oldpos = "oldPos: " + oldPos.ToString();
			velocity = (curPos - oldPos) / Time.deltaTime;
			speed = Mathf.Sqrt(Mathf.Pow(velocity.x, 2) + Mathf.Pow(velocity.y, 2) + Mathf.Pow(velocity.z,2));
			Debug.Log ("speed: " + speed.ToString ());
			Debug.Log(curpos);
			Debug.Log (oldpos);
			if (curPos != oldPos) {
				animationHandler.SetWalking (true, speed);
			} else {
				animationHandler.SetWalking(false, 0f);
			}

			oldPos = curPos;
        }
        public void OnDestroy()
        {
            if (OnDestroyed != null)
                OnDestroyed();
        }



        public void Deselect()
        {
			animationHandler.SetSelected (false);
			selectionIndicator.gameObject.SetActive(false);
        }
        public void Select()
        {
			animationHandler.SetSelected (true);
			selectionIndicator.gameObject.SetActive(true);
        }
        
        public void HighlightOn()
        {
            //meshRenderer.material = settings.selectedMaterial;
        }
        public void HighlightOff()
        {
            //meshRenderer.material = settings.idleMaterial; 
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