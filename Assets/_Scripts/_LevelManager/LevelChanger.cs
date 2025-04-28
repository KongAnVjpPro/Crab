using UnityEngine;
[RequireComponent(typeof(TriggerZone))]
public class LevelChanger : MyMonobehaviour
{
    [SerializeField] private LevelConnection _connection;
    [SerializeField] string _targetSceneName;
    [SerializeField] private Transform _spawnPoint;

    public void EnterNewZone()
    {
        // if (PlayerEntity.Instance.pState.walkIntoNewScene) return;
        LevelConnection.ActiveConnection = _connection;//luu connect tu level truoc
        LevelManager.Instance.LoadScene(_targetSceneName, "WaveFade");
        // PlayerEntity.Instance.pState.walkIntoNewScene = true;
    }
    public void ExitTrigger()
    {
        // PlayerEntity.Instance.pState.walkIntoNewScene = false;
    }
    protected override void Awake()
    {
        base.Awake();
        if (_connection == LevelConnection.ActiveConnection)
        {
            // Debug.Log("ye");
            // PlayerEntity.Instance.transform.position = _spawnPoint.position;
        }
    }
    void Start()
    {
        if (_connection == LevelConnection.ActiveConnection)
        {
            // Debug.Log("ye");
            FindObjectOfType<PlayerEntity>().transform.position = _spawnPoint.position;
        }
    }
}