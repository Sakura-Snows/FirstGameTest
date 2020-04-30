using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Version Record V0.0.2-Alpha
public class playerController : MonoBehaviour
{
    //人物控制类
    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private Animator anim;
    private bool isHurt;
    //物理判定类
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public Collider2D coll;
    public int cherry = 0;
    public int Gem = 0;
    public int jumpcheck=0;
    //UI类
    public Text CherryNum;
    public Text GemNum;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        if(isHurt == false)
        {
            Movement();
        }
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
        }else if(Input.GetKeyUp("s") )          //此处有bug,在引擎内选中人物项时功能正常，选中其他层时无法正常回到站立动画，具体情况带游戏初版导出测试  Alpha V0.0.1
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

    void SwitchAnimation()//动画变换函数
    {
        if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y<0)
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
                
            }
        }else if(isHurt)//插入收到伤害时的动画效果
        {
            anim.SetBool("hurt",true);
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt",false);
                anim.SetBool("idle",true);
                isHurt = false;
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
            CherryNum.text = cherry.ToString();
        }
        if(other.tag == "Gem")
        {
            Destroy(other.gameObject);
            Gem++;
            GemNum.text = Gem.ToString();

        }
    }

    private void OnCollisionEnter2D(Collision2D other)//敌人的控制类
    {   
        if(other.gameObject.tag == "Forg")//消灭敌人的方法和碰撞敌人产生的效果
        {
            if(anim.GetBool("falling"))
            {
                Destroy(other.gameObject);
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                anim.SetBool("jumping",true);
                anim.SetBool("crouching",false);
            }else if(transform.position.x < other.transform.position.x)
            {
                rb.velocity = new Vector2(-5,rb.velocity.y);
                isHurt = true;
            }else if(transform.position.x > other.transform.position.x)
            {
                rb.velocity = new Vector2(5,rb.velocity.y);
                isHurt = true;
            }
        }      
    }
    
}
