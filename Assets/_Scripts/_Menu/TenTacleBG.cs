using UnityEngine;
public class TentacleBG : MyMonobehaviour
{
    [SerializeField] Vector3 originPos;

    [SerializeField] float limitRange = 10f;
    [SerializeField] float deltaMove = 1f;

    [SerializeField] float timerMove = 1f;
    [SerializeField] float counter = 0f;
    protected override void Awake()
    {
        base.Awake();
        originPos = transform.position;
    }
    void RandomMove()
    {

        float newPos = Random.Range(transform.position.x - deltaMove, transform.position.x + deltaMove);
        newPos = Mathf.Clamp(newPos, originPos.x - limitRange, originPos.y + limitRange);
        transform.position = new Vector3(newPos, transform.position.y, 0);
    }
    void Update()
    {
        counter += Time.deltaTime;
        if (counter < timerMove) return;
        RandomMove();
        counter = 0;
    }
}