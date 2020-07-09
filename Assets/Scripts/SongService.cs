using UnityEngine;

namespace stinkeys {
  public static class SongService {
    private static AudioSource SOURCE;

    public static void Initialize(GameObject gameObject) {
      SongService.SOURCE = gameObject.AddComponent<AudioSource>();
    }

    public static bool IsPlaying => SongService.SOURCE.isPlaying;

    public static void PlayLooped(AudioClip clip) {
      SongService.SOURCE.loop = true;
      SongService.SOURCE.pitch = 1;
      SongService.SOURCE.clip = clip;
      SongService.SOURCE.Play();
    }

    public static void Stop() => SongService.SOURCE.Stop();
  }
}
