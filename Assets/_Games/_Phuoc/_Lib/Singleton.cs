﻿using System;
using UnityEngine;

namespace Pextension
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_Ins;

        public static T Ins
        {
            get
            {
                if (m_Ins == null)
                {
                    // Find singleton
                    m_Ins = FindObjectOfType<T>();

                    // Create new instance if one doesn't already exist.
                    if (m_Ins == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        GameObject singletonObject = new GameObject();
                        m_Ins = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                    }

                }
                return m_Ins;
            }
        }
    }
}

