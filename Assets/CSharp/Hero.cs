using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    
    private int IsJumpID = Animator.StringToHash("IsJump");
    private int IsMoveID = Animator.StringToHash("IsMove");
    private int IsRunID = Animator.StringToHash("IsRun");
    private int IsTossGrenadeID = Animator.StringToHash("IsTossGrenade");
    private int IsReloadID = Animator.StringToHash("IsReload");
    private int IsFiringRifleID = Animator.StringToHash("IsFiringRifle");
    private int TurnDirectionID = Animator.StringToHash("TurnDirection");
    private int MoveDirectionID = Animator.StringToHash("MoveDirection");

    

        

        
    public Animator m_Anim;
    public CharacterController m_CharacterController;

    public float m_TurnDirection;
    public float m_MoveDirection;
    public float m_IsRun=0;
    public float m_MoveSpeed;
    public Vector3 m_MoveData;
    public Vector3 m_MoveRotation;

    void Start()
    {
        m_MoveData = new Vector3(0,0,0);
        m_MoveRotation = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            m_MoveDirection = 1;
            m_MoveSpeed = 2;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            m_MoveDirection = 0;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            m_MoveDirection = -1;
            m_MoveSpeed = 2;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            m_MoveDirection = 0;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            m_MoveDirection = 0;
            m_TurnDirection = -1;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            m_TurnDirection = 0;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            m_MoveDirection = 0;
            m_TurnDirection = 1;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            m_TurnDirection = 0;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_IsRun = 1;
            m_MoveSpeed = 5;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            m_IsRun = 0;
            m_MoveSpeed = 2;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            m_Anim.SetBool(IsJumpID, true);

        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            m_Anim.SetBool(IsReloadID, true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_Anim.SetBool(IsFiringRifleID, true);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            m_Anim.SetBool(IsFiringRifleID, false);
        }else if (Input.GetMouseButtonDown(1))
        {
            m_Anim.SetBool(IsTossGrenadeID, true);
        }


        m_MoveData.z = m_MoveDirection * m_MoveSpeed * Time.deltaTime;
        m_Anim.SetBool(IsMoveID, m_TurnDirection!=0 || m_MoveDirection != 0);
        m_Anim.SetFloat(MoveDirectionID, m_MoveDirection);
        m_Anim.SetFloat(IsRunID, m_IsRun);
        m_Anim.SetFloat(TurnDirectionID, m_TurnDirection);
        m_CharacterController.Move(m_CharacterController.transform.rotation *m_MoveData);

        m_MoveRotation = m_CharacterController.transform.rotation.eulerAngles;
        m_MoveRotation.y += m_TurnDirection * 45 *Time.deltaTime;
        m_CharacterController.transform.rotation = Quaternion.Euler(m_MoveRotation);
    }

}
