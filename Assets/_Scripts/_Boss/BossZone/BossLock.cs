using Cinemachine;
using DG.Tweening;
using UnityEngine;
public class BossLock : MyMonobehaviour
{
    [SerializeField] Collider2D lockCollide;
    [SerializeField] GameObject modelLock;
    [SerializeField] Transform startPos;
    [SerializeField] float lockHeight = 5f;
    [SerializeField] ParticleSystem appearParticle;
    [SerializeField] float animTime = 0.5f;
    [SerializeField] CinemachineImpulseSource impulseSource;

    public void Appear()
    {
        lockCollide.isTrigger = false;
        appearParticle.Play();
        impulseSource.GenerateImpulse();
        modelLock.transform.DOMove(startPos.position + new Vector3(0, lockHeight), animTime).OnComplete(() =>
        {
            appearParticle.Stop();
            impulseSource.GenerateImpulse();
        });
    }
    public void Disappear()
    {
        lockCollide.isTrigger = true;
        appearParticle.Play();
        impulseSource.GenerateImpulse();
        modelLock.transform.DOMove(startPos.position, animTime).OnComplete(() =>
        {
            appearParticle.Stop();
            impulseSource.GenerateImpulse();
        });
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos.position, startPos.position + new Vector3(0, lockHeight));
    }
}