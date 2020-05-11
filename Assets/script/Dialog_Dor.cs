using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_Dor : MonoBehaviour
{
    public GameObject enterDialog;
    public Animator anim;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            enterDialog.SetActive(true);
        }    
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            anim.Play("exitDialog");
        }    
    }  
}
