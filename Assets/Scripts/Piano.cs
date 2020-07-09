using System;
using System.Collections.Generic;

using stinkeys.button;
using stinkeys.destruction;

using UnityEngine;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace stinkeys {
  public class Piano : MonoBehaviour {
    public static float GRAVITY_STRENGTH = 5;
    public static float VELOCITY_STRENGTH = Mathf.Sqrt(GRAVITY_STRENGTH);

    private float brokenTimer_ = 0;
    private bool IsBroken => this.brokenTimer_ > 0;

    private MeshCollider[] meshColliders_;
    private BoxCollider boxCollider_;

    public void Start() {
      GrossSoundService.Initialize();
      SongService.Initialize(this.gameObject);

      Physics.gravity *= GRAVITY_STRENGTH;

      this.meshColliders_ = GetComponentsInChildren<MeshCollider>();
      this.boxCollider_ = this.GetComponent<BoxCollider>();

      var physicMaterials = new List<PhysicMaterial>();
      foreach (var meshCollider in this.meshColliders_) {
        physicMaterials.Add(meshCollider.material);
      }
      physicMaterials.Add(this.boxCollider_.material);

      foreach (var physicMaterial in physicMaterials) {
        physicMaterial.bounceCombine = PhysicMaterialCombine.Maximum;
        physicMaterial.bounciness = .5f;
        physicMaterial.staticFriction = 0;
        physicMaterial.dynamicFriction = 0f;
      }
    }

    public void Update() {
      var prevBrokenTime = this.brokenTimer_;
      this.brokenTimer_ -= Time.deltaTime;

      if (prevBrokenTime > 0 && this.brokenTimer_ <= 0) {
        this.CleanUpExplosion_();
      }
    }

    public void Explode() {
      if (!this.IsBroken) {
        this.TriggerExplosion_();
      }
      this.brokenTimer_ = DestructionConstants.BROKEN_TIME;
    }

    private void TriggerExplosion_() {
      foreach (var meshCollider in this.meshColliders_) {
        meshCollider.convex = true;
        meshCollider.enabled = false;
      }

      this.boxCollider_.enabled = true;

      var rigidBody = gameObject.AddComponent<Rigidbody>();
      rigidBody.drag = 0;
      rigidBody.angularDrag = 0;
      rigidBody.mass = 10;

      rigidBody.AddExplosionForce(20f,
                                  new Vector3(
                                      0,
                                      -20,
                                      30),
                                  100f,
                                  0,
                                  ForceMode.VelocityChange);
    }

    private void CleanUpExplosion_() {
      Object.DestroyImmediate(GetComponent<Rigidbody>());

      this.boxCollider_.enabled = false;

      foreach (var meshCollider in this.meshColliders_) {
        meshCollider.convex = false;
        meshCollider.enabled = true;
      }

      transform.localPosition = Vector3.zero;
      transform.localRotation = Quaternion.identity;
    }
  }
}