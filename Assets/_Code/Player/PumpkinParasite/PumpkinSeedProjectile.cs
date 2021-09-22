using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PumpkinSeedProjectile : MonoBehaviour
{
    [SerializeField] private float maxLifetime = 4.0f;
    [SerializeField] private float speed = 16.0f;
    [SerializeField] private VisualEffect explodeEffect;

    private Rigidbody myRigidbody;
    private Hurtbox myHurtbox;
    private Vector3 direction;
    private float lifetime = 0.0f;
    private bool isInKillStage = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;
        myHurtbox = GetComponent<Hurtbox>();
    }

    public void Init(GameObject owner, Vector3 forwardDirection)
    {
        direction = forwardDirection;
        myHurtbox.SetOwner(owner);
        myHurtbox.AddOnHitCallback(OnHit);
        myHurtbox.Activate();
    }

    private void OnHit(Collider other, int hitDamage, int hitType)
    {
        if (gameObject != null)
        {
            StartCoroutine(KillSelf());
        }
    }

    private IEnumerator KillSelf()
    {
        isInKillStage = true;
        myHurtbox.Deactivate();
        explodeEffect.enabled = true;
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInKillStage)
        {
            return;
        }

        myRigidbody.MovePosition(transform.position + direction * speed * Time.deltaTime);

        lifetime += Time.deltaTime;
        if (lifetime > maxLifetime)
        {
            StartCoroutine(KillSelf());
        }
    }
}
