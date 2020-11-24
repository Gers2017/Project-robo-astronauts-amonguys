using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public class CursorManager : MonoBehaviour
    {
        private void Start()
        {
            LockCursor();
        }
        
        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void FreeCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}