using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new weapon", order = 0)]
    public class Weapon : ScriptableObject
    {

        [SerializeField] AnimatorOverrideController weaponOverrideController = null;
        [SerializeField] GameObject EquippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 20;

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (EquippedPrefab != null)
            {
                Instantiate(EquippedPrefab, handTransform);
            }
            if (weaponOverrideController != null)
            {
                animator.runtimeAnimatorController = weaponOverrideController;
            }
        }
    }
}