using stinkeys.button;

using UnityEngine;

namespace stinkeys {
  public class ExplodeButton : MonoBehaviour {
    private IButton buttonImpl_;

    private Piano piano_;
    private BDestructibleButton[] destructibleButtons_;

    public void Start() {
      this.buttonImpl_ = this.gameObject.AddComponent<TouchButton>();
      this.buttonImpl_.OnPress += this.OnPress_;

      this.piano_ = Object.FindObjectOfType<Piano>();
      this.destructibleButtons_ =
          Object.FindObjectsOfType<BDestructibleButton>();
    }

    private void OnPress_() {
      this.piano_.Explode();
      foreach (var destructibleButton in this.destructibleButtons_) {
        destructibleButton.Explode();
      }
    }
  }
}