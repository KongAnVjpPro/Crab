using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class WaterDistortion : MyMonobehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] LensDistortion lens;
    [Header("Distortion Settings")]
    public float baseIntensity = -0.3f;
    public float waveSpeed = 1.5f;
    public float waveStrength = 0.1f;

    public float xMultiplierBase = 1.0f;
    public float xMultiplierWave = 0.05f;

    public float yMultiplierBase = 1.0f;
    public float yMultiplierWave = 0.05f;

    private float timeOffset;
    void Start()
    {

        if (volume != null && volume.profile.TryGet(out lens))
        {
            lens.active = true;
        }


        timeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (lens == null) return;

        float t = Time.time + timeOffset;


        float wave = Mathf.Sin(t * waveSpeed) * waveStrength;

        lens.intensity.Override(baseIntensity + wave);
        lens.scale.Override(1.0f);

        lens.xMultiplier.Override(xMultiplierBase + Mathf.Sin(t * 2f) * xMultiplierWave);
        lens.yMultiplier.Override(yMultiplierBase + Mathf.Cos(t * 2.5f) * yMultiplierWave);


        lens.center.Override(new Vector2(
            0.5f + Mathf.Sin(t * 0.6f) * 0.01f,
            0.5f + Mathf.Cos(t * 0.7f) * 0.01f
        ));
    }
}