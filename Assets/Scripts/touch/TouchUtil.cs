using System;

using UnityEngine;

namespace stinkey.touch {
  public static class TouchUtil {
    private const float STANDARD_RESOLUTION_DIAGONAL = 758f;
    private const float VIOLENT_MAGNITUDE = 2000;

    public static float GetScreenSizeAdjustmentFactor() {
      var resolution = Camera.current.pixelRect;
      var resolutionDiagonal =
          new Vector2(resolution.width, resolution.height).magnitude;

      var screenSizeAdjustmentFactor =
          TouchUtil.STANDARD_RESOLUTION_DIAGONAL / resolutionDiagonal;

      return screenSizeAdjustmentFactor;
    }

    public static Vector2 GetScreenNormalizedSwipe(Touch touch) {
      return touch.deltaPosition * TouchUtil.GetScreenSizeAdjustmentFactor() /
             touch.deltaTime;
    }

    public static float GetSwipeIntensityMagnitude(Touch touch) {
      return TouchUtil.GetScreenNormalizedSwipe(touch).magnitude /
             TouchUtil.VIOLENT_MAGNITUDE;
    }

    public static Vector2 GetSwipeIntensityVector(Touch touch) {
      var screenNormalizedSwipe = TouchUtil.GetScreenNormalizedSwipe(touch);

      var normalizedSwipe = screenNormalizedSwipe.normalized;
      var swipeIntensity =
          screenNormalizedSwipe.magnitude / TouchUtil.VIOLENT_MAGNITUDE;

      return normalizedSwipe * swipeIntensity;
    }

    public static bool IsSwipeViolent(Touch touch) =>
        TouchUtil.GetSwipeIntensityMagnitude(touch) > 1;
  }
}