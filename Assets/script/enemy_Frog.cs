using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_Frog : MonoBehaviour
{

    private Rigidbody2D rb;
    public Transform leftpoint,rightpoint;
    private Animator anim;
    private Collider2D coll;
    public LayerMask ground;
    public float Speed,jumpForce;
    private bool faceleft;
    private float leftx,rightx;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        transform.DetachChildren();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);//储存空点坐标以后销毁游戏实体，节省内存
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        switchAnimation();
    }

    void Movement()//根据空点判断位移的范围
    {
        if(faceleft)
        {
            if(rb.IsTouchingLayers(ground))//判断是否在地面上，在地面上才能跳跃
            {
                anim.SetBool("jumping",true);
                rb.velocity = new Vector2(-Speed,jumpForce);
            }
            if(transform.position.x < leftx)
            {
                rb.velocity = new Vector2(0,0);
                transform.localScale = new Vector3(-1,1,1);
                faceleft = false;
            }
        }else
        {
            if(rb.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping",true);
                rb.velocity = new Vector2(Speed,jumpForce);
            } 
            if(transform.position.x > rightx)
            {
                rb.velocity = new Vector2(0,0);
                transform.localScale = new Vector3(1,1,1);
                faceleft = true;
            }
        }
    }

    void switchAnimation()
    {
        if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y < 0.1f)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }
        if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling",false);
        }
    }
}
