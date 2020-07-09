namespace stinkeys.button {
  public delegate void OnPressHandler();
  public delegate void OnReleaseHandler();

  public interface IButton {
    bool Disabled { get; }

    event OnPressHandler OnPress;
    event OnReleaseHandler OnRelease;
  }
}
