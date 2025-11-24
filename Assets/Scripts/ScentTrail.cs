using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ScentTrail : MonoBehaviour
{
    public Transform target;               // Cat reference
    public int points = 20;                // Curve smoothness
    public float trailLength = 5f;         // How far the trail bends
    public float wiggleAmount = 0.3f;      // Wavy animation
    public float wiggleSpeed = 2f;         // Wavy animation speed
    public Gradient colorGradient;         // Glow color

    private LineRenderer lr;
    private Vector3[] curvePoints;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = points;
        curvePoints = new Vector3[points];

        if (colorGradient != null)
            lr.colorGradient = colorGradient;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 start = transform.position;
        Vector3 end = start + (target.position - start).normalized * trailLength;

        for (int i = 0; i < points; i++)
        {
            float t = i / (float)(points - 1);

            // Base curve
            Vector3 pos = Vector3.Lerp(start, end, t);

            // Wiggle on XZ
            pos += new Vector3(
                Mathf.Sin(Time.time * wiggleSpeed + t * 3f) * wiggleAmount,
                Mathf.Cos(Time.time * wiggleSpeed + t * 4f) * wiggleAmount * 0.2f,
                Mathf.Sin(Time.time * wiggleSpeed + t * 5f) * wiggleAmount
            );

            curvePoints[i] = pos;
        }

        lr.SetPositions(curvePoints);
    }
}
