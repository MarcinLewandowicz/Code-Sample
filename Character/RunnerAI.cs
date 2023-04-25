using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.Combat
{
    public class RunnerAI : Enemy
    {
        void Start()
        {
            Attack();
            SetExclamationMarkStatus(true);
        }
        protected override void RespawnBehaviour()
        {
            base.RespawnBehaviour();
            SetExclamationMarkStatus(true);
            Invoke(nameof(Attack), 1.0f);
        }
    }

}