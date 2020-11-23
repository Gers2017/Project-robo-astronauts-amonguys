using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPooler : MonoBehaviour
    {
        [Header("Pool settings")]
        [SerializeField] List<GameObject> object_pool = new List<GameObject>();
        public GameObject object_prefab;
        public int start_pool_instances = 5;
        [SerializeField] bool self_parent = false;
        Transform parent;

        private void Awake()
        {
            if(self_parent)
            {
                parent = transform;
            }
            else
            {
                var p = new GameObject(object_prefab.name + " container");
                parent = p.transform;
            }

            for (int i = 0; i < start_pool_instances; i++)
            {
                CreateInstance(false);
            }
        }
        protected GameObject CreateInstance(bool create_active = true, Vector3? position = null, 
        Quaternion? rotation = null)
        {
            GameObject instance = Instantiate(object_prefab, transform.position, 
            rotation ?? Quaternion.identity);
            instance.transform.position = position ?? transform.position;
            instance.transform.SetParent(parent);
            instance.SetActive(create_active);
            object_pool.Add(instance);
            return instance;
        }

        public GameObject GetInstance(Vector3? position = null)
        {
            GameObject instace = null;
            for (int i = 0; i < object_pool.Count; i++)
            {
                var current = object_pool[i];
                if(!current.activeInHierarchy)
                {
                    instace = current;
                }
            }

            //If there's no instance create one
            if(instace == null)
            {
                instace = CreateInstance(true, position);
            }
            else
            {
                instace.transform.position = position ?? transform.position;
                instace.SetActive(true);
            }

            IPoolObject pool_object;
            bool exist = instace.TryGetComponent<IPoolObject>(out pool_object);
            if(exist) pool_object.OnActivation();

            return instace;
        }

        public int GetActiveInstances()
        {
            return object_pool.Count((go) => { return go.activeInHierarchy; });
        }
    }
}