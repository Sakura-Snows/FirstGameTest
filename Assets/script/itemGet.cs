using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemGet : MonoBehaviour
{
   public void Death()//物品被
   {
       if(gameObject.tag == "collection")
       {
           FindObjectOfType<playerController>().cherryGet();
           Destroy(gameObject);
       }
       else if(gameObject.tag == "Gem")
       {
           FindObjectOfType<playerController>().gemGet();
           Destroy(gameObject);
       }
   }
}
