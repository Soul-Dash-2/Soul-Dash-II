using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private class ShakeInstance
    {
        private float amplitude;
        private float duration;
        private float currentTime;
        private float scale;
        private float seed;

        private Vector2 shake;

        public ShakeInstance(float duration, float amplitude, float scale)
        {
            this.duration = duration;
            this.amplitude = amplitude * (Screen.width * 0.0015f);
            this.scale = scale;
            currentTime = duration;
            seed = Random.Range(-1000, 1000);
        }

        // recalculate the shake data, return true if the shake is still active, false otherwise
        public bool Update(float deltaTime)
        {
            if (currentTime < 0)
            {
                return false;
            }
            shake = CalculateShake();
            currentTime -= deltaTime;
            return true;
        }

        public Vector2 CalculateShake()
        {
            float x = Mathf.PerlinNoise(seed + (currentTime * scale), (-seed * 23) + (currentTime * scale));
            float y = Mathf.PerlinNoise(((seed + 10) * 17) + (currentTime * scale), seed + (currentTime * scale));
            return amplitude * (currentTime / duration) * (new Vector2(x - 0.5f, y - 0.5f));
        }

        public Vector2 GetShake()
        {
            return shake;
        }
    }

    private LinkedList<ShakeInstance> shakes;
    private Transform hero;         // The player
    private Camera playerCamera;    // The camera
    public float cameraSpeed;       // The speed at which the camera moves
    public float floatBias;         /* Factor which determines how close the camera 
                                        floats to the player, vs the mouse.
                                        Use a value of zero to deactivate camera floating
                                    */

    void Start()
    {
        hero = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        playerCamera = GetComponent<Camera>();
        shakes = new LinkedList<ShakeInstance>();
    }

    void Update()
    {
        // Dont do anything if float is disabled
        if (floatBias <= 0)
        {
            return;
        }
        Vector2 destination = GetDestination();
        Vector2 goal = destination + (Vector2)hero.position;
        Vector2 pos = playerCamera.transform.position;
        Vector2 direction = (goal - pos);
        Vector2 movement = direction * cameraSpeed * Time.deltaTime;
        Vector2 shakeSum = Vector2.zero;

        // get the sum of all shakes and turn them into the final shake vector
        LinkedList<ShakeInstance> newShakes = new();
        
        foreach (ShakeInstance shake in shakes)
        {
            if (shake.Update(Time.deltaTime))
            {
                shakeSum += shake.GetShake();
                newShakes.AddFirst(shake);
            }
        }
        shakes = newShakes;
        if (shakes.Count > 1) 
            shakeSum /= (shakes.Count - 1);
        SetPosition(new Vector2(pos.x + movement.x + shakeSum.x, pos.y + movement.y + shakeSum.y));
    }

    public void Shake(float duration, float amplitude, float scale)
    {
        shakes.AddFirst(new ShakeInstance(duration, amplitude, scale));
    }

    // Set the camera position
    void SetPosition(Vector2 vec)
    {
        playerCamera.transform.position = new Vector3(vec.x, vec.y, playerCamera.transform.position.z);
    }

    // Get the position between the player and mouse that the camera will move towards
    Vector2 GetDestination()
    {
        Vector2 pos = hero.position;
        Vector2 mouse = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        return (mouse - pos) * floatBias;
    }
}
