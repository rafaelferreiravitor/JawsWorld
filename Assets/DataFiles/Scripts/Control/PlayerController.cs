using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Control
{

    public class PlayerController : MonoBehaviour
    {


        // Update is called once per frame
        void Update()
        {
            if (InteractWithCursorClick())
                return;
            /*else if (InteractWithMovement())
                return;
            */

        }

        private bool InteractWithCursorClick()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
                if (hits.Length > 0)
                {
                    foreach (RaycastHit item in hits)
                    {
                        CombatTarget combatTarget = item.transform.GetComponent<CombatTarget>();

                        if (combatTarget == null)
                            continue;

                        if (GetComponent<Fighter>().CanAttack(combatTarget.gameObject))
                        {
                            GetComponent<Fighter>().Attack(combatTarget.gameObject.GetComponent<Health>());
                            //GetComponent<Mover>().StartAction(combatTarget.transform.position);
                            return true;
                        }
                    }
                    GetComponent<Mover>().StartAction(hits[0].point);
                }
            }
            return false;
        }


        /*private bool MoveToCursor()
        {
            Ray ray = GetMouseRay();
            bool hasHit = Physics.Raycast(ray, out RaycastHit hit);
            Debug.DrawRay(ray.origin, ray.direction * 100);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().MoveTo(hit.point);
                    return true;
                }
            }
            return false;

        }*/

       private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        /*private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 2);
        }*/
    }
}