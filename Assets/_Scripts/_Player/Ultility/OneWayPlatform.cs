using System.Collections;
using UnityEngine;
public class OneWayPlatform : MyMonobehaviour

{
    [SerializeField] private Collider2D playerCollide;
    [SerializeField] private Collider2D platformCollide;
    [SerializeField] float disableTime = 0.5f;
    public string groundLayer = "Ground";
    public string alterLayer = "OneWayLayer";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (platformCollide != null)
            {
                StartCoroutine(DisableCollide());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision.name);
        if (!collision.gameObject.CompareTag("OneWayPlatform"))
        {
            return;
        }
        platformCollide = collision;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("Exit");
        if (!collision.gameObject.CompareTag("OneWayPlatform"))
        {
            return;
        }
        platformCollide = null;
    }
    IEnumerator DisableCollide()
    {
        if (platformCollide == null)
            yield break;
        Collider2D collide = platformCollide;
        // Physics2D.IgnoreCollision(collide, platformCollide);
        // groundLayer = collide.gameObject.layer;
        collide.gameObject.layer = LayerMask.NameToLayer(alterLayer);
        yield return new WaitForSeconds(disableTime);

        collide.gameObject.layer = LayerMask.NameToLayer(groundLayer);
        // Physics2D.IgnoreCollision(collide, platformCollide, false);
    }
}