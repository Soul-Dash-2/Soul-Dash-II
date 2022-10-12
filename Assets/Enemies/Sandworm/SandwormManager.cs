using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandwormManager : MonoBehaviour
{
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    [SerializeField] float scale;
    [SerializeField] float distanceBetweenBodyParts = 0.2f;
    [SerializeField] float speed = 10f;
    [SerializeField] float warningTime = 0.5f;
    [SerializeField] float gravityScale = 1;
    [SerializeField] float groundLevel;

    private List<GameObject> sandwormBody = new List<GameObject>();
    private bool startedMoving = false;

    float count = 0;
    // Start is called before the first frame update
    void Start()
    {
        //CreateBodyParts();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }
        if (!startedMoving)
        {
            StartCoroutine(Move());
            startedMoving = true;
        }
        UpdateRotation();
        UpdateBodyParts();
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(warningTime);
        Vector2 target = new Vector2(-32, 13f);
        Vector2 vel = new Vector2((target.x - sandwormBody[0].transform.position.x) * speed, (target.y - sandwormBody[0].transform.position.y) * speed);
        sandwormBody[0].GetComponent<Rigidbody2D>().velocity = vel;
        sandwormBody[0].GetComponent<Rigidbody2D>().gravityScale = gravityScale;
    }

    private void UpdateRotation()
    {
        Vector2 moveDirection = sandwormBody[0].GetComponent<Rigidbody2D>().velocity;
        if (moveDirection != Vector2.zero)
        {
            //sprite upward -x, y;  to the right y, x
            float angle = Mathf.Atan2(-moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
            sandwormBody[0].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void UpdateBodyParts()
    {
        if (sandwormBody.Count > 1)
        {
            for (int i = 1; i < sandwormBody.Count; i++)
            {
                MarkerManager markM = sandwormBody[i - 1].GetComponent<MarkerManager>();
                sandwormBody[i].transform.position = markM.markerList[0].position;
                sandwormBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
            }
        }
    }

    void CreateBodyParts()
    {
        if (sandwormBody.Count == 0)
        {
            GameObject temp = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp.GetComponent<MarkerManager>())
                temp.AddComponent<MarkerManager>();
            if (!temp.GetComponent<Rigidbody2D>())
            {
                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            }

            temp.transform.localScale = new Vector3(scale, scale, scale);
            sandwormBody.Add(temp);
            bodyParts.RemoveAt(0);
        }

        MarkerManager markM = sandwormBody[sandwormBody.Count - 1].GetComponent<MarkerManager>();
        if (count == 0)
        {
            markM.ClearMarkerList();
        }
        count += Time.deltaTime;
        if (count >= distanceBetweenBodyParts)
        {
            GameObject temp = Instantiate(bodyParts[0], markM.markerList[0].position, markM.markerList[0].rotation, transform);
            if (!temp.GetComponent<MarkerManager>())
                temp.AddComponent<MarkerManager>();
            if (!temp.GetComponent<Rigidbody2D>())
            {
                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            temp.transform.localScale = new Vector3(scale, scale, scale);
            sandwormBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            count = 0;
        }
    }
}
