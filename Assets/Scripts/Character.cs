using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SIDE { Left, Mid, Right }

public class Character : MonoBehaviour
{
    public SIDE m_Side = SIDE.Mid;
    float NewXPos = 0f;
    public bool SwipeLeft;
    public bool SwipeRight;
    public bool SwipeUp;
    public float XValue = 2.0f; // Ensure this is set to a reasonable value
    private CharacterController m_char;
    private float x;
    public float speedDodge = 5.0f;
    private float y;
    public float jumpPower = 10.0f;
    public bool Injump;
    public bool Inroll;

    void Start()
    {
        m_char = GetComponent<CharacterController>();
        transform.position = Vector3.zero;
        x = transform.position.x;
    }

    void Update()
    {
        // Detect swipe or key inputs
        SwipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        SwipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        SwipeUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        // Handle lane changes
        if (SwipeLeft)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXPos = -XValue;
                m_Side = SIDE.Left;
            }
            else if (m_Side == SIDE.Right)
            {
                NewXPos = 0;
                m_Side = SIDE.Mid;
            }
        }
        else if (SwipeRight)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXPos = XValue;
                m_Side = SIDE.Right;
            }
            else if (m_Side == SIDE.Left)
            {
                NewXPos = 0;
                m_Side = SIDE.Mid;
            }
        }

        // Lerp the x position for smooth movement
        x = Mathf.Lerp(x, NewXPos, Time.deltaTime * speedDodge);

        // Create movement vector with the updated x and y components
        Vector3 moveVector = new Vector3(x - transform.position.x, y, 0);

        // Apply movement to the CharacterController
        m_char.Move(moveVector * Time.deltaTime);

        // Handle jump
        jump();
    }

    public void jump()
    {
        if (m_char.isGrounded)
        {
            if (SwipeUp && !Injump) // Only jump if not already jumping
            {
                y = jumpPower;
                Injump = true;
            }
            else
            {
                y = -1f; // Slight downward force to ensure grounded state
                Injump = false;
            }
        }
        else
        {
            // Apply gravity
            y -= jumpPower * 2 * Time.deltaTime;
        }
    }
}
