using UnityEngine;
using System.Collections.Generic;

public class Cache
{

    private static Dictionary<float, WaitForSeconds> m_WFS = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWFS(float key)
    {
        if(!m_WFS.ContainsKey(key))
        {
            m_WFS[key] = new WaitForSeconds(key);
        }

        return m_WFS[key];
    }

    //------------------------------------------------------------------------------------------------------------


    //private static Dictionary<Behaviour, Hero> m_Hero = new Dictionary<Behaviour, Hero>();

    //public static Hero GetHero(Behaviour key)
    //{
    //    if (!m_Hero.ContainsKey(key))
    //    {
    //        m_Hero.Add(key, key.GetComponent<Hero>());
    //    }

    //    return m_Hero[key];
    //}


    //private static Dictionary<Collider, Character> m_Char = new Dictionary<Collider, Character>();

    //public static Character GetCharacter(Collider key)
    //{
    //    if (!m_Char.ContainsKey(key))
    //    {
    //        m_Char.Add(key, key.GetComponent<Character>());
    //    }

    //    return m_Char[key];
    //}

    //public static bool GetHero(Collider key, out Hero hero)
    //{
    //    hero = null;

    //    if (!m_Char.ContainsKey(key))
    //    {
    //        hero = key.GetComponent<Character>() as Hero;
    //        m_Char.Add(key, hero);
    //    }

    //    return hero != null;
    //}

    //private static Dictionary<Collider, Character> m_Character = new Dictionary<Collider, Character>();

    //public static Character GetCharacter(Collider key)
    //{
    //    if (!m_Character.ContainsKey(key))
    //    {
    //        m_Character.Add(key, key.GetComponent<Character>());
    //    }

    //    return m_Character[key];
    //}


}
