using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new weapon", order = 0)]
    public class Weapon : ScriptableObject
    {

        [SerializeField] AnimatorOverrideController weaponOverrideController = null;
        [SerializeField] GameObject weapon = null;

        public void Spawn(Transform handTransform, Animator animator)
        {
            Instantiate(weapon, handTransform);
            animator.runtimeAnimatorController = weaponOverrideController;
        }
    }
}