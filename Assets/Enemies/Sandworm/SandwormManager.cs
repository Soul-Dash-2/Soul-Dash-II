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
    [SerializeField] GameObject finalSceneTrigger;
    [SerializeField] GameObject finalDialogueTrigger;

    private List<GameObject> sandwormBody = new List<GameObject>();
    private bool playerInSight;
    private bool readyToMove = true;
    private static System.Random rnd;
    private bool aboveGround = false;

    private AudioClip breakingGroundSFX;

    // Need a way to prevent children from all reporting the same instance of damage
    // this Dictionary maps the damage amount to the amount of time which has passed since that amount of damage was taken
    private Dictionary<float, float> damageToTime = new Dictionary<float, float>();
    private float damageInstanceTime;

    public float health;

    float count = 0;
    // Start is called before the first frame update
    void Start()
    {
        breakingGroundSFX = Resources.Load<AudioClip>("Audio/sandworm");
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        rnd = new System.Random();
        //CreateBodyParts();
        damageInstanceTime = 0.21f;
    }

    void Update()
    {
        Dictionary<float, float> newDict = new Dictionary<float, float>();
        foreach (KeyValuePair<float, float> entry in damageToTime)
        {
            // damageToTime[entry.Key] += Time.deltaTime;
            newDict[entry.Key] = damageToTime[entry.Key] + Time.deltaTime;
        }
        damageToTime = newDict;
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

        this.gameObject.GetComponent<AudioSource>().PlayOneShot(breakingGroundSFX, 1f);
        PlayerController p = player.gameObject.GetComponent<PlayerController>();
        p.GetPlayerCamera().Shake(warningTime/2, 0.2f, 20f);
        yield return new WaitForSeconds(warningTime);
        sandwormBody[0].transform.position = rnd.Next(0, 2) == 0 ? new Vector2(player.position.x - 16 - rnd.Next(0, 8), sandwormBody[0].transform.position.y) : new Vector2(player.position.x + 16 + rnd.Next(0, 8), sandwormBody[0].transform.position.y);
        Vector2 target;
        if (player.position.x - sandwormBody[0].transform.position.x > 0)
        {
            target = new Vector2(player.position.x - 5f, System.Math.Max(System.Math.Min(player.position.y + 10f, this.transform.position.y + (float)territory.y / 2), (this.transform.position.y + 20)));
        }
        else
        {
            target = new Vector2(player.position.x + 5f, System.Math.Max(System.Math.Min(player.position.y + 10f, this.transform.position.y + (float)territory.y / 2), (this.transform.position.y + 20)));
        }
        Vector2 vel = new Vector2((target.x - sandwormBody[0].transform.position.x) * speed, (target.y - sandwormBody[0].transform.position.y) * speed);
        sandwormBody[0].GetComponent<Rigidbody2D>().velocity = vel;
        yield return new WaitForSeconds(0.5f);
        p.GetPlayerCamera().Shake(0.3f, 0.5f, 8f);
        p.GetPlayerCamera().Shake(8f, 0.1f, 20f);
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
                temp = Instantiate(bodyParts[0], new Vector3(previousMarkerM.markerList[0].position.x, previousMarkerM.markerList[0].position.y, previousMarkerM.markerList[0].position.z + 0.1f), previousMarkerM.markerList[0].rotation, transform);
            }
            else
            {
                temp = Instantiate(bodyParts[0], new Vector3(transform.position.x, transform.position.y, 0.1f), transform.rotation, transform);

                if (!temp.GetComponent<Rigidbody2D>())
                    temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            if (!temp.GetComponent<MarkerManager>())
                temp.AddComponent<MarkerManager>();
            if (!temp.GetComponent<CircleCollider2D>())
                temp.AddComponent<CircleCollider2D>();
            temp.GetComponent<CircleCollider2D>().isTrigger = true;
            temp.GetComponent<CircleCollider2D>().radius = colliderRadius;
            temp.transform.localScale = new Vector3(spriteScale, spriteScale, spriteScale);
            sandwormBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            temp.GetComponent<WormSection>().SetManager(this);
            count = 0;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, territory);
    }

    public void TakeDamage(float dmg)
    {
        // this prevents taking damage from several segments at once
        if (!damageToTime.ContainsKey(dmg))
        {
            damageToTime[dmg] = damageInstanceTime;
        }
        if (damageToTime[dmg] < damageInstanceTime)
        {
            return;
        }
        health -= dmg;
        if (health <= 0)
        {
            PlayerController p = player.gameObject.GetComponent<PlayerController>();
            if (p.getDashing())
            {
                p.setDashType("sandworm");
                p.letDash();
            }
            p.GetPlayerCamera().Shake(2f, 0.75f, 10f);
            if (finalDialogueTrigger != null && finalSceneTrigger != null)
            {
                finalDialogueTrigger.GetComponent<BoxCollider2D>().enabled = true;
                finalSceneTrigger.GetComponent<BoxCollider2D>().enabled = true;
            }
            Destroy(this.gameObject);
        }
        damageToTime[dmg] = 0;
    }

    public float GetHealth()
    {
        return health;
    }
}
