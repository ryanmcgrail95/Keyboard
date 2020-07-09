using System.Numerics;

using Vector3 = UnityEngine.Vector3;

namespace stinkeys {
  public abstract class BSoftDestructibleButton : BDestructibleButton {

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
