using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    public ProjectileMovement bullet;
    public bool Firing;
    public float attackSpeed;

    public Vector3[] path;
    public GameObject boom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KaBoom(bool killed)
    {
        if (killed)
        {
            // odul
            transform.DOKill();
        }

        Instantiate(boom).transform.position = transform.position;
        Destroy(gameObject);
    }
}
