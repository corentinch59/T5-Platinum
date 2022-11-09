using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sunshine : MonoBehaviour
{
    [SerializeField] private Transform endPos;
    [SerializeField] private float timing;

    private Vector3 startPos;
    private Vector3 center;
    private Vector3 distance;

    public PostProcessManager postProcessManager;

    private float angle;
    private float radius;
    private float _ratio = 0f;

    public float ratio { get { return _ratio; } }



    private void Start()
    {
        startPos = transform.position;
        //endPos += transform.position;
        distance = endPos.position - startPos;

        center = (startPos + endPos.position) / 2;

        radius = distance.magnitude / 2;

        StartCoroutine(SunCoroutine());
        StartCoroutine(postProcessManager.SunshinePostProcess(timing));

    }

    private IEnumerator SunCoroutine()
    {
        float timer = 0f;

        while(timer < timing)
        {
            _ratio = timer / timing;
            angle =  Mathf.PI * _ratio;
            transform.position = new Vector3(-Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f) + center;
            timer += Time.deltaTime;

            yield return null;
        }
        //timer = 0f;
    }
}
