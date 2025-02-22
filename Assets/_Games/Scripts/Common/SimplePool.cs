﻿
using UnityEngine;
using System.Collections.Generic;

public static class SimplePool
{

    // You can avoid resizing of the Stack's internal array by
    // setting this to a number equal to or greater to what you
    // expect most of your pool sizes to be.
    // Note, you can also use Preload() to set the initial size
    // of a pool -- this can be handy if only some of your pools
    // are going to be exceptionally large (for example, your bullets.)
    const int DEFAULT_POOL_SIZE = 3;

    static Transform m_sRoot = null;

    /// <summary>
    /// The Pool class represents the pool for a particular prefab.
    /// </summary>
    class Pool
    {
        // We append an id to the name of anything we instantiate.
        // This is purely cosmetic.
        int nextId = 1;

        // The structure containing our inactive objects.
        // Why a Stack and not a List? Because we'll never need to
        // pluck an object from the start or middle of the array.
        // We'll always just grab the last one, which eliminates
        // any need to shuffle the objects around in memory.
        Stack<GameObject> inactive;

        // The prefab that we are pooling
        GameObject prefab;

        // Constructor
        public Pool(GameObject prefab, int initialQty)
        {
            this.prefab = prefab;

            // If Stack uses a linked list internally, then this
            // whole initialQty thing is a placebo that we could
            // strip out for more minimal code.
            inactive = new Stack<GameObject>(initialQty);
        }
        public int Count {
            get { return inactive.Count;}
        }
        // Spawn an object from our pool
        public GameObject Spawn(Vector3 pos, Quaternion rot, string name)
        {
            GameObject obj;
            if (inactive.Count == 0)
            {
                // We don't have an object in our pool, so we
                // instantiate a whole new object.
                obj = (GameObject)GameObject.Instantiate(prefab, pos, rot);
                obj.name = prefab.name + " (" + (nextId++) + ")";

                // Add a PoolMember component so we know what pool
                // we belong to.
                obj.AddComponent<PoolMember>().myPool = this;
                //obj.GetComponent<PoolMember>().name = name;
            }
            else
            {
                // Grab the last object in the inactive array
                obj = inactive.Pop();

                if (obj == null)
                {
                    // The inactive object we expected to find no longer exists.
                    // The most likely causes are:
                    //   - Someone calling Destroy() on our object
                    //   - A scene change (which will destroy all our objects).
                    //     NOTE: This could be prevented with a DontDestroyOnLoad
                    //	   if you really don't want this.
                    // No worries -- we'll just try the next one in our sequence.

                    return Spawn(pos, rot, name);
                }
            }

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);
            return obj;

        }
        public T Spawn<T>( Vector3 pos, Quaternion rot,string name) {
            return Spawn(pos,rot,name).GetComponent<T>();
        }
        // Return an object to the inactive pool.
        public void Despawn(GameObject obj)
        {
            obj.SetActive(false);
            if (inactive.Contains(obj)) {
                return;
            }
            if (inactive.Count > 0)
            {
                GameObject old = inactive.Pop();
                if (old == obj)
                {
                    ////Debug.Log("AHA ");
                }
                inactive.Push(old);
            }
            // Since Stack doesn't have a Capacity member, we can't control
            // the growth factor if it does have to expand an internal array.
            // On the other hand, it might simply be using a linked list 
            // internally.  But then, why does it allow us to specificy a size
            // in the constructor? Stack is weird.
            inactive.Push(obj);
            ////Debug.Log("INACTIVE: " + inactive.Count);
        }
        public void Clamp(int amount) {
            while(inactive.Count> amount) {
                GameObject go = inactive.Pop();
                GameObject.Destroy(go);
            }
        }
        public void Release() {
            while(inactive.Count>0) {
                GameObject go = inactive.Pop();
                GameObject.Destroy(go);
            }
            inactive.Clear();
        }
    }


    /// <summary>
    /// Added to freshly instantiated objects, so we can link back
    /// to the correct pool on despawn.
    /// </summary>
    class PoolMember : MonoBehaviour
    {
        public Pool myPool;        
    }

    // All of our pools
    static Dictionary<string, Pool> pools;

    /// <summary>
    /// Init our dictionary.
    /// </summary>
    static void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE, string name = "")
    {
        if (pools == null)
        {
            pools = new Dictionary<string, Pool>();
        }
        if (prefab != null && !pools.ContainsKey(name))
        {
            pools[name] = new Pool(prefab, qty);
        }
    }

    /// <summary>
    /// If you want to preload a few copies of an object at the start
    /// of a scene, you can use this. Really not needed unless you're
    /// going to go from zero instances to 10+ very quickly.
    /// Could technically be optimized more, but in practice the
    /// Spawn/Despawn sequence is going to be pretty darn quick and
    /// this avoids code duplication.
    /// </summary>
    static public void Preload(GameObject prefab, int qty = 1, string name = "default")
    {
        Init(prefab, qty, name);

        // Make an array to grab the objects we're about to pre-spawn.
        GameObject[] obs = new GameObject[qty];
        for (int i = 0; i < qty; i++)
        {
            obs[i] = Spawn(name, Vector3.zero, Quaternion.identity);
            obs[i].transform.SetParent(GetRootGameObject());
        }

        // Now despawn them all.
        for (int i = 0; i < qty; i++)
        {
            Despawn(obs[i]);
        }
    }
    static public int GetPoolingObjectCount(string name) {
        Pool _pool = null;
        if(pools.TryGetValue(name,out _pool)) {
            return _pool.Count;
        }
        return 0;
    }

    static public void ClampPools(string name, int amount) {
        Pool _pool = null;
        if(pools.TryGetValue(name,out _pool)) {
            _pool.Clamp(amount);
        }
    }

    /// <summary>
    /// Spawns a copy of the specified prefab (instantiating one if required).
    /// NOTE: Remember that Awake() or Start() will only run on the very first
    /// spawn and that member variables won't get reset.  OnEnable will run
    /// after spawning -- but remember that toggling IsActive will also
    /// call that function.
    /// </summary>
    static public GameObject Spawn(string name, Vector3 pos, Quaternion rot)
    {
        //Init(prefab);        
        GameObject obj = pools[name].Spawn(pos, rot, name);
        //if (name == "Tile") {
        //    obj.GetComponent<IngameObject>().Reset();
        //}
        return obj;
    }
    static public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot) {
        if (!IsHasPool(prefab.name)) {
            Preload(prefab, 2, prefab.name);
        }
        return Spawn(prefab.name,pos,rot);
    }
    static public GameObject Spawn( GameObject prefab, Vector3 pos ) {
        if (!IsHasPool(prefab.name)) {
            Preload(prefab, 2, prefab.name);
        }
        return Spawn(prefab.name, pos, Quaternion.identity);
    }
    static public GameObject Spawn( GameObject prefab ) {
        if (!IsHasPool(prefab.name)) {
            Preload(prefab, 2, prefab.name);
        }
        return Spawn(prefab.name, Vector3.zero, Quaternion.identity);
    }
    static public bool IsHasPool(string name) {
        if(pools == null) return false;
        return pools.ContainsKey(name);
    }

    /// <summary>
    /// Despawn the specified gameobject back into its pool.
    /// </summary>
    static public void Despawn(GameObject obj)
    {
        PoolMember pm = obj.GetComponent<PoolMember>();
        if (pm == null)
        {
            //Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
            GameObject.Destroy(obj);
        }
        else {
            pm.myPool.Despawn(obj);
            obj.transform.SetParent(GetRootGameObject());
        }
        //obj.transform.SetParent(GetRootGameObject());
    }

    static public void Release()
    {
        if(pools== null || pools.Count==0)return;
        foreach (KeyValuePair < string, Pool> kvp in pools)
        {
            //Do some interesting things;            
            kvp.Value.Release();
        }
        pools.Clear();
    }
    static public void Release(string name) {
        if(pools.ContainsKey(name)) {
            foreach(KeyValuePair<string, Pool> kvp in pools) {
                if(kvp.Key == name) {
                    kvp.Value.Release();
                }
            }
            pools.Remove(name); 
        }
    }
    static Transform GetRootGameObject() {
        if (m_sRoot == null) {
            GameObject go = new GameObject();
            go.name = "PoolingRoot";
            m_sRoot = go.transform;
        }
        return m_sRoot;
    }
}