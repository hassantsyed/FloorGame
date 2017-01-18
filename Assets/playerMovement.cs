using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour {

    public float speed;
    private Vector3 spawn;
    public generate g;
    public float down;
    public Text score;
    public virtualjoystick jsl;
    public vertJS jsR;
    private int streak;
    public GameObject c;

    // Use this for initialization
    private Rigidbody rb;
    //private Text score;
	void Start () {
        rb = GetComponent<Rigidbody>();
        spawn = rb.position;
        score.text = ("STREAK: 0");

    }
    void Update()
    {
        //rb.transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);
        if (g.fall)
        {
            if (transform.position.y < -30)
            {
                die(false);
            }
        }
        else
        {
            if (transform.position.y < -10)
            {
                die(false);
            }
        }
        
    }
	// Update is called once per frame
	void FixedUpdate () {
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        Vector3 pos = new Vector3(jsl.Horizontal(), 0, jsR.Vertical());
        //print(transform.position.y);
        //if (transform.position.y <= .635)
        //{
            rb.AddForce(pos * speed);
        //}

        rb.AddForce(Physics.gravity * down, ForceMode.Force);
        //rb.AddForce(Vector3.down * down, ForceMode.Acceleration);
        //rb.AddForce(0, -down, 0);
        //rb.velocity = new Vector3 (0, -down, 0);
    }
    //reset start position and velocites upon falling off
    public void die(bool win)
    {
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //regen();
        gameObject.SetActive(true);
        rb.position = spawn;
        if (!win)
        {
            streak = 0;
            score.text = ("STREAK: " + streak);
        }
        g.time = 0;
        /*if (g.fall)
        {
            g.clean();
            g.makeBlocks();
        }*/
        g.clean();
        g.makeBlocks();

    }


    //generate new level if goal is touched
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("goal"))
        {
            streak++;
            score.text = ("STREAK: " + streak);
            g.reset();
            die(true);
        }
    }
}
