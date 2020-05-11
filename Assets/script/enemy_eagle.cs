using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_eagle : enemy_Controller
{
    private Rigidbody2D rb;
    private Collider2D coll;
    public Transform top,bottom;
    public float speed;
    private float topY,botttomY;
    private bool isUp = true;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        topY = top.position.y;
        botttomY = bottom.position.y;
        Destroy(top.gameObject);
        Destroy(bottom.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()//规定上下活动区间，到达顶点后向反方向移动
    {
        if(isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x,speed);
            if(transform.position.y > topY)
            {
                isUp = false;
            }
        }else
        {
            rb.velocity = new Vector2(rb.velocity.x,-speed);
            if(transform.position.y < botttomY)
            {
                isUp = true;
            }
        }
    }
}
