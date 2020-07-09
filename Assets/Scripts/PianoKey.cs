using UnityEngine;

namespace stinkeys {
  public class PianoKey : BDestructibleButton {
    private int semitonesFromCenterA_;

    private AudioSource noteSource_;

    //private static readonly Vector3 ROTATION_POINT = new Vector3(-17.18f, 4.29f, 20.81f);

    //private static readonly Vector3 ROTATION_POINT = new Vector3(0, -15f, 3f);
    private static readonly Vector3 ROTATION_POINT = new Vector3(0, -9.5f, 2f);
    private static readonly Vector3 ROTATION_AXIS = Vector3.left;

    private AudioClip clip_;

    // Use this for initialization
    public new void Start() {
      base.Start();

      this.noteSource_ = gameObject.AddComponent<AudioSource>();

      this.semitonesFromCenterA_ = this.transform.GetSiblingIndex() - 16;
      this.clip_ = Resources.Load<AudioClip>("Piano-A-long");
    }

    protected override void OnBreak() => this.PlayNote_();

    protected override void OnPress() {
      if (KeyService.Mode == KeyMode.NORMAL) {
        this.PlayNote_();
      }
      else {
        //this.playRandomPitch(fartClips[this.semitonesFromCenterA_ + 16]);
        AudioUtil.PlayAtRandomPitch(this.noteSource_,
                                    GrossSoundService.AudioClips);
      }
    }

    private void PlayNote_() {
      var pitch = Mathf.Pow(1.05946f, this.semitonesFromCenterA_);
      AudioUtil.PlayAtPitch(this.noteSource_, this.clip_, pitch);
    }

    protected override void OnRelease() {
      if (!this.isBroken()) {
        this.StartCoroutine(AudioUtil.FadeSourceOut(this.noteSource_, .25f));
      }
    }

    protected override void Move() {
      var targetedTransform = this.rendererWrapper.transform;

      targetedTransform.RotateAround(PianoKey.ROTATION_POINT,
                                     PianoKey.ROTATION_AXIS,
                                     -this.moveAmount);
      this.moveFrac += (this.toMoveFrac - this.moveFrac) / 3;
      this.moveAmount = -this.moveFrac * 4;
      targetedTransform.RotateAround(PianoKey.ROTATION_POINT,
                                     PianoKey.ROTATION_AXIS,
                                     this.moveAmount);
    }
  }
}