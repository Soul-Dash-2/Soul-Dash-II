using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Animator _animator;
    [SerializeField] float moveSpeed =10f;
    [SerializeField] float moveDistance = 10f;
    private GameObject player;
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] float fireballSpeed = 20f;
    [SerializeField] float fireballRange= 40f;
    private bool ifRun = true;
    private bool ifAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (ifRun)
        {
            _animator.SetBool("ifAttack",false);
            StartCoroutine(run());
            ifRun = false;
        }

        if (ifAttack)
        {
            _animator.SetBool("ifAttack", true);
            StartCoroutine(attack());
            ifAttack = false;

        }
        
    }

    IEnumerator run()
    {
        float movedDistance = 0;
        while (movedDistance < moveDistance)
        {
            _renderer.color= new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, _renderer.color.a==0.01f?0.05f:0.01f);
            _renderer.flipX = player.transform.position.x < transform.position.x; 
            transform.Translate(new Vector3(player.transform.position.x- transform.position.x, 0, 0).normalized*Time.deltaTime*moveSpeed);
            movedDistance += Time.deltaTime * moveSpeed;
            yield return null;
        }
        ifAttack = true;
    }

    IEnumerator attack()
    {
        float movedDistance = 0;
        _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 1);
        Instantiate(fireballPrefab,new Vector3(transform.position.x+ (_renderer.flipX?-1f:1f)
            ,transform.position.y,transform.position.z), Quaternion.identity);
        GameObject theFireball = GameObject.FindGameObjectsWithTag("Projectile")[0];
        while (movedDistance< fireballRange&& theFireball != null)
        {

            theFireball.transform.Translate((_renderer.flipX ? -transform.right : transform.right) * fireballSpeed*Time.deltaTime);
            movedDistance += fireballSpeed * Time.deltaTime;
            yield return null;
        }
        Destroy(theFireball);
        yield return new WaitForSecondsRealtime(1);
        ifRun = true;
    }

}
