using UnityEngine;

namespace stinkeys {
  public class PercussionButton : BDestructibleButton {
    [SerializeField] protected AudioClip sound;

    protected override void OnPress() {
      if (this.sound != null) {
        this.Play(this.sound);
      }
      else {
        base.OnPress();
      }
    }


    protected override void Move() {
      var targetedTransform = this.rendererWrapper.transform;

      moveFrac += (toMoveFrac - moveFrac) / 3;
      moveAmount = -moveFrac * 3;

      Vector3 pos = targetedTransform.localPosition;
      pos.y = .05f * moveAmount;

      targetedTransform.localPosition = pos;
    }
  }
}