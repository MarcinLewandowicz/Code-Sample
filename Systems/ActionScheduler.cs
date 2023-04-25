using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.Systems
{
    public class ActionScheduler : MonoBehaviour
    {
        private IActionable currentAction;

        public void SetAction(IActionable action)
        {
            if(currentAction == action) { return; }
            if (currentAction != null)
            {
                currentAction.StopAction();
            }
            currentAction = action;
        }
    }



}