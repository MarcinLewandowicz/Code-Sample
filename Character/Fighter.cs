using UnityEngine;
using ML.Systems;
using ML.Character;
using System;

namespace ML.Combat
{
    public class Fighter : MonoBehaviour, IActionable
    {
        [SerializeField] private Health target;
        [SerializeField] private Transform handTransform;
        private Inventory inventory;
        [SerializeField] private float timeSinceLastAttack = Mathf.Infinity;
        private Animator animator;
        public bool isAttacking = false;

        private void OnEnable()
        {
            inventory = GetComponent<Inventory>();
            inventory.OnWeaponChanged += OnWeaponChanged;
            Health health = GetComponent<Health>();
            health.OnDeath += StopAction;
        }

        private void OnDisable()
        {
            inventory.OnWeaponChanged -= OnWeaponChanged;
            Health health = GetComponent<Health>();
            health.OnDeath -= StopAction;
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;            
            if (target == null) { return; }
            if (!CanAttackTarget()) { return; }
            if (DistanceToTarget() <= inventory.CurrentWeapon.WeaponRange)
            {
                isAttacking = true;
                AttackBehaviour();
            }
            else if((DistanceToTarget() <= inventory.CurrentWeapon.WeaponRange+inventory.CurrentWeapon.WeaponRangeThreshold)&&(isAttacking))
            {
                AttackBehaviour();
            }
            else
            { 
                ResumeFollowing();
                MoveToTarget();
            }
        }
        public void StartAttack(Health target)
        {
            GetComponent<ActionScheduler>().SetAction(this);
            animator.ResetTrigger("stopAttack");
            animator.ResetTrigger("weaponChanged");
            this.target = target;
        }

        private void ResumeFollowing()
        {
            isAttacking = false;
            animator.SetBool("canAim", false);
        }

        public void StopAction()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
            animator.SetBool("canAim", false);
            target = null;
            isAttacking = false;
        }

        public void OnWeaponChanged()
        {
            StopAction();
        }

        //ANIMATION EVENT
        private void DealDamage()
        {
            if (target == null) { return; }
            if (DistanceToTarget() > inventory.CurrentWeapon.WeaponRange + inventory.CurrentWeapon.WeaponRangeThreshold/2)
            {
                target.MissedAttack();
            }
            else
            {
                target.TakeDamage(inventory.CurrentWeapon.WeaponDamage, this);
            }
        }


        //ANIMATION EVENT
        private void SpawnWeaponParticles()
        {
            inventory.GetCurrentWeaponInstance().GetComponent<WeaponFXSpawner>().SpawnParticles();  
        }

        //ANIMATION EVENT
        private void PlayWeaponSound()
        {
            inventory.GetCurrentWeaponInstance().GetComponent<WeaponSounds>().PlaySound();
        }



        private void AttackBehaviour()
        {
            animator.SetBool("canAim", true);
            GetComponent<Mover>().StopMovement();
            transform.LookAt(target.transform.position);
            if (timeSinceLastAttack > inventory.CurrentWeapon.AttackInterval)
            {
                timeSinceLastAttack = 0f;
                Attack();
            }
        }

        private void MoveToTarget()
        {
            animator.ResetTrigger("attack");
            GetComponent<Mover>().MoveToTarget(target.transform.position);
        }

        private void Attack()
        {
            GetComponent<Mover>().StopMovement();
            animator.SetTrigger("attack");
        }


        private float DistanceToTarget()
        {
            if (target == null) { return Mathf.Infinity; }
            return Vector3.Distance(transform.position, target.transform.position);
        }

        private bool CanAttackTarget()
        {
            return !target.IsDead();
        }
    }

}