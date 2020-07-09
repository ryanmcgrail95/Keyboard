namespace stinkeys {
  public class FartButton : BSoftDestructibleButton {
    protected override void OnPress() {
      KeyService.Mode =
          (KeyService.Mode == KeyMode.NORMAL) ? KeyMode.FART : KeyMode.NORMAL;
    }
  }
}