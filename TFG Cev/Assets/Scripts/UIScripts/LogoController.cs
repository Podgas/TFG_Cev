using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoController : MonoBehaviour
{
    [SerializeField]
    bool sukaiLogo = false; 
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
   
        if (Input.anyKeyDown && anim.GetCurrentAnimatorStateInfo(0).IsName("ProcyonLogo") || anim.GetCurrentAnimatorStateInfo(0).IsName("ProcyonLogo") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            anim.SetBool("sukaiLogo", true);
        }

        if (Input.anyKeyDown && anim.GetCurrentAnimatorStateInfo(0).IsName("SukaiLogo") || anim.GetCurrentAnimatorStateInfo(0).IsName("SukaiLogo") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            SceneController.LoadScene(SceneController.Scene.TitleScreen,false);
        }

    }
}
