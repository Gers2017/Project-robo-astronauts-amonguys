using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Collectables
{
    public class CollectableManager : MonoBehaviour
    {
        List<Collectable> collectablesList = new List<Collectable>();
        [SerializeField] private TMP_Text collectedText;
        public UnityEvent OnAllCollected;
        int startCount;

        void Start()
        {
            collectablesList = FindObjectsOfType<Collectable>().ToList();
            startCount = collectablesList.Count;

            foreach (var collectable in collectablesList)
            {
                collectable.OnCollected += HandlePickup;
            }

            UpdateText();
        }

        void HandlePickup(Collectable collected)
        {
            collectablesList.Remove(collected);
            collected.OnCollected -= HandlePickup;

            if(collectablesList.Count == 0) OnAllCollected.Invoke();

            UpdateText();
        }

        void UpdateText()
        {
            collectedText.SetText($"{startCount - collectablesList.Count} / {startCount}");
        }
    }
}