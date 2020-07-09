using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace stinkeys {
  public static class AudioUtil {
    /**
     * Loads in a list of audio clips. Assumes they are in the format
     * "[name]0", "[name]1", "[name]2", etc. Stops loading once it fails to
     * load one of the clips.
     */
    public static AudioClip[] LoadClips(string name) {
      var clips = new List<AudioClip>();

      AudioClip clip;
      var i = 0;
      while ((clip = Resources.Load<AudioClip>(name + i)) != null) {
        clips.Add(clip);
        i++;
      }

      return clips.ToArray();
    }

    public static void Play(AudioSource source, AudioClip[] clips)
      => AudioUtil.Play(source, AudioUtil.PickRandom_(clips));

    public static void Play(AudioSource source, AudioClip clip)
      => AudioUtil.PlayAtPitch(source, clip, 1);


    public static void PlayAtPitch(
        AudioSource source,
        AudioClip[] clips,
        float pitch)
      => AudioUtil.PlayAtPitch(source, AudioUtil.PickRandom_(clips), pitch);

    public static void PlayAtPitch(
        AudioSource source,
        AudioClip clip,
        float pitch) {
      if (clip == null) {
        return;
      }

      source.volume = 1;
      source.pitch = pitch;
      source.PlayOneShot(clip);
    }


    public static void PlayAtRandomPitch(AudioSource source, AudioClip[] clips)
      => AudioUtil.PlayAtRandomPitch(source, AudioUtil.PickRandom_(clips));

    public static void PlayAtRandomPitch(AudioSource source, AudioClip clip)
      => AudioUtil.PlayAtPitch(source, clip, Random.Range(.9f, 1.1f));


    private static T PickRandom_<T>(T[] items)
      => items[Random.Range(0, items.Length)];

    public static IEnumerator FadeSourceOut(
        AudioSource source,
        float durationInSeconds) {
      var elapsedTimeInSeconds = 0f;
      var startValue = source.volume;

      while (elapsedTimeInSeconds < durationInSeconds) {
        elapsedTimeInSeconds += Time.deltaTime;

        var elapsedFraction = elapsedTimeInSeconds / durationInSeconds;
        source.volume = Mathf.Lerp(startValue, 0, elapsedFraction);
        yield return null;
      }

      source.Stop();
    }
  }
}