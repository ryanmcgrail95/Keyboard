using System.Linq;

using UnityEngine;

namespace stinkeys {
  public static class GrossSoundService {
    private static AudioClip[] GROSS_CLIPS;

    public static void Initialize() {
      var fartClips = AudioUtil.LoadClips("fart");
      var burpClips = AudioUtil.LoadClips("burp");
      var sillyClips = AudioUtil.LoadClips("silly");

      GrossSoundService.GROSS_CLIPS =
          fartClips.Concat(burpClips).Concat(sillyClips).ToArray();
    }

    public static AudioClip[] AudioClips => GrossSoundService.GROSS_CLIPS;
  }
}