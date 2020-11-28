using UnityEngine;

namespace Secrets
{
    [CreateAssetMenu(fileName = "SkinData", menuName = "Skin/SkinData", order = 1)]

    public class SkinData : ScriptableObject
    {
        public Color skinColor;
        public Color eyesColor;
        public Color eyesEmisionColor;
    }
}