using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityForce : MonoBehaviour {

    private static readonly float G = .1f;

    public float mass;
    public float radius = 1f;
    public Vector2 initForce;

    private Rigidbody2D rb;
    private CircleCollider2D cc2D;

    private bool paused;
    private Vector3 pauseForce = Vector3.zero;
    private float pauseAngle = 0f;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        cc2D = GetComponent<CircleCollider2D>();

        // local scale based on picture on collider, relative
        // Circle Collider shows actual radius amo
        this.transform.localScale = new Vector3(radius, radius, 1f);
        radius = radius * cc2D.radius;

        rb.gravityScale = 0f;
        if(mass == 0f) {
            rb.useAutoMass = true;
        } else {
            rb.useAutoMass = false;
            rb.mass = mass;
        }

        rb.velocity = initForce;
	}

    private void FixedUpdate() {
        if (paused) {
            rb.velocity = Vector3.zero;
            return;
        }

        GravityForce[] forces = GameObject.FindObjectsOfType<GravityForce>();
        Vector3 forceDirection = Vector3.zero;
        foreach (var force in forces) {
            if (force == this) { continue; }

            Vector3 distance = (force.transform.position - transform.position);

            float distSqr = distance.sqrMagnitude;

            Vector3 direction = distance / distSqr;

            float F = G * (mass * force.mass) / distSqr;

            forceDirection += direction * F;
            
            // F = G * (M1 * M2) / (Dist^2)
            // G = 6.674×10−11 N · (m/kg)2
        }

       // print(name + ": " + forceDirection);
        Vector2 vel = rb.velocity;
        vel.x += forceDirection.x;
        vel.y += forceDirection.y;
        rb.velocity = vel;
    }

    public void OnPauseGame() {
        paused = true;
        if (rb) {
            pauseForce = rb.velocity;
            pauseAngle = rb.angularVelocity;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
        }
    }

    public void OnResumeGame() {
        paused = false;
        if (rb) {
            rb.velocity = pauseForce;
            rb.angularVelocity = pauseAngle;
            pauseForce = Vector3.zero;
            pauseAngle = 0f;
        }
    }
}
