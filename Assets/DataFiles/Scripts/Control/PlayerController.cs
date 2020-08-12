using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{

    public class PlayerController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit item in hits)
            {
                CombatTarget combatTarget = item.transform.GetComponent<CombatTarget>();
                if (combatTarget == null)
                    continue;
                
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(combatTarget);
                    return true;
                }
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            if (Input.GetMouseButton(0))
            {
                return MoveToCursor();
            }
            return false;
        }

        private bool MoveToCursor()
        {
            Ray ray = GetMouseRay();
            bool hasHit = Physics.Raycast(ray, out RaycastHit hit);
            Debug.DrawRay(ray.origin, ray.direction * 100);
            if (hasHit)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Mover>().MoveTo(hit.point);
                    return true;
                }
            }
            return false;

        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}