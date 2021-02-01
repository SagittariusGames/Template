using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(RectTransform))]
public class GameTemplate : MonoBehaviour {

    // Variables declared in Unity Editor
    //[Header("Header")]
    //[Tooltip("Tooltip")]
    //[HideInInspector]
    //public float gizmoRadius = 0.0f;
    //public Rigidbody rb = null;

    // Script Lyfecycle: https://docs.unity3d.com/Manual/ExecutionOrder.html

    /// <summary>
    /// Awake is called when the script instance is being loaded. Only once in your life.
    /// </summary>
    void Awake() {
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start() {
        //rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// This function is called every fixed framerate frame.
    /// FixedUpdate should be used instead of Update when dealing with Rigidbody.
    /// </summary>
    void FixedUpdate() {
        //rb.AddForce(10.0f * Vector3.up);

        // movement
        /*if (device_x != 0) {
            rb.velocity = new Vector2(device_x * speed, myBody.velocity.y);
            anim.SetBool("Speed", true);
        }
        else {
            anim.SetBool("Speed", false);
            // avoiding slice on the floor
            rb.velocity = new Vector2(0, myBody.velocity.y);
        }*/

    }

    /// <summary>
    /// Update is called once per frame.
    /// Also for controls.
    /// </summary>
    void Update () {
        // Check controls
        //if (Input.GetKeyUp(KeyCode.Escape)) {
        //
        //}

        // Keep same time
        // float velocityGroupFixed = velocityGroup * Time.deltaTime;

        //flip
        /*if (device_x > 0) {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (device_x < 0) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }*/

    }

    /// <summary>
    /// This allows you to quickly pick important objects in your scene
    /// </summary>
    void OnDrawGizmos() {
        //if (gizmoRadius > 0) {
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawWireSphere(transform.position, gizmoRadius);
        //}
    }

    void OnTriggerEnter2D(Collider2D other) {
        // get points
        /*if (playerTag == other.tag) {
            // coin collected
            levelManager.CoinCollected();
            //play audio
            AudioSource.PlayClipAtPoint(audioCollected, transform.position);
            // self-destroing
            Destroy(this.gameObject);
        }*/
        /*if (((1 << other.gameObject.layer) & ladderLayer) != 0) {
            ladder++;
            myBody.velocity = new Vector2(0, 0);
            anim.SetBool("Jump", false);
        }*/

    }

    void OnTriggerExit2D(Collider2D other) {
        /*if (((1 << other.gameObject.layer) & ladderLayer) != 0) {
            ladder--;
        }*/
    }

    void OnCollisionEnter2D(Collision2D col) {
        /*if (enemyTag == col.collider.tag) {
            //dying
            levelManager.UpdateLives(-1);
            // active the effect
            Instantiate(explosion, this.transform.position, Quaternion.identity);
            //self-destroy
            Destroy(gameObject);

            levelManager.RestartLevel();
        }*/
    }

}
