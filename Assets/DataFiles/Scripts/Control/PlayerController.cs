using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Resources;

namespace RPG.Control
{

    public class PlayerController : MonoBehaviour
    {
        public float fractionSpeed = 1;

        public enum CursorType
        {
            None,
            Movement,
            Combat
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;

        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        // Update is called once per frame
        void Update()
        {
            if (InteractWithCursorClick())
                return;
            SetCursor(CursorType.None);
            /*else if (InteractWithMovement())
                return;
            */

        }

        private bool InteractWithCursorClick()
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
                            
                        if (Input.GetMouseButton(0))
                        {
                            GetComponent<Fighter>().Attack(combatTarget.gameObject.GetComponent<Health>());
                            //GetComponent<Mover>().StartAction(combatTarget.transform.position);
                                
                        }
                        SetCursor(CursorType.Combat);
                        return true;
                    }
                }
                if (Input.GetMouseButton(0))
                    GetComponent<Mover>().StartAction(hits[0].point,fractionSpeed);

            }
            
            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping cursorMapping = GetCursorMapping(type);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping item in cursorMappings)
            {
                if(item.type == type)
                {
                    return item;
                }
            }
            return cursorMappings[0];
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