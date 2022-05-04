using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public Transform leftLeg;
    public Transform rightLeg;
    [Space(20)]
    public float moveSpeed = 2;
    [Space(20)]
    public Transform cam;
    public Transform camPitch;
    public float camLerpSpeed = 2;

    private void Start()
    {
        anim.Play("walk");
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        //cam.position = Vector3.Lerp(cam.position, camPitch.position, camLerpSpeed * Time.deltaTime);
    }
}