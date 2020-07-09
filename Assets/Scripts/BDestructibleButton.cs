using stinkey.touch;

using stinkeys.button;
using stinkeys.destruction;

using UnityEngine;

using Object = UnityEngine.Object;

namespace stinkeys {
  public enum TouchStage {
    UP,
    DOWN_VIOLENT,
    DOWN_GENTLE,
    DOWN_TO_UP,
  }

  public abstract class BDestructibleButton : MonoBehaviour {
    // Breaking sounds.
    protected AudioSource source;

    private static AudioClip[] crackClips_, ughClips_, tapClips;

    protected GameObject rendererWrapper;

    private TouchStage touchStage_ = TouchStage.UP;
    private Touch? touchOrNull_;

    protected float moveAmount, moveFrac, toMoveFrac;

    private TouchButton touchButton_;

    private MeshCollider meshCollider_;

    public void Start() {
      // Blocks recursive case.
      if (this.transform.parent.gameObject.GetComponent<PianoKey>() != null) {
        return;
      }

      this.rendererWrapper = GameObject.Instantiate(this).gameObject;

      this.touchButton_ = this.gameObject.AddComponent<TouchButton>();
      this.touchButton_.OnTouch += this.MarkClicked_;
      this.touchButton_.OnPress += () => {
        toMoveFrac = 1;
        this.OnPress();
      };
      this.touchButton_.OnRelease += () => {
        toMoveFrac = 0;
        this.OnRelease();
      };

      var wrapperTransform = this.rendererWrapper.transform;
      wrapperTransform.parent = this.transform;
      wrapperTransform.localPosition = Vector3.zero;
      wrapperTransform.localRotation = Quaternion.identity;

      Object.Destroy(this.GetComponent<MeshRenderer>());
      Object.Destroy(this.rendererWrapper.GetComponent<MeshCollider>());
      Object.Destroy(this.rendererWrapper.GetComponent(this.GetType()));

      source = gameObject.AddComponent<AudioSource>();

      this.meshCollider_ = GetComponent<MeshCollider>();
      PhysicMaterial physicMaterial = this.meshCollider_.material;
      physicMaterial.bounceCombine = PhysicMaterialCombine.Maximum;
      physicMaterial.bounciness = 1;
      physicMaterial.staticFriction = 0;
      physicMaterial.dynamicFriction = 0;

      if (crackClips_ == null) {
        crackClips_ = AudioUtil.LoadClips("crack");
        ughClips_ = AudioUtil.LoadClips("ugh");
        tapClips = AudioUtil.LoadClips("tap");
      }
    }

    protected bool IsPlaying() => source.isPlaying;

    protected void Stop() => source.Stop();

    protected void PlayLooped(AudioClip clip) {
      if (clip == null) {
        return;
      }
      this.source.volume = 1;
      this.source.loop = true;
      this.source.pitch = 1;
      this.source.clip = clip;
      this.source.Play();
    }

    protected void Play(AudioClip clip) => AudioUtil.Play(this.source, clip);

    protected void PlayAtPitch(AudioClip clip, float pitch)
      => AudioUtil.PlayAtPitch(this.source, clip, pitch);

    protected void PlayAtRandomPitch(AudioClip[] clips)
      => AudioUtil.PlayAtRandomPitch(this.source, clips);

    protected void PlayAtRandomPitch(AudioClip clip)
      => AudioUtil.PlayAtRandomPitch(this.source, clip);

    private void PlayCrackSound_() => this.PlayAtRandomPitch(crackClips_);
    private void PlayUghSound_() => this.PlayAtRandomPitch(ughClips_);
    private void PlayTapSound_() => this.PlayAtRandomPitch(tapClips);

    public void OnCollisionEnter(Collision collision) {
      if (isBroken()) {
        PlayTapSound_();
      }
    }

    private float brokenTimer = 0;

    public bool isBroken() => brokenTimer > 0;

    protected virtual void OnBreak() {}

    protected virtual void OnPress()
      => AudioUtil.PlayAtRandomPitch(this.source,
                                     GrossSoundService.AudioClips);

    protected virtual void OnRelease() {}
    protected virtual void Move() {}

    private enum TouchAction {
      NONE,
      CLICK,
      DESTROY,
    }

    private void MarkClicked_(Touch touch) {
      if (this.isBroken() || this.touchStage_ == TouchStage.DOWN_VIOLENT) {
        return;
      }

      var isViolent = TouchUtil.IsSwipeViolent(touch);
      if (isViolent) {
        this.touchButton_.Disabled = true;
      }
      this.touchStage_ =
          isViolent ? TouchStage.DOWN_VIOLENT : TouchStage.DOWN_GENTLE;
      this.touchOrNull_ = touch;
    }

    public void LateUpdate() {
      if (this.touchStage_ == TouchStage.DOWN_TO_UP) {
        this.touchStage_ = TouchStage.UP;
      }

      this.UpdateState_();
      this.UpdateAnimation_();

      if (this.touchStage_ == TouchStage.DOWN_GENTLE ||
          this.touchStage_ == TouchStage.DOWN_VIOLENT) {
        this.touchStage_ = TouchStage.DOWN_TO_UP;
        this.touchOrNull_ = null;
      }
    }

    private void UpdateState_() {
      if (!this.isBroken() && this.touchStage_ == TouchStage.DOWN_VIOLENT) {
        brokenTimer = DestructionConstants.BROKEN_TIME;

        PlayCrackSound_();
        PlayUghSound_();

        this.meshCollider_.convex = true;

        var rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.drag = 0;
        rigidBody.angularDrag = 0;

        if (this.touchOrNull_ is Touch touch) {
          var keyPosition = this.meshCollider_.sharedMesh.bounds.center;

          var swipeIntensityVector = TouchUtil.GetSwipeIntensityVector(touch);

          var processedTouchDeltaPosition =
              new Vector3(-swipeIntensityVector.x,
                          0,
                          -swipeIntensityVector.y) * 4;

          var explosionDirection =
              processedTouchDeltaPosition + Vector3.up;
          var explosionPosition = keyPosition - 20 * explosionDirection;

          rigidBody.AddForce(Piano.VELOCITY_STRENGTH * 5 * explosionDirection,
                             ForceMode.VelocityChange);
        }
        else {
          rigidBody.AddExplosionForce(50f,
                                      20 * Vector3.down,
                                      100f,
                                      0,
                                      ForceMode.VelocityChange);
        }

        this.toMoveFrac = 0;
        this.OnBreak();
      }
    }

    private void UpdateAnimation_() {
      if (!isBroken()) {
        Move();
      }
      else {
        brokenTimer -= Time.deltaTime;

        if (!isBroken()) {
          this.touchButton_.Disabled = false;

          Destroy(gameObject.GetComponent<Rigidbody>());

          transform.localPosition = Vector3.zero;
          transform.localRotation = Quaternion.identity;
        }
      }
    }

    public void Explode() {
      if (!this.isBroken()) {
        this.touchStage_ = TouchStage.DOWN_VIOLENT;
        this.touchOrNull_ = null;
      }
      else {
        this.brokenTimer = DestructionConstants.BROKEN_TIME;
      }
    }
  }
}