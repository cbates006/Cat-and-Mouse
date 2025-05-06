using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;

public class Cat : Agent
{
    public float velocityCorrection = 100f;
    public float speed = 0.05f;
    public Transform catTransform;
    public Transform mouseTransform;
    public GameObject mouse;
    public Rigidbody2D rb;
    private Mouse mouseScript;
    
    public void Start()
    {
        rb = mouse.GetComponent<Rigidbody2D>();
        mouseScript = mouse.GetComponent<Mouse>();
    }

    public override void OnEpisodeBegin()
    {
        //mouseTransform.localPosition = new Vector3 (0f, 0f, 0f);
        //catTransform.localPosition = new Vector3(8f, -3f, 0f);
        resetSpawn(catTransform);
        resetSpawn(mouseTransform);
        Debug.Log("begin");
        mouseScript.moveDirection = Random.insideUnitCircle.normalized;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(catTransform.localPosition);
        sensor.AddObservation(mouseTransform.localPosition);
        sensor.AddObservation(mouseTransform.localRotation);
        sensor.AddObservation(catTransform.localRotation);
        sensor.AddObservation(rb.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = actions.ContinuousActions[0];
        float rotation = actions.ContinuousActions[1];
        /*
        if (move < 0)
        {
            move *= -1;
        }
        */
        gameObject.transform.Translate(0, (move * velocityCorrection * 0.2f) * Time.deltaTime, 0);
        gameObject.transform.Rotate(0, 0, (rotation * speed * 0.05f) * Time.deltaTime);
        Debug.Log("action recieved");
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Vertical");
        continuousActions[1] = Input.GetAxisRaw("Horizontal");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="Mouse")
        {
            SetReward(+75f);
            EndEpisode();
            Debug.Log("collided");
        }
        
    }

    void Update()
    {
        //Move();
        SetReward(-0.1f);
    }
    void Move()
    {  
        float vert = Input.GetAxis("Vertical");
        
        if (vert < 0)
        {
            vert *= -1;
        }
        
        gameObject.transform.Translate(0, (vert * velocityCorrection) * Time.deltaTime, 0);

        float horiz = Input.GetAxis("Horizontal");

        gameObject.transform.Rotate(0, 0, (horiz * speed *0.1f) * Time.deltaTime);

    }

    void resetSpawn(Transform transform)
    {
        transform.localPosition = new Vector3(Random.Range(-9.5f,9.5f), Random.Range(-4f,4f), 0);
    }
}

