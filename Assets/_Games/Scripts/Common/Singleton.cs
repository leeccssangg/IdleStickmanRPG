using UnityEngine;

/**<summary> Singleton class which is not MonoBehavior</summary> */
public class Singleton<T> where T : new() {
    protected static T instance;
    private bool initialized = false;

    public static T Ins {
        get {
            if (instance != null)
                return instance;
            instance = new T();
            (instance as Singleton<T>).Initialize();
            return instance;
        }
    }

    protected virtual void Initialize() {
        if (initialized)
            return;
        initialized = true;
    }

    /**<summary> Call T.Instance.Preload() at the first script startup to pre init. </summary>*/
    public virtual void Preload() { }
}

/** <summary> Base Singleton class which is MonoBehavior </summary> */
public abstract class SingletonMono<T> : MonoBehaviour where T : Component {
    protected static T instance;

    protected virtual void Awake() {
        if (instance == null)
            instance = this as T;
        else if (this != instance) {
            Debug.LogWarningFormat("[MonoSingleton] Class {0} is initialized multiple times", typeof(T).FullName);
            DestroyImmediate(gameObject);
            return;
        }

        OnAwake();
    }

    protected abstract void OnAwake();

    public virtual void PreLoad() { }

    protected virtual void OnDestroy() {
        instance = null;
    }
}

/** <summary> 
 * <para> "Instance" = Find object in scene. </para>
 * <para> Must be added to scene before run </para>
 * </summary> */
public class SingletonBind<T> : SingletonMono<T> where T : Component {
    public static T Instance {
        get {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<T>();
            if (instance == null) {
                Debug.LogErrorFormat("[Singleton] Class {0} must be added to scene before run!", typeof(T));
            }
            return instance;
        }
    }

    protected override void OnAwake() { }
}

/** <summary> 
 * <para> "Instance" = Find object in scene. </para>
 * <para> Must be added to scene before run </para>
 * <para> Instance is DontDestroyOnLoad </para>
 * </summary> */
public class SingletonBindAlive<T> : SingletonBind<T> where T : Component {
    protected override void Awake() {
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }
}

/** <summary> 
 * <para>"Instance" = new GameObject if can not find it on scene. </para>
 * <para> No scene reference variables. </para>
 * </summary> */
public class SingletonFree<T> : SingletonMono<T> where T : Component {
    public static T Ins {
        get {
            if (instance != null)
                return instance;
            instance = (T)FindObjectOfType(typeof(T));
            if (instance == null) {
                Debug.LogWarningFormat("[Singleton] Class {0} not found! Create empty instance", typeof(T));
                instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }
            return instance;
        }
    }

    protected override void OnAwake() { }
}

/** <summary> 
 * <para> "Instance" = new GameObject if can not find it on scene. </para>
 * <para> No scene reference variables. </para>
 * <para> Instance is DontDestroyOnLoad </para>
 * </summary> */
public class SingletonFreeAlive<T> : SingletonFree<T> where T : MonoBehaviour {
    protected override void Awake() {
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }
}

/** <summary> 
 * <para> "Instance" = Instantiate from Resources folder when be called at runtime.</para>
 * <para> Place prefab on path: "Resources/Prefabs/T/T", T is the name of class </para>
 * </summary> */
public class SingletonResources<T> : SingletonMono<T> where T : Component {
    protected static string PrefabPath {
        get { return string.Format("Prefabs/{0}/{1}", typeof(T).Name, typeof(T).Name); }
    }

    public static T Instance {
        get {
            if (instance != null)
                return instance;
            instance = Instantiate(Resources.Load<GameObject>(PrefabPath)).GetComponent<T>();
            if (instance == null)
                Debug.LogErrorFormat("[{0}] Wrong resources path: {1}!", typeof(T).Name, PrefabPath);
            return instance;
        }
    }

    protected override void OnAwake() { }
}

/** <summary>
 * <para> "Instance" = Instantiate from Resources folder when be called at runtime. </para>
 * <para> Place prefab on path: "Resources/Prefabs/T/T", T is the name of class </para>
 * <para> Instance is DontDestroyOnLoad</para>
 * </summary> */
public class SingletonResourcesAlive<T> : SingletonResources<T> where T : Component {
    protected override void Awake() {
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }
}
