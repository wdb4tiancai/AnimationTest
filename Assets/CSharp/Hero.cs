using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    
    private int IsJumpID = Animator.StringToHash("IsJump");
    private int IsMoveID = Animator.StringToHash("IsMove");
    private int MoveSpeedID = Animator.StringToHash("MoveSpeed");
    private int TurnDirectionID = Animator.StringToHash("TurnDirection");
    private int MoveDirectionID = Animator.StringToHash("MoveDirection");

    public Animator anim;
    public float MoveSpeed;
    public float TurnDirection;
    public float MoveDirection;

    void Start()
    {
    }

    void Update()
    {
        anim.SetBool(IsMoveID, TurnDirection != 0 || MoveDirection != 0);
        anim.SetFloat(MoveSpeedID, MoveSpeed);
        anim.SetFloat(TurnDirectionID, TurnDirection);
        anim.SetFloat(MoveDirectionID, MoveDirection);
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetBool(IsJumpID, true);
        }
    }

}
