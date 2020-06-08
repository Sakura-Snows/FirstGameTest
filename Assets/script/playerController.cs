using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Version Record V0.6.8-Alpha TimeVersion Start on 2020-6-8
public class playerController : MonoBehaviour
{
    //人物控制类
    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private Animator anim;
    private bool isHurt;
    public AudioSource jumpAudio,hurtAudio,collectionAudio;
    //物理判定类
    [Space]
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public Collider2D coll;
    public Collider2D disColl;
    public int cherry = 0;
    public int Gem = 0;
    public int jumpcheck=0;
    public Transform cellingCheck;
    private bool headCheck = false;
    //UI类
    [Space]
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
        rb.velocity = new Vector2(horizontalmove * speed * Time.fixedDeltaTime, rb.velocity.y);

        //人物移动
        if(facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

        //人物移动动画
        anim.SetFloat("running",Mathf.Abs(facedirection));

        //趴姿动画
        crouch();//调用下面的趴姿函数
        /*
        if(Input.GetKey(KeyCode.S))
        {
            anim.SetBool("crouching",true);
        }else if(Input.GetKeyUp(KeyCode.S) )          //注：此处有bug,在引擎内选中人物项时功能正常，选中其他层时无法正常回到站立动画，具体情况带游戏初版导出测试  Alpha V0.0.1
        {                                             // ↑ 注：bug已修复，见下面趴姿函数
            anim.SetBool("crouching",false);
        }
        */
        //人物跳跃
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            jumpcheck++;
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);
            jumpAudio.Play();
            anim.SetBool("jumping",true);
            anim.SetBool("crouching",false);
        }
        
    }

    void crouch()//趴姿函数，实现自动站立
    {
        if(Input.GetButton("crouch"))
        {
            disColl.enabled = false;
            anim.SetBool("crouching",true);
        }else
        {
            headCheck = true;
        }
        if(headCheck)
        {
            if(!Physics2D.OverlapCircle(cellingCheck.position,0.2f,ground))
            {
                disColl.enabled = true;
                anim.SetBool("crouching",false);
                headCheck = false;
            }
        }

        /*无自动站立功能
        if(!Physics2D.OverlapCircle(cellingCheck.position,0.2f,ground))
        {
            if(Input.GetButtonDown("crouch"))
            {
                anim.SetBool("crouching",true);
                disColl.enabled = false;
            }else if(Input.GetButtonUp("crouch"))
            {
                anim.SetBool("crouching",false);
                disColl.enabled = true;
            }
        }
        */
    }

    void SwitchAnimation()//动画变换函数
    {
        if(Mathf.Abs(rb.velocity.y) > 5.0f && !coll.IsTouchingLayers(ground))//实现人物自然掉落的动画  注：此处判定引发消灭敌人的BUG，人物从斜坡下落时处于掉落状态，碰撞敌人将直接消灭对象。需重写敌人消灭判定 Alpha V0.0.4
        {
            anim.SetBool("falling",true);
            anim.SetBool("natureFall",true);
        }
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

    //碰撞器部分
    private void OnTriggerEnter2D(Collider2D other) //收集品触碰检测
    {
        if(other.tag == "collection")
        {
            collectionAudio.Play();
            Destroy(other.gameObject);
            cherry++;
            CherryNum.text = cherry.ToString();
        }
        if(other.tag == "Gem")
        {
            collectionAudio.Play();
            Destroy(other.gameObject);
            Gem++;
            GemNum.text = Gem.ToString();

        }
        if(other.tag == "DeadLine")//添加角色死亡条件碰撞检测
        {
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart",2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)//敌人的控制类
    {   
        if(other.gameObject.tag == "enemy")//消灭敌人的方法和碰撞敌人产生的效果 
        {
            enemy_Controller enemy = other.gameObject.GetComponent<enemy_Controller>();//申明父类
            if(anim.GetBool("falling"))
            {
                enemy.JumpOn();//用父类完成物体销毁
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);
                anim.SetBool("jumping",true);
                anim.SetBool("crouching",false);
            }else if(transform.position.x < other.transform.position.x)//实现与敌人碰撞时受伤的效果
            {
                rb.velocity = new Vector2(-5,rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
                
            }else if(transform.position.x > other.transform.position.x)
            {
                rb.velocity = new Vector2(5,rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }
        }      
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
