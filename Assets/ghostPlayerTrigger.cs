using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor.Animations;
public class ghostPlayerTrigger : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;
    [SerializeField] AnimationClip anim;
    [SerializeField] int demoTimeInSec = 5;
    [SerializeField] int cooldownBeforeRegen = 2;
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

        if (col.gameObject.tag == "Player"&&!ghostAlive)
        {
            ghostAlive = true;
            currGhost = Instantiate(ghostPrefab, gameObject.transform,false);
            Animator animator = currGhost.GetComponent<Animator>();
            AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
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
