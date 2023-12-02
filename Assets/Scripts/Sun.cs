using DG.Tweening;
using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField] private AnimationCurve graph;
    [SerializeField] private float timeForMaxGraph;
    private float time;
    private float timeMultiplier;
    private float stopTimeLeft;
    private bool sunStopped;

    private void Start()
    {
        timeMultiplier = 0f;
        sunStopped = false;
    }
    private void Update()
    {
        if (Time.timeScale > 0)
        {
            if (!sunStopped)
            {
                time += Time.deltaTime;
                transform.position = new Vector3(0, transform.position.y - ((graph.Evaluate(time / timeForMaxGraph) * Time.deltaTime) * timeMultiplier), 0);
            }
            else if(stopTimeLeft <= 0)
            {
                sunStopped = false;
            }
            else
            {
                stopTimeLeft -= Time.deltaTime;
            }
        }
    }

    public void AscendTheSun(float amount)
    {
        //transform.position = new Vector3(0, transform.position.y + amount, 0);
        transform.DOMoveY(transform.position.y + amount, 0.2f);
    }

    public void ChangeTheSunSpeedMultiplier(float newMultiplier)
    {
        timeMultiplier = newMultiplier;
    }

    public void StopTheSunForTime(float time)
    {
        if (sunStopped)
            stopTimeLeft += time;
        else
        {
            sunStopped = true;
            stopTimeLeft = time;
        }

    }

}
