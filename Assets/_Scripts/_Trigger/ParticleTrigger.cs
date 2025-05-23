using UnityEngine;
public class ParticleTrigger : MyMonobehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] Vector3 originPos;
    protected override void Awake()
    {
        base.Awake();
        originPos = particle.transform.position;
    }
    public void PlayEff()
    {
        Vector3 pos = PlayerEntity.Instance.transform.position;
        if (pos == null) return;
        particle.transform.position = pos;
        if (particle.isPlaying)
        {
            particle.Stop();
        }
        particle.Play();
    }
    public void EndEff()
    {
        if (particle.isPlaying)
        {
            particle.Stop();
        }
        particle.transform.position = originPos;
    }
}