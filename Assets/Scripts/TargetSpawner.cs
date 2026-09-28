using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private TargetPool _targetPool;
    [SerializeField] private Camera _camera;

    [SerializeField] private float _spawnBottomMargin = 0.5f;
    [SerializeField] private float _apexMin = 0.7f;
    [SerializeField] private float _apexMax = 0.8f;

    [SerializeField] private float _spawnInterval = 1.0f;

    [SerializeField] private float _spawnZMin = -7.0f;
    [SerializeField] private float _spawnZMax = -5.0f;

    [SerializeField] private float _horizontalDistanceMin = 1.5f;
    [SerializeField] private float _horizontalDistanceMax = 3.0f;

    private float _time;

    private void Awake()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    public void Spawn()
    {
        Target target = _targetPool.Get();

        float z = Random.Range(_spawnZMin, _spawnZMax);

        float depth = Vector3.Dot(new Vector3(0.0f, 0.0f, z) - _camera.transform.position, _camera.transform.forward);

        Vector3 bottomLeft = _camera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, depth));

        Vector3 topRight = _camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, depth));

        // 타겟의 크기를 고려해서 화면 가장자리에 여유 공간을 둔다.
        float targetRadius = GetTargetRadius(target);
        float horizontalMargin = targetRadius + 0.5f;
        float minX = bottomLeft.x + horizontalMargin;
        float maxX = topRight.x - horizontalMargin;

        float minY = bottomLeft.y + targetRadius;
        float maxY = topRight.y - targetRadius;

        // 시작 위치
        float spawnX = Random.Range(minX, maxX);
        float spawnY = Mathf.Lerp(minY, maxY, 0.05f);

        spawnY -= _spawnBottomMargin;

        // 최고점
        float apexY = Mathf.Lerp(minY, maxY, Random.Range(_apexMin, _apexMax));

        // 시작점에서 이동할 X 거리
        float horizontalDistance = Random.Range(_horizontalDistanceMin, _horizontalDistanceMax);

        // 좌우 방향 랜덤
        float direction = Random.value < 0.5f ? -1.0f : 1.0f;
        float targetX = spawnX + horizontalDistance * direction;

        // 최고점의 X도 화면 밖으로 나가지 않도록 제한
        targetX = Mathf.Clamp(targetX, minX, maxX);
        Vector3 position = new Vector3(spawnX, spawnY, z);

        float gravity = Mathf.Abs(Physics.gravity.y);

        float height = apexY - spawnY;

        float verticalSpeed = Mathf.Sqrt(2.0f * gravity * height);

        // 최고점까지 걸리는 시간
        float apexTime = verticalSpeed / gravity;

        // 최고점까지 이동할 X 속도를 계산
        float horizontalSpeed = (targetX - spawnX) / apexTime;

        Vector3 velocity = new Vector3(horizontalSpeed, verticalSpeed, 0.0f);

        target.Launch(position, velocity);
    }

    private float GetTargetRadius(Target target)
    {
        Collider collider = target.GetComponentInChildren<Collider>();

        if (collider == null)
        {
            return 0.5f;
        }

        Bounds bounds = collider.bounds;

        return Mathf.Max(bounds.extents.x, bounds.extents.y);
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _spawnInterval)
        {
            Spawn();
            _time = 0.0f;
        }
    }

}
