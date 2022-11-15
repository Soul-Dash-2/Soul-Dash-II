using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform hero;         // The player
    private Camera playerCamera;    // The camera
    public float cameraSpeed;       // The speed at which the camera moves
    public float floatBias;         /* Factor which determines how close the camera 
                                        floats to the player, vs the mouse.
                                        Use a value of zero to deactivate camera floating
                                    */

    private float shakeAmplitude;
    private float shakeDuration;
    private float shakeCurrentTime;
    private float shakeScale;
    private bool doShake;
    private float shakeSeed;
    private float decreaseFactor;

    void Start() {
        hero = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        playerCamera = GetComponent<Camera>();
        doShake = false;
    }

    void Update()
    {
        // Dont do anything if float is disabled
        if (floatBias <= 0) {
            return;
        }
        Vector2 destination = GetDestination();
        Vector2 goal = destination + (Vector2) hero.position;
        Vector2 pos = playerCamera.transform.position;
        Vector2 direction = (goal - pos);
        Vector2 movement = direction * cameraSpeed * Time.deltaTime;
        Vector2 shake;

        if(doShake && shakeCurrentTime > 0) {
            shake = CalculateShake();
            shakeCurrentTime -= Time.deltaTime;
            if (shakeCurrentTime < 0) {
                doShake = false;
            }
        } else {
            shake = Vector2.zero;
        }

        SetPosition(new Vector2(pos.x + movement.x + shake.x, pos.y + movement.y + shake.y));
    }

    public void Shake(float duration, float amplitude, float scale) {
        doShake = true;

        shakeDuration = duration;
        shakeCurrentTime = duration;

        shakeAmplitude = amplitude;

        shakeScale = scale;
        shakeSeed = Random.Range(-1000, 1000);
    }

    // Set the camera position
    void SetPosition(Vector2 vec) {
        playerCamera.transform.position = new Vector3(vec.x, vec.y, playerCamera.transform.position.z);
    }

    // Get the position between the player and mouse that the camera will move towards
    Vector2 GetDestination()
    {
        Vector2 pos = hero.position;
        Vector2 mouse = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        return (mouse - pos) * floatBias;
    }

    Vector2 CalculateShake() {
        float x = Mathf.PerlinNoise(shakeSeed + (shakeCurrentTime * shakeScale), (-shakeSeed * 23) + (shakeCurrentTime * shakeScale));
        float y = Mathf.PerlinNoise(shakeSeed + (shakeCurrentTime * shakeScale), shakeSeed + (shakeCurrentTime * shakeScale));

        return shakeAmplitude * (shakeCurrentTime / shakeDuration) * (new Vector2(x - 0.5f, y - 0.5f));
    }
}
