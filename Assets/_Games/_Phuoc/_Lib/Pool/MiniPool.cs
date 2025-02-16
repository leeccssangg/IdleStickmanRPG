using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pextension
{
    [System.Serializable]
    public class MiniPool<T> where T : Component
    {
        [Header("Check Pool")]
        [ShowInInspector, ReadOnly] Queue<T> pools = new Queue<T>();
        [SerializeField, ReadOnly] List<T> listActives = new List<T>();
        [Header("Init Pool")]
        [SerializeField] Transform parent;
        [SerializeField] T prefab;
        [SerializeField] int amount = 1;
        public List<T> Actives => listActives;

        public MiniPool()
        {
            OnInit(prefab, amount, parent);
        }

        public MiniPool(T prefab, int amount, Transform parent = null)
        {
            OnInit(prefab, amount, parent);
        }

        public void OnInit(T prefab, int amount, Transform parent = null)
        {
            if (prefab != null)
            {
                this.prefab = prefab;
                this.parent = parent;

                for (int i = 0; i < amount; i++)
                {
                    Despawn(GameObject.Instantiate(prefab, parent));
                }
            }
        }

        public T Spawn(Vector3 pos, Quaternion rot)
        {
            T go = pools.Count > 0 ? pools.Dequeue() : GameObject.Instantiate(prefab, parent);

            listActives.Add(go);

            go.transform.SetPositionAndRotation(pos, rot);
            go.gameObject.SetActive(true);

            return go;
        }
        public T Spawn()
        {
            T go = pools.Count > 0 ? pools.Dequeue() : GameObject.Instantiate(prefab, parent);

            listActives.Add(go);
            go.gameObject.SetActive(true);

            return go;
        }

        public void Despawn(T obj)
        {
            if (obj.gameObject.activeSelf)
            {
                obj.gameObject.SetActive(false);
                pools.Enqueue(obj);
                listActives.Remove(obj);
            }
        }

        public void Collect()
        {
            while (listActives.Count > 0)
            {
                Despawn(listActives[0]);
            }
        }

        public void Collect(Action<T> action)
        {
            while (listActives.Count > 0)
            {
                action.Invoke(listActives[0]);
                Despawn(listActives[0]);
            }
        }

        public void Release()
        {
            Collect();

            while (pools.Count > 0)
            {
                GameObject.Destroy(pools.Dequeue().gameObject);
            }
        }

    }

}

