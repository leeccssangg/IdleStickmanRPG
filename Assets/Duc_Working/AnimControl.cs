using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;

public class AnimControl : MonoBehaviour
{
    #region Inspector
    // [SpineAnimation] attribute allows an Inspector dropdown of Spine animation names coming form SkeletonAnimation.
    [SpineAnimation]
    public string runAnimationName;

    [SpineAnimation]
    public string idleAnimationName;

    [SpineAnimation]
    public string atkAnimationName_1;

    [SpineAnimation]
    public string hitAnimationName;

    [SpineAnimation]
    public string deathAnimationName;

    [SpineAnimation]
    public string stunAnimationName;

    [SpineAnimation]
    public string skillAnimationName_1;

    #endregion

    SkeletonAnimation skeletonAnimation;

    // Spine.AnimationState and Spine.Skeleton are not Unity-serialized objects. You will not see them as fields in the inspector.
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    // Start is called before the first frame update
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            spineAnimationState.SetAnimation(0, atkAnimationName_1, false);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            spineAnimationState.SetAnimation(0, skillAnimationName_1, false);
        }
    }

    public void running()
    {
        spineAnimationState.SetAnimation(0, runAnimationName, true);
    }
    public void idle()
    {
        spineAnimationState.SetAnimation(0, idleAnimationName, true);
    }
    public void getHit()
    {
        spineAnimationState.SetAnimation(0, hitAnimationName, true);
    }
    public void death()
    {
        spineAnimationState.SetAnimation(0, deathAnimationName, true);
    }
    public void stun()
    {
        spineAnimationState.SetAnimation(0, stunAnimationName, true);
    }
    public void attack_1()
    {
        spineAnimationState.SetAnimation(0, atkAnimationName_1, false);
    }
    public void skill_1()
    {
        spineAnimationState.SetAnimation(0, skillAnimationName_1, false);
    }

}
