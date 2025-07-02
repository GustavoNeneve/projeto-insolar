using System.Collections.Generic;
using UnityEngine;

namespace EditorDeNiveis {
    // Singleton manager for pooling objects
    public class ObjectPoolManager : MonoBehaviour {
        private static ObjectPoolManager _instance;
        public static ObjectPoolManager Instance {
            get {
                if (_instance == null) {
                    var obj = new GameObject("ObjectPoolManager");
                    _instance = obj.AddComponent<ObjectPoolManager>();
                    DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }

        // Pool per prefab
        private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

        // Get an object from pool or instantiate new
        public GameObject GetObject(GameObject prefab) {
            if (!pools.ContainsKey(prefab)) {
                pools[prefab] = new Queue<GameObject>();
            }
            var queue = pools[prefab];
            if (queue.Count > 0) {
                var obj = queue.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            return Instantiate(prefab);
        }

        // Return object to pool
        public void ReturnObject(GameObject obj, GameObject prefab) {
            obj.SetActive(false);
            if (!pools.ContainsKey(prefab)) {
                pools[prefab] = new Queue<GameObject>();
            }
            pools[prefab].Enqueue(obj);
        }
    }
}
