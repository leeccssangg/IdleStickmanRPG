
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;


// =================================	
// Classes.
// =================================

//[ExecuteInEditMode]
[System.Serializable]

public class ParticleSystems : MonoBehaviour {
    // =================================	
    // Nested classes and structures.
    // =================================



    // =================================	
    // Variables.
    // =================================

    public ParticleSystem.MinMaxCurve[] m_PrimeScales;
    public ParticleSystem[] particleSystems { get; set; }

    // Event delegates.

    public delegate void onParticleSystemsDeadEventHandler();
    public event onParticleSystemsDeadEventHandler onParticleSystemsDeadEvent;

    // =================================	
    // Functions.
    // =================================

    // ...

    protected virtual void Awake() {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        SavePrimeScale();
    }

    // ...

    protected virtual void Start() {

    }

    // ...

    protected virtual void Update() {

    }

    // ...

    protected virtual void LateUpdate() {
        if (!isAlive()) {
            if (onParticleSystemsDeadEvent != null) {
                onParticleSystemsDeadEvent();
            }
        }
    }

    // ...

    public void play() {
        for (int i = 0; i < particleSystems.Length; i++) {
            particleSystems[i].Play(false);
        }
    }

    // ...

    public void pause() {
        for (int i = 0; i < particleSystems.Length; i++) {
            particleSystems[i].Pause();
        }
    }

    // ...

    public void stop() {
        if (particleSystems == null) return;
        for (int i = 0; i < particleSystems.Length; i++) {
            particleSystems[i].Stop(false);
        }
    }

    // ...

    public void clear() {
        for (int i = 0; i < particleSystems.Length; i++) {
            particleSystems[i].Clear();
        }
    }

    // ...

    public void setLoop(bool loop) {
        for (int i = 0; i < particleSystems.Length; i++) {
            ParticleSystem ps = particleSystems[i];
            ParticleSystem.MainModule psm = ps.main;
            psm.loop = loop;
        }
    }

    // ...

    public void setPlaybackSpeed(float speed) {
        for (int i = 0; i < particleSystems.Length; i++) {
            ParticleSystem ps = particleSystems[i];
            ParticleSystem.MainModule psm = ps.main;
            psm.simulationSpeed = speed;
        }
    }

    // ...

    public bool isAlive() {
        for (int i = 0; i < particleSystems.Length; i++) {
            if (particleSystems[i]) {
                if (particleSystems[i].IsAlive()) {
                    return true;
                }
            }
        }

        return false;
    }

    // ...

    public bool isPlaying(bool checkAll = false) {
        if (particleSystems.Length == 0) {
            return false;
        } else if (!checkAll) {
            return particleSystems[0].isPlaying;
        } else {
            for (int i = 0; i < 0; i++) {
                if (!particleSystems[i].isPlaying) {
                    return false;
                }
            }

            return true;
        }
    }

    // ...

    public int getParticleCount() {
        int pcount = 0;

        for (int i = 0; i < particleSystems.Length; i++) {
            if (particleSystems[i]) {
                pcount += particleSystems[i].particleCount;
            }
        }

        return pcount;
    }

    public void scaleParticle(float scale) {
        for (int i = 0; i < particleSystems.Length; i++) {
            ParticleSystem ps = particleSystems[i];
            if (ps) {
                ParticleSystem.MainModule mm = ps.main;
                ParticleSystem.MinMaxCurve mmc = mm.startSize;
                mmc.constant = m_PrimeScales[i].constant * scale;
                mmc.constantMax = m_PrimeScales[i].constantMax * scale;
                mmc.constantMin = m_PrimeScales[i].constantMin * scale;
                mmc.curveMultiplier = m_PrimeScales[i].curveMultiplier * scale;
                mm.startSize = mmc;
            }
        }
    }
    private void SavePrimeScale() {
        m_PrimeScales = new ParticleSystem.MinMaxCurve[particleSystems.Length];
        for (int i = 0; i < particleSystems.Length; i++) {
            ParticleSystem ps = particleSystems[i];
            if (ps) {
                ParticleSystem.MinMaxCurve saved = new ParticleSystem.MinMaxCurve();
                ParticleSystem.MainModule mm = ps.main;
                ParticleSystem.MinMaxCurve mmc = mm.startSize;
                saved.constant = mmc.constant;
                saved.constantMax = mmc.constantMax;
                saved.constantMin = mmc.constantMin;
                saved.curveMultiplier = mmc.curveMultiplier;
                saved.curve = mmc.curve;
                saved.mode = mmc.mode;
                m_PrimeScales[i] = saved;
            }
        }
    }
    // =================================	
    // End functions.
    // =================================

}

// =================================	
// End namespace.
// =================================