using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    bool done;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.state == GameManager.GameState.ESC) return;

        transform.Translate(speed * Time.deltaTime * direction);
        if (transform.position.y > 6 || transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (done) return;

        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            done = true;
            enemy.KaBoom(true);
            Destroy(gameObject);
        }
    }
}
