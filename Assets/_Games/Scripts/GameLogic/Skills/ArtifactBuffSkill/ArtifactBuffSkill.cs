using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactBuffSkill : MonoBehaviour
{
    [SerializeField] private int m_ArtifactID;
    public BigNumber GetBuff()
    {
        Artifact artifact = ArtifactsManager.Ins.GetArtifactEquipped(m_ArtifactID);
        if (artifact == null) return 0;
        if (artifact.GetArtifactId() != m_ArtifactID) return 0;
        Debug.Log($"ArtifactBuffSkill artifact - {artifact.GetArtifactId()} - {artifact.GetSkillValue()}");
        return artifact?.GetSkillValue() ?? 0;
    }
}
