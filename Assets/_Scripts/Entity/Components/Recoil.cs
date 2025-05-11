using System.Collections;
using UnityEngine;

public class Recoil : EntityComponent
{
    [SerializeField] float recoilXSpeed = 10f;
    [SerializeField] float recoilYSpeed = 5f;
    [SerializeField] float recoilDuration = 0.2f;
    private bool isRecoiling = false;
    public bool IsRecoiling => isRecoiling;

    public void RecoilHorizontal(float direction)
    {

        StartCoroutine(ApplyRecoil(new Vector2(-direction * recoilXSpeed, entityController.rb.velocity.y)));

    }

    public void RecoilVertical(bool upward = true)
    {

        StartCoroutine(ApplyRecoil(new Vector2(entityController.rb.velocity.x, (upward ? 1 : -1) * recoilYSpeed)));
    }
    public void SetRecoilSpeed(float xSpeed, float ySpeed)
    {
        recoilXSpeed = xSpeed;
        recoilYSpeed = ySpeed;
    }
    public void RecoilBoth(float direction, bool upward = true)
    {
        if (!isRecoiling && entityController != null && entityController.rb != null)
        {
            Vector2 recoilVelocity = new Vector2(-direction * recoilXSpeed, (upward ? 1 : -1) * recoilYSpeed);
            StartCoroutine(ApplyRecoil(recoilVelocity));
        }
    }
    private IEnumerator ApplyRecoil(Vector2 recoilVelocity)
    {
        isRecoiling = true;

        if (entityController.rb != null)
        {
            entityController.rb.velocity = recoilVelocity;
        }

        yield return new WaitForSeconds(recoilDuration);

        isRecoiling = false;
    }

}

