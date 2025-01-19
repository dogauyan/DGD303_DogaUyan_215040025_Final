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
        Dance,
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
    bool spawning = true;
    static public byte EnemyShipCount;

    // Start is called before the first frame update
    void Start()
    {
        EnemyShipCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.state == GameManager.GameState.ESC) return;
        if (waveIndex == enemyWaves.Length)
        {
            if (!spawning && EnemyShipCount == 0)
            {
                GameManager.Ended = true;
            }
            return;
        }

        timer += Time.deltaTime;
        while (timer >= enemyWaves[waveIndex].delay)
        {
            timer -= enemyWaves[waveIndex].delay;
            SpawnWave(enemyWaves[waveIndex]);

            if (waveIndex == enemyWaves.Length) break;
        }
    }

    void SpawnWave(EnemyWave wave)
    {
        spawning = true;

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
        GenerateMotion(wave.motion, new Vector3(_start, 6, 0), out List<Vector3> _motion, out PathType ptype);

        float realizedDuration = (float)_motion.Count / (float)wave.intendedSteps;

        _i = 0;
        while (true)
        {
            if (GameManager.state == GameManager.GameState.ESC)
            {
                yield return null;
            }

            EnemyShipCount++;
            var _enemy = Instantiate(enemy);
            _enemy.spriteRenderer.sprite = settings.Ships[(int)wave.ships[0]];
            _enemy.path = _motion.ToArray();
            _enemy.transform.position = _enemy.path[0];
            _enemy.transform.DOPath(_enemy.path, wave.duration * realizedDuration, ptype, default).SetEase(Ease.Flash).OnUpdate(() => { _enemy.transform.rotation = default; }).OnComplete(() => { _enemy.KaBoom(false); });

            if (++_i >= wave.count)
            {
                spawning = false;
                break;
            }

            yield return new WaitForSeconds(wave.gap);
        }

    }

    void GenerateMotion(MotionType type, Vector3 _point, out List<Vector3> _motion, out PathType ptype)
    {
        byte _i = 0;
        switch (type)
        {
            case MotionType.Vertical:
                _motion = new() { _point };

                _point.y = -6;
                _motion.Add(_point);

                ptype = PathType.Linear;
                break;
            case MotionType.Dance:
                ptype = PathType.CatmullRom;
                _motion = new() { _point };

                while (true)
                {
                    _point.x += Random.Range(-3.5f, 3.5f);
                    _point.x = Mathf.Clamp(_point.x, -3.5f, 3.5f);

                    _point.y -= Random.Range(-1, 3);
                    _point.y = Mathf.Clamp(_point.y, -6, 6);

                    _motion.Add(_point);

                    if (_i++ >= 32)
                    {
                        _point.x += Random.Range(-3.5f, 3.5f);
                        _point.x = Mathf.Clamp(_point.x, -3.5f, 3.5f);

                        _point.y = -6;

                        _motion.Add(_point);
                    }
                    if (_point.y <= -6) break;
                }

                break;
            case MotionType.Zigzag:
                ptype = PathType.Linear;
                _motion = new() { _point };

                bool yatay = false;
                bool sag = true;
                while (true)
                {
                    if (yatay)
                    {
                        if (sag)
                        {
                            _point.x += Random.Range(1, 7f);
                        }
                        else
                        {
                            _point.x -= Random.Range(1, 7f);
                        }

                        sag = !sag;
                        _point.x = Mathf.Clamp(_point.x, -3.5f, 3.5f);
                    } 
                    else
                    {
                        _point.y -= Random.Range(1, 3);
                        _point.y = Mathf.Clamp(_point.y, -6, 6);
                    }
                    yatay = !yatay;

                    _motion.Add(_point);

                    if (_i++ >= 32)
                    {
                        _point.y = -6;

                        _motion.Add(_point);
                    }
                    if (_point.y <= -6) break;
                }
                break;
            default:
                ptype = PathType.Linear;
                _motion = null;
                break;
        }
    }
}
