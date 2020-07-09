using stinkeys;

using UnityEngine;

public class SongButton : BSoftDestructibleButton {

	[SerializeField]
	protected AudioClip song;

	protected override void OnPress() {
    if (this.song != null) {
      if (SongService.IsPlaying) {
        SongService.Stop();
      } else {
        SongService.PlayLooped(this.song);
      }
    }
    else {
      base.OnPress();
    }
  }
  protected override void OnRelease() { }

}
