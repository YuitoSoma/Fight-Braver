using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    const int MinLine = -2;
    const int MaxLine = 2;
    const float LaneWidth = 1.0f;

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLine;

    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;
    public float accelerationZ;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown("left")) MoveToLeft();
        if (Input.GetKeyDown("right")) MoveToRight();
        if (Input.GetKeyDown("space")) Jump();

        // 徐々に加速しZ方向に常に全身させる
        float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
        moveDirection.z = Mathf.Clamp(accelerationZ, 0, speedZ);

        // X方向は目標のポジションまでの差分の割合で速度を計算
        float rationX = (targetLine * LaneWidth - transform.position.x) / LaneWidth;
        moveDirection.x = rationX * speedX;

        // 重力分の力をマイフレーム追加
        moveDirection.y -= gravity * Time.deltaTime;

        // 移動
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        // 移動後接地してたらY方向の速度はリセットする
        if (controller.isGrounded)
            moveDirection.y = 0;

        // 速度が０以上なら走っているフラグをtrueにする
        animator.SetBool("run", moveDirection.z > 0.0f);
    }

    public void MoveToLeft()
    {
        if (controller.isGrounded && targetLine > MinLine)
            targetLine--;
    }

    public void MoveToRight()
    {
        if (controller.isGrounded && targetLine < MaxLine)
            targetLine++;
    }

    public void Jump()
    {
        if (Input.GetButton("Jump"))
        {
            moveDirection.y = speedJump;
            animator.SetTrigger("Jump");
        }
    }
}
