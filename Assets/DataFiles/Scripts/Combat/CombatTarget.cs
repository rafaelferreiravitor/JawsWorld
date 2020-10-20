using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Resources;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {

            if (callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(gameObject.gameObject.GetComponent<Health>());

                }
                return true;
            }
            return false;
        }
    }
}