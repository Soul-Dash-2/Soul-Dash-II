using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
public class ghostTrigger : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;
    [SerializeField] AnimationClip anim;
    [SerializeField] AnimatorOverrideController aoc;
    [SerializeField] float demoTimeInSec = 5;
    [SerializeField] float cooldownBeforeRegen = 2;
    private bool ghostAlive = false;
    private GameObject currGhost;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player" && !ghostAlive)
        {
            ghostAlive = true;
            currGhost = Instantiate(ghostPrefab, transform.position,Quaternion.identity);
            Animator animator = currGhost.GetComponent<Animator>();
            animator.runtimeAnimatorController = aoc;
            aoc["defaultAnim"] = anim;
            StartCoroutine(endDemo());


        }

    }
    IEnumerator endDemo()
    {
        yield return new WaitForSeconds(demoTimeInSec);
        Destroy(currGhost);
        yield return new WaitForSeconds(cooldownBeforeRegen);
        ghostAlive = false;

    }
}