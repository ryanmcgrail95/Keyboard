namespace stinkeys {
  public class StopButton : BSoftDestructibleButton {
    protected override void OnPress() => SongService.Stop();
  }
}