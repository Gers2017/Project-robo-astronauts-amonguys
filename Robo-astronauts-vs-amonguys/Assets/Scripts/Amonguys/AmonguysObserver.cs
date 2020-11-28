using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Amonguys
{
    public class AmonguysObserver : MonoBehaviour
    {
        [SerializeField] private GameObject amonguyPrefab;
        private int dieCount;
        [SerializeField] private int limitDieCount = 10;
        [SerializeField] Transform spawnPoint;

        private void Awake()
        {
            Amonguys.OnAnyAmonguyDie += HandleOnAnyAmonguyDie;
        }

        private void HandleOnAnyAmonguyDie()
        {
            dieCount ++;

            if(dieCount > limitDieCount)
            {
                SpawnAmonguyDark();
                //Reset the die count
                dieCount = 0;
            }
        }

        public void SpawnAmonguyDark()
        {
            var obj = Instantiate(amonguyPrefab, spawnPoint.position, amonguyPrefab.transform.rotation);
            var amonguy = obj.GetComponent<Amonguys>();
            amonguy?.OnActivation();
        }

    }
}
