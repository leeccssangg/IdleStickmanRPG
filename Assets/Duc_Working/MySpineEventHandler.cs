// Sample written for for Spine 3.7
using UnityEngine;
using Spine;
using Spine.Unity;

// Add this to the same GameObject as your SkeletonAnimation
public class MySpineEventHandler : MonoBehaviour {
    public GameObject attack_fx;
    public GameObject skill_fx;
   // The [SpineEvent] attribute makes the inspector for this MonoBehaviour
   // draw the field as a dropdown list of existing event names in your SkeletonData.
   [SpineEvent] public string attackEventName = "Hit"; 
   [SpineEvent] public string skillEventName = "Skill"; 

   void Start () {
      var skeletonAnimation = GetComponent<SkeletonAnimation>();
      if (skeletonAnimation == null) return;

      // This is how you subscribe via a declared method.
      // The method needs the correct signature.
      skeletonAnimation.AnimationState.Event += HandleEvent;

      skeletonAnimation.AnimationState.Start += delegate (TrackEntry trackEntry) {
         // You can also use an anonymous delegate.
         Debug.Log(string.Format("track {0} started a new animation.", trackEntry.TrackIndex));
      };

      skeletonAnimation.AnimationState.End += delegate {
         // ... or choose to ignore its parameters.
         Debug.Log("An animation ended!");
      };
   }

    void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        // Play some sound if the event named "footstep" fired.
        if (e.Data.Name == attackEventName)
        {
            //Debug.Log("attack_fx");
            Instantiate(attack_fx, this.transform.position, this.transform.rotation);
        }
        if (e.Data.Name == skillEventName)
        {
            //Debug.Log("skill_fx");
            Instantiate(skill_fx, this.transform.position, this.transform.rotation);
        }
    }
}