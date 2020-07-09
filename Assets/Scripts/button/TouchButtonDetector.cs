using System.Collections.Generic;

using stinkey.touch;

using stinkeys.destruction;

using UnityEngine;

namespace stinkeys.button {
  [RequireComponent(typeof(TrailRenderer))]
  public sealed class TouchButtonDetector : MonoBehaviour {
    private readonly IDictionary<int, TrailRenderer> trailRenderers_ =
        new Dictionary<int, TrailRenderer>();

    private TrailRenderer trailRendererPrototype_;

    public void Start() {
      this.trailRendererPrototype_ = GetComponent<TrailRenderer>();
    }

    public void Update() {
      var leftoverTrails =
          new Dictionary<int, TrailRenderer>(this.trailRenderers_);

      // Processes trails for the touches.
      foreach (var touch in Input.touches) {
        var fingerId = touch.fingerId;

        if (!leftoverTrails.TryGetValue(fingerId, out var trailRenderer)) {
          var gameObject = new GameObject();
          gameObject.transform.parent = this.transform;

          trailRenderer =
              ComponentUtil.CopyComponent(this.trailRendererPrototype_,
                                          gameObject);


          trailRenderer.material = this.trailRendererPrototype_.material;

          trailRenderer.numCapVertices = 5;
          trailRenderer.endWidth = 0;

          trailRenderer.time = .1f;

          this.trailRenderers_.Add(fingerId, trailRenderer);
        }
        else {
          leftoverTrails.Remove(fingerId);
        }

        var deltaPosition = touch.deltaPosition;
        var finalPosition = touch.position;

        var iterationMagnitude = 10;
        var totalMagnitude = deltaPosition.magnitude;

        var currentMagnitude = 0;
        while (currentMagnitude <= totalMagnitude) {
          var fraction = totalMagnitude > 0
                             ? currentMagnitude / totalMagnitude
                             : 0;
          var currentPosition = finalPosition - fraction * deltaPosition;

          var ray = Camera.main.ScreenPointToRay(currentPosition);
          if (Physics.Raycast(ray, out var hit)) {
            if (currentMagnitude == 0) {
              trailRenderer.gameObject.transform.position = hit.point;

              Color.RGBToHSV(Color.green,
                             out var greenH,
                             out var greenS,
                             out var greenV);

              Color.RGBToHSV(Color.red,
                             out var redH,
                             out var redS,
                             out var redV);

              var violenceIntensity =
                  Mathf.Min(TouchUtil.GetSwipeIntensityMagnitude(touch), 1);

              var h = Mathf.Lerp(greenH, redH, violenceIntensity);
              var s = Mathf.Lerp(greenS, redS, violenceIntensity);
              var v = Mathf.Lerp(greenV, redV, violenceIntensity);

              var lerpColor = Color.HSVToRGB(h, s, v);
              var invisibleLerpColor =
                  new Color(lerpColor.r, lerpColor.g, lerpColor.b, 0);

              trailRenderer.startColor = lerpColor;
              trailRenderer.endColor = invisibleLerpColor;
            }

            var button = hit.collider?.gameObject.GetComponent<TouchButton>();
            if (button != null) {
              button.MarkTouched(touch);
            }
          }

          currentMagnitude += iterationMagnitude;
        }
      }

      // Possibly prunes the remaining trails.
      foreach (var keyValuePair in leftoverTrails) {
        var fingerId = keyValuePair.Key;
        var trailRenderer = keyValuePair.Value;

        if (trailRenderer.positionCount == 0) {
          this.trailRenderers_.Remove(fingerId);
          Destroy(trailRenderer.gameObject);
        }
      }
    }
  }
}