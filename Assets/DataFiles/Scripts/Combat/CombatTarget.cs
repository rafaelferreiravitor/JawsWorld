using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {

            if (callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(gameObject.GetComponent<Health>());

                }
                return true;
            }
            return false;
        }
    }
}