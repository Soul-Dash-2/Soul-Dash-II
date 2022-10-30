using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistantView : MonoBehaviour
{
    public GameObject follow;
    public float scaleOffset;
    public bool isHorizontal = true;
    public bool isVertical = true;
    Vector2 pos;
    Vector2 followPos;
    float offsetX;
    float offsetY;


    // Start is called before the first frame update
    void Start()
    {
        if (follow != null)
            followPos = follow.transform.localPosition;

    }

    void LateUpdate()
    {
        if(follow!= null)
        {
            pos = transform.localPosition;
            if (isHorizontal)
            {
                offsetX = (follow.transform.localPosition.x - followPos.x) * scaleOffset;
                pos.x += offsetX;
            }

            if (isVertical)
            {
                offsetY = (follow.transform.localPosition.y - followPos.y) * scaleOffset;
            }

            transform.localPosition = pos;
            followPos = follow.transform.localPosition;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
