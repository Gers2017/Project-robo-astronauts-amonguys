using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Amonguys
{
    public class AmonguysDark : Amonguys
    {
        [SerializeField] float distance_to_sneak = 5f;
        [SerializeField] float target_angle_visible_limit = 90;
        [SerializeField] protected float start_speed;
        [SerializeField] protected float sneaky_speed = 12f;

        public override void OnActivation()
        {
            base.OnActivation();
            start_speed = agent.speed;
        }
        
        protected override void OnAmonguyAlive()
        {
            base.OnAmonguyAlive();

            //Cheat to be speed. I'm speed
            if(TargetCanSeeMe() && distance_to_target >= distance_to_sneak)
            {
                agent.speed = start_speed;
            }
            else
            {
                agent.speed = sneaky_speed;
            }
        }

        protected bool TargetCanSeeMe()
        {
            if(target == null) return false;

            Vector3 targetLookDir = target.forward;
            Vector3 diff = (transform.position - target.position);
            Vector3 dirToMe = new Vector3(diff.x, 0f, diff.z).normalized;

            float dot = Vector3.Dot(targetLookDir, dirToMe);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            return angle <= target_angle_visible_limit;            

        }
    }
}
