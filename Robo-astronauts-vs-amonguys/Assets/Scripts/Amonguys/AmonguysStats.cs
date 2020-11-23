using System.Collections;
namespace Amonguys
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "NewAmonguyStat", menuName = "Amonguys/Stats", order = 0)]
    public class AmonguysStats : ScriptableObject
    {
        public Color amonguy_color;
        public int damage = 10;
        public float speed = 4;
        public int health = 80;
        
        [Range(1f, 2.5f)] public float min_anim_speed = 1f;
        [Range(1f, 2.5f)] public float max_anim_speed = 2f;

        [Range(0.1f, 2f)] public float min_search_time = 0.25f;
        [Range(0.1f, 2f)] public float max_search_time = 1f;

        private void OnValidate()
        {
            min_anim_speed = Mathf.Max(0, min_anim_speed);
            max_anim_speed = Mathf.Max(min_anim_speed, max_anim_speed);

            min_search_time = Mathf.Max(0.1f, min_search_time);
            max_search_time = Mathf.Max(min_search_time, max_search_time);
        }

        public float GetAnimationSpeed()
        {
            return Random.Range(min_anim_speed, max_anim_speed);
        }

        public float GetSearchTime()
        {
            return Random.Range(min_search_time, max_search_time);
        }

    }
}