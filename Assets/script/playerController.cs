using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private Animator anim;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public Collider2D coll;
    public int cherry=0;
    public int jumpcheck=0;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        Movement();
        SwitchAnimation();
    }

    void Movement()//人物移动函数
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");
        //水平上给刚体赋予移动力
        rb.velocity = new Vector2(horizontalmove * speed * Time.deltaTime, rb.velocity.y);

        //人物移动
        if(facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

        //人物移动动画
        anim.SetFloat("running",Mathf.Abs(facedirection));

        //趴姿动画
        if(Input.GetKey("s"))//这里采用了直接检测是否按下S键的方法，注意改键问题
        {
            anim.SetBool("crouching",true);
        }else if(anim.GetBool("crouching") && Input.GetKeyUp("s"))
        {
            anim.SetBool("crouching",false);
        }

        //人物跳跃
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            jumpcheck++;
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
            anim.SetBool("jumping",true);
            anim.SetBool("crouching",false);
        }
        
    }

    void SwitchAnimation()//跳跃变换函数
    {
        if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y<0)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
                
            }
        }
        else if(coll.IsTouchingLayers(ground))//触碰地面检测
        {
            anim.SetBool("falling",false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //收集品触碰检测
    {
        if(other.tag == "collection")
        {
            Destroy(other.gameObject);
            cherry++;
        }
    }
}
