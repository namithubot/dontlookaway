using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.Animations;
public class MoveCloser : MonoBehaviour
{
    [SerializeField]
    public Camera mainCamera;
    public Animator m_animator;

    [HideInInspector]
    public Player player;
    [SerializeField]
    public float speed = 1.0f;
    int pose;
    bool walking;

    public AudioSource footStep1;
    public AudioSource footStep2;

    public AudioSource softFootStep1;
    public AudioSource softFootStep2;

    public AudioSource quietFootStep1;
    public AudioSource quietFootStep2;

    void Start()
    {
        mainCamera = Camera.main;
        m_animator = GetComponent<Animator>();

        player = mainCamera.GetComponent<Player>();
        pose = m_animator.GetInteger("pose");
        walking = m_animator.GetBool("walking");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == Camera.main.gameObject.name)
        {
            player.EndGame();
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(mainCamera.transform.position, transform.position) < 1.5)
        {
            player.EndGame();
            Destroy(this.gameObject);
        }
        if (isCharacterVisible())
        {
            if (m_animator.GetInteger("pose")==0)
            {
                m_animator.SetInteger("pose", 1);
            }
  
        }
       else {
            if (m_animator.GetInteger("pose") == 1)
            {
                m_animator.SetInteger("pose", 0);   
            }
            MoveCharacter();

        }

    }
    
    bool isCharacterVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        if(GeometryUtility.TestPlanesAABB(planes, this.gameObject.GetComponent<Collider>().bounds))
        {
            return true;
        }

        return false;
    }
     public void MoveCharacter()
    {
        Vector3 direction =  mainCamera.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        Quaternion toRotation = Quaternion.LookRotation(direction,Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 100 * Time.deltaTime);
    }


    public void PlayFootStep1()
    {
        footStep1.Play();
    }

    public void PlayFootStep2()
    {
        footStep2.Play();
    }


    public void PlaySoftFootStep1() { 
        softFootStep1.Play();
    }

    public void PlaySoftFootStep2()
    {
        softFootStep1.Play();
    }


    public void PlayQuietFootStep1()
    {
        quietFootStep1.Play();
    }

    public void PlayQuietFootStep2()
    {
        quietFootStep2.Play();
    }


}
