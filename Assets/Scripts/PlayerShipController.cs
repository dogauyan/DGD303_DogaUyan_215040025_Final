using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject boom;
    byte sprIndex = 2;
    public Sprite[] turnSprites;
    public SpriteRenderer spriteRenderer;
    Vector3 currentVelocity;
    public ProjectileMovement bullet;
    public float attackSpeed;
    bool _firing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.state == GameManager.GameState.ESC) return;
        if (audioSource.outputAudioMixerGroup == null && VolumeManager.SoundFx != null) audioSource.outputAudioMixerGroup = VolumeManager.SoundFx;

        // fare konumunu al
        Vector3 Fare = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Fare.x = Mathf.Clamp(Fare.x, -3.5f, 3.5f);
        Fare.y = Mathf.Clamp(Fare.y, -4.5f, 4.5f);
        Fare.z = 0;

        // oraya dogru suruklen
        if ((Fare - transform.position).magnitude > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, Fare, ref currentVelocity, 0.1f, 5);
        }
        else currentVelocity = default;
        
        // yatay suruklenmene gore sprite degistir
        byte _sprIndex = (byte)(Mathf.RoundToInt(Mathf.Min(2, Mathf.Abs(currentVelocity.x * 1)) * Mathf.Sign(currentVelocity.x)) + 2);
        if (_sprIndex != sprIndex)
        {
            sprIndex = _sprIndex;
            spriteRenderer.sprite = turnSprites[sprIndex];
        }

        // fare tikliyken ates et
        if (Input.GetMouseButtonDown(0))
        {
            _firing = true;
            StartCoroutine(Firing());
        }
        if (Input.GetMouseButtonUp(0) || (_firing && !Input.GetMouseButton(0)))
        {
            _firing = false;
            StopAllCoroutines();
        }
    }

    IEnumerator Firing()
    {
        while (true)
        {
            Instantiate(bullet).transform.position = transform.position;
            audioSource.PlayOneShot(audioSource.clip);
            yield return new WaitForSeconds(1/attackSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.DOKill();
        Instantiate(boom).transform.position = transform.position;
        Destroy(gameObject);
    }
}
