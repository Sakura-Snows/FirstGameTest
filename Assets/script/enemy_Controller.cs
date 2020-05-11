using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_Controller : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource deathAudio;
    // Start is called before the first frame update
    protected virtual void Start()//父类函数
    {
        anim = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }

    public void Death()//敌人死亡
    {
        Destroy(gameObject);
    }

    public void JumpOn()//跳到敌人头上触发的函数
    {
        deathAudio.Play();
        anim.SetTrigger("Death");
    }
}
