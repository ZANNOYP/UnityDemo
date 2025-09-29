using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    //玩家移动速度
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    //玩家转动速度
    public float roundSpeed = 5f;

    public Animator animator;

    public Transform foot;

    public float jumpHeight = 0.5f;

    public LayerMask layerGround;

    public float checkSphereRadius = 0.1f;
    //玩家控制器
    private CharacterController controller;
    //玩家移动方向
    private Vector3 move;
    //玩家旋转
    private Quaternion targetRotation;
    [SerializeField]
    private float nowSpeed; 
    [SerializeField]
    private float targetSpeed;

    private float changeSpeed = 5f;
    [SerializeField]
    private bool isGround;
    [SerializeField]
    private float nowYspeed;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();
        Atk();
        TogDoor();
    }

    public void Move()
    {
        move = Quaternion.Euler(0, CameraController.yaw, 0) * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        move = move.magnitude > 1 ? move.normalized : move;
        if (move == Vector3.zero) 
            targetSpeed = 0;
        if (move != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, roundSpeed * Time.deltaTime);
            targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            
        }
        nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;

        controller.Move(move * nowSpeed * Time.deltaTime);
        animator.SetFloat("Speed", nowSpeed);
    }

    public void Jump()
    {
        
        isGround = CheckGround();
        if (isGround && Input.GetKeyDown(KeyCode.Space)) 
        {
            nowYspeed = Mathf.Sqrt(2 * 10 * jumpHeight);
        }
        if (!isGround)
        {
            nowYspeed -= 10 * Time.deltaTime;
        }
        else if (nowYspeed < 0)
        {
            nowYspeed = -1f;
        }
        
        controller.Move(Vector3.up * nowYspeed * Time.deltaTime);

        animator.SetFloat("ySpeed", nowYspeed);
        animator.SetBool("IsGround", isGround);
    }

    public bool CheckGround()
    {
        return Physics.CheckSphere(foot.position, checkSphereRadius, layerGround, QueryTriggerInteraction.Ignore);
    }

    public void Atk()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 1f, 1 << LayerMask.NameToLayer("Monster"), QueryTriggerInteraction.Ignore))
            {
                Monster m = hit.collider.gameObject.GetComponent<Monster>();
                if (m != null) 
                    m.Wound();
            }
        }
    }

    public void TogDoor()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 2f, 1 << LayerMask.NameToLayer("Door"), QueryTriggerInteraction.Ignore))
            {
                Door d = hit.collider.gameObject.GetComponent<Door>();
                print(d.gameObject.name);
                if (d != null)
                    d.ToggleDoor();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(foot.position, checkSphereRadius);
        //Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + transform.forward * 1f);
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + transform.forward * 2f);
    }
}
