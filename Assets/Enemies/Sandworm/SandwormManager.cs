using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandwormManager : MonoBehaviour
{
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    [SerializeField] Vector2 territory;
    [SerializeField] float spriteScale;
    [SerializeField] float colliderRadius;
    [SerializeField] float distanceBetweenBodyParts = 0.2f;
    [SerializeField] float speed = 10f;
    [SerializeField] float warningTime = 0.5f;
    [SerializeField] float gravityScale = 1;
    [SerializeField] float groundLevel;
    [SerializeField] LayerMask playerLayer;
    private Transform player;
    [SerializeField] float coolDown;

    private List<GameObject> sandwormBody = new List<GameObject>();
    private bool playerInSight;
    private bool readyToMove = true;
    private static System.Random rnd;
    private bool aboveGround = false;

    float count = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        rnd = new System.Random();
        //CreateBodyParts();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerInSight = Physics2D.OverlapBox(transform.position, territory, 0, playerLayer);
        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }
        if (readyToMove && playerInSight)
        {
            StartCoroutine(Move());
            readyToMove = false;
        }
        UpdateRotation();
        UpdateBodyParts();
        if (aboveGround && sandwormBody[0].transform.position.y < transform.position.y)
        {
            sandwormBody[0].GetComponent<Rigidbody2D>().gravityScale = 0;
            sandwormBody[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            aboveGround = false;
            StartCoroutine(StartCoolDown());
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(warningTime);
        sandwormBody[0].transform.position = rnd.Next(0, 2) == 0 ? new Vector2(player.position.x - 15 - rnd.Next(0,20), sandwormBody[0].transform.position.y) : new Vector2(player.position.x + 15 + rnd.Next(0, 20), sandwormBody[0].transform.position.y);
        Vector2 target;
        if (player.position.x - sandwormBody[0].transform.position.x > 0)
        {
            target = new Vector2(player.position.x - 5f, player.position.y + 12f);
        }
        else
        {
            target = new Vector2(player.position.x + 6f, player.position.y + 12f);
        }
        Vector2 vel = new Vector2((target.x - sandwormBody[0].transform.position.x) * speed, (target.y - sandwormBody[0].transform.position.y) * speed);
        sandwormBody[0].GetComponent<Rigidbody2D>().velocity = vel;
        yield return new WaitForSeconds(0.5f);
        sandwormBody[0].GetComponent<Rigidbody2D>().gravityScale = gravityScale;
        aboveGround = true;
    }

    IEnumerator StartCoolDown()
    {
        yield return new WaitForSeconds(coolDown);
        readyToMove = true;
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
                MarkerManager previousMarkerM = sandwormBody[i - 1].GetComponent<MarkerManager>();
                sandwormBody[i].transform.position = new Vector3(previousMarkerM.markerList[0].position.x, previousMarkerM.markerList[0].position.y, previousMarkerM.markerList[0].position.z + 0.1f);
                sandwormBody[i].transform.rotation = previousMarkerM.markerList[0].rotation;
                previousMarkerM.markerList.RemoveAt(0);
            }
        }
    }

    void CreateBodyParts()
    {
        count += Time.deltaTime;
        if (count >= distanceBetweenBodyParts || sandwormBody.Count == 0)
        {
            GameObject temp;
            if (sandwormBody.Count != 0)
            {
                MarkerManager previousMarkerM = sandwormBody[sandwormBody.Count - 1].GetComponent<MarkerManager>();
                temp = Instantiate(bodyParts[0], new Vector3 (previousMarkerM.markerList[0].position.x, previousMarkerM.markerList[0].position.y, previousMarkerM.markerList[0].position.z + 0.1f), previousMarkerM.markerList[0].rotation, transform);
            }
            else
            {
                temp = Instantiate(bodyParts[0], new Vector3 (transform.position.x, transform.position.y, 0.1f), transform.rotation, transform);
            }
            if (!temp.GetComponent<MarkerManager>())
                temp.AddComponent<MarkerManager>();
            if (!temp.GetComponent<Rigidbody2D>())
                temp.AddComponent<Rigidbody2D>();
            if (!temp.GetComponent<CircleCollider2D>())
                temp.AddComponent<CircleCollider2D>();
            temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            temp.GetComponent<CircleCollider2D>().isTrigger = true;
            temp.GetComponent<CircleCollider2D>().radius = colliderRadius;
            temp.transform.localScale = new Vector3(spriteScale, spriteScale, spriteScale);
            sandwormBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            count = 0;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, territory);
    }
}
