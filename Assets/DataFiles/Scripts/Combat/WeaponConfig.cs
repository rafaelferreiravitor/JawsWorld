using RPG.Core;
using RPG.Attributes;
using System;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {

        [SerializeField] AnimatorOverrideController weaponOverrideController = null;
        [SerializeField] Weapon EquippedPrefab = null;
        [SerializeField] float weaponRange = 20f;
        [SerializeField] float weaponDamage = 20;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator,float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(instigator,target, calculatedDamage);
        }

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;

            Transform handTransform;
            handTransform = GetTransform(rightHand, leftHand);

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (EquippedPrefab != null)
            {
                weapon = Instantiate(EquippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            if (weaponOverrideController != null)
            {
                animator.runtimeAnimatorController = weaponOverrideController;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DestroyingOldWeapon";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHand;
            }
            else
            {
                handTransform = leftHand;
            }

            return handTransform;
        }
    }
}