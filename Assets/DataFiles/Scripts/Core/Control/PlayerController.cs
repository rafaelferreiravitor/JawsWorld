﻿using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Attributes;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{

    public class PlayerController : MonoBehaviour
    {
        public float fractionSpeed = 1;


        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;

        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float maxPathLength = 40f;
        [SerializeField] float raycastRadius = 1f;

        bool UIAction = false;

        // Update is called once per frame
        void Update()
        {
            if (InteractWithComponent()) return;
            if (InteractWithUI()) return;

            //if (InteractWithCursorClick())
            //    return;
            if(!UIAction)
                if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
            /*else if (InteractWithMovement())
                return;
            */

        }

        private bool InteractWithMovement()
        {
            //RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartAction(target, fractionSpeed);
                    
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;
            NavMeshHit navMeshHit;
            bool hasCastToNavmesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, maxNavMeshProjectionDistance,NavMesh.AllAreas);
            if (!hasCastToNavmesh) return false;
            target = navMeshHit.position;

            return true;
        }


        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];
            for(int i = 0; i< hits.Length; i++)
                distances[i] = hits[i].distance;
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
                UIAction = false;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                if (Input.GetMouseButton(0))
                    UIAction = true;
                return true;
            }

            if (UIAction)
                return true;

            return false;
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