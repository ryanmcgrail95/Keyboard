using UnityEngine;

namespace stinkeys {
  public static class ComponentUtil {
    public static TComponent CopyComponent<TComponent>(
        TComponent original,
        GameObject destination) where TComponent : Component {
      var copy = destination.AddComponent<TComponent>();

      // Copied fields can be restricted with BindingFlags
      var type = original.GetType();
      var fields = type.GetFields();
      foreach (var field in fields) {
        field.SetValue(copy, field.GetValue(original));
      }

      return copy;
    }
  }
}