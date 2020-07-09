using UnityEngine;

namespace stinkeys.button {
  public delegate void OnTouchHandler(Touch touch);

  [RequireComponent(typeof(Collider))]
  public class TouchButton : MonoBehaviour, IButton {
    public bool Disabled { get; set; }

    private enum InternalTouchState {
      NOT_TOUCHED,
      NEWLY_TOUCHED,
      TOUCHED,
      POSSIBLY_NO_LONGER_TOUCHED,
    }

    private InternalTouchState touchState_;

    public event OnTouchHandler OnTouch = delegate {};
    public event OnPressHandler OnPress = delegate {};
    public event OnReleaseHandler OnRelease = delegate {};

    /**
     * Don't call this manually, this will be called by the
     * TouchButtonDetector.
     */
    public void MarkTouched(Touch touch) {
      this.OnTouch(touch);
      if (this.touchState_ == InternalTouchState.NOT_TOUCHED ||
          this.touchState_ == InternalTouchState.NEWLY_TOUCHED) {
        this.touchState_ = InternalTouchState.NEWLY_TOUCHED;
      }
      else {
        this.touchState_ = InternalTouchState.TOUCHED;
      }
    }

    public void LateUpdate() {
      if (this.Disabled) {
        this.touchState_ = InternalTouchState.NOT_TOUCHED;
        return;
      }

      if (this.touchState_ == InternalTouchState.POSSIBLY_NO_LONGER_TOUCHED) {
        this.touchState_ = InternalTouchState.NOT_TOUCHED;
        this.OnRelease();
      }

      var isNewlyTouched = this.touchState_ == InternalTouchState.NEWLY_TOUCHED;
      if (isNewlyTouched) {
        this.OnPress();
      }

      if (isNewlyTouched || this.touchState_ == InternalTouchState.TOUCHED) {
        this.touchState_ = InternalTouchState.POSSIBLY_NO_LONGER_TOUCHED;
      }
    }
  }
}