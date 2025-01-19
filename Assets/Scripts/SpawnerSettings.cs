using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Spawner settings", fileName = "_SpSet")]
public class SpawnerSettings : ScriptableObject
{
    public Sprite[] Ships;
    public PathType pathType;
    public PathMode pathMode;
    public MotionPath[] MotionPaths;
    [System.Serializable] public struct MotionPath { public Vector3[] path; }
}
