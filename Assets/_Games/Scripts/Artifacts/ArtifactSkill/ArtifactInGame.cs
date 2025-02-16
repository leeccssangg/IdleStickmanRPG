using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactInGame : MonoBehaviour
{
    [SerializeField] private int m_Id;
    [SerializeField] protected Artifact m_Artifact;

    public int Id => m_Id;

    public virtual void OnInit(Artifact artifact)
    {
        m_Artifact = artifact;
    }
    public virtual void OnUpdateArtifact()
    {
        
    }
    public virtual void OnDespawn()
    {
        
    }

    public virtual void OnRelease()
    {
        OnDespawn();
        Destroy(gameObject);
    }
}
