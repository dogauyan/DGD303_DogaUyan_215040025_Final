using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum ShipType
    {
        Asteroid,
        Mine,
        Barrier,
        Mower,
        Boarder,
        Cruiser,
        Fighter,
        Parafighter,
        Kamikaze,
        RocketShip,
        Flamer,
        ShieldShip,
        Core
    }
    public enum SpawnType
    {
        Repeater,
        Lined,
        Random,
        Group
    }
    public enum MotionType
    {
        Vertical,
        Wavey,
        Standoff,
        Zigzag,
        Positioned
    }
    [System.Serializable] public struct EnemyWave
    {
        public SpawnType spawn;
        public ShipType[] ships;
        public MotionType motion;
        public float gap;
        public float delay;
        public float duration;
        public byte intendedSteps;
        public byte count;
    }
    public EnemyWave[] enemyWaves;
    int waveIndex;
    public SpawnerSettings settings;
    public EnemyController enemy;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waveIndex == enemyWaves.Length) return;

        timer += Time.deltaTime;
        if (timer >= enemyWaves[waveIndex].delay)
        {
            timer -= enemyWaves[waveIndex].delay;
            SpawnWave(enemyWaves[waveIndex]);
        }
    }

    void SpawnWave(EnemyWave wave)
    {
        switch (wave.spawn)
        {
            case SpawnType.Repeater:
                StartCoroutine(RepeatedSpawn(wave));
                break;
            case SpawnType.Lined:
                break;
            case SpawnType.Random:
                break;
            case SpawnType.Group:
                break;
            default:
                break;
        }

        waveIndex++;
    }

    IEnumerator RepeatedSpawn(EnemyWave wave)
    {
        byte _i = 0;
        float _start = Random.Range(-4.5f, 4.5f);
        List<Vector3> _motion = new();
        Vector3 _point = new Vector3(_start,6,0);
        while (true)
        {
            _point.x += Random.Range(-4.5f, 4.5f);
            _point.x = Mathf.Clamp(_point.x, -4.5f, 4.5f);

            _point.y -= Random.Range(-0.5f, 2);
            _point.y = Mathf.Clamp(_point.y, -6, 6);

            _motion.Add(_point);

            if (_i++ >= 32)
            {
                _point.x += Random.Range(-4.5f, 4.5f);
                _point.x = Mathf.Clamp(_point.x, -4.5f, 4.5f);

                _point.y = -6;

                _motion.Add(_point);
            }
            if (_point.y <= -6) break;
        }

        float realizedDuration = (float)_motion.Count / (float)wave.intendedSteps;

        _i = 0;
        while (true)
        {
            var _enemy = Instantiate(enemy);
            _enemy.path = _motion.ToArray();
            _enemy.transform.position = _enemy.path[0];
            _enemy.transform.DOPath(_enemy.path, wave.duration * realizedDuration, settings.pathType, settings.pathMode).OnComplete(() => { _enemy.KaBoom(false); });

            if (++_i >= wave.count)
            {
                break;
            }

            yield return new WaitForSeconds(wave.gap);
        }
    }
}
