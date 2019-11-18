using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoController : MonoBehaviour
{
    [SerializeField]
    SceneController sc;
    bool fadeout = false;
    float timeToFade = 2f;
    float actualTime;   
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("FadeIn") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >=1.0f) { 
            if (actualTime <= timeToFade)
            {
                actualTime += Time.deltaTime;
            }
            else
            {
                anim.SetBool("fadeOut", true);
            }

        
        }

        if (Input.anyKey || anim.GetCurrentAnimatorStateInfo(0).IsName("FadeOut") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            sc.LoadScene(1);
        }

    }
}
