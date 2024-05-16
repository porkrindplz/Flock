using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public List<Mover> birds { get; private set; }

    public float m_speed;
    public float m_separationRadius = 1.0f;
    [Range(0, 1)] public float m_separationWeight = 1.0f;
    public float m_alignmentRadius = 30;
    [Range(0, 1)] public float m_alignmentWeight = 1.0f;
    public float m_cohesionRadius = 30;
    [Range(0, 1)] public float m_cohesionWeight = 1.0f;
    public float m_targetRadius = 30;
    [Range(0, 1)] public float m_targetWeight = 1.0f;
    public float m_sightRadius = 5f;
    public float m_visionConeAngle = 60f;
    public float m_shootVisionAngle = 10f;
    public Transform target;
    private void Awake()
    {
        birds = new List<Mover>(GetComponentsInChildren<Mover>());
        foreach (var bird in birds)
        {
            bird.SetFLock(this);
        }
    }
}
