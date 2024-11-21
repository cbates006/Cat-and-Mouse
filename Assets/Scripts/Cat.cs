using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;

public class Cat : Agent
{
    /*
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private Transform wallTransform;
    [SerializeField] private GameObject wallGap;
    //[SerializeField] private Transform agentTransform;
    //[SerializeField] private Transform goalTransform;

    [SerializeField] public float velocityCorrection = 18f;
    [SerializeField] public float speed = 10000f;
    [SerializeField] public Transform catTransform;
    [SerializeField] public Transform mouseTransform;
    [SerializeField] public GameObject mouse;
    [SerializeField] public Rigidbody2D rb;

    //new float distanceToGoal;

    //new float distanceReward;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-13.5f, -1.5f), 0, Random.Range(-13.5f, 13.5f));
        targetTransform.localPosition = new Vector3(Random.Range(1.5f, +13.5f), 0, Random.Range(-13.5f, 13.5f));
        //wallTransform.localPosition = new Vector3(0, 0, Random.Range(-9f, 9f));
        Debug.Log("begin");
        mouseTransform.localPosition = new Vector3(0, 0, 0);
        //transform.localPosition = new Vector3(8, -3, 0);
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(agentTransform.localPosition);
        //sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(wallTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 3f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
        Debug.Log("action recieved");

    }*/
    
    public float velocityCorrection = 18f;
    public float speed = 1f;
    public Transform catTransform;
    public Transform mouseTransform;
    public GameObject mouse;
    public Rigidbody2D rb;
    
    public void Start()
    {
        rb = mouse.GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        mouseTransform.localPosition = new Vector3 (0f, 0f, 0f);
        catTransform.localPosition = new Vector3(8f, -3f, 0f);
        Debug.Log("begin");
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
        
        if (move < 0)
        {
            move *= -1;
        }

        gameObject.transform.Translate(0, (move * velocityCorrection) * Time.deltaTime, 0);
        gameObject.transform.Rotate(0, 0, (rotation * speed * 500) * Time.deltaTime);
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
            SetReward(+50f);
            EndEpisode();
            Debug.Log("collided");
        }
        
    }

    void Update()
    {
        //Move();
        //SetReward(-0.0001f);
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
}

