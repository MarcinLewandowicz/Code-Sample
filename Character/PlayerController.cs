using ML.Character;
using ML.Combat;
using ML.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ML.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Health playerHealth;

        [System.Serializable]
        private struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField]
        CursorMapping[] cursorMappings = null;

        private void Awake()
        {
            playerHealth = GetComponent<Health>();
        }
        private void Update()
        {
            if (InteractWithUI()) { return; }
            if(playerHealth.IsDead()) 
            {
                SetCursor(CursorType.None);
                return; }
            if (Input.GetMouseButtonDown(1))
            {
                GetComponent<Inventory>().SwitchWeapons();
            }
            if (InteractWithRaycastTarget()) { return; }
            if (InteractWithMovement()) { return; }
            SetCursor(CursorType.None);
        }

        private bool InteractWithRaycastTarget()
        {

            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IClickable[] targets = hit.transform.GetComponents<IClickable>();
                foreach(IClickable target in targets)
                {
                    if(target == GetComponent<Health>()) { return false; }
                    if (target.InteractWithRaycast(this))
                    {
                        SetCursor(target.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InteractWithMovement()
        {            
            RaycastHit rayHit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out rayHit);
            if (hasHit)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Mover>().StartMovement(rayHit.point);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping cursor in cursorMappings)
            {
                if (cursor.cursorType == type)
                {
                    return cursor;
                }
            }
            return cursorMappings[0];
        }
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public static float ScrollWheel { get { return Input.mouseScrollDelta.y / 10; } }
    }

}