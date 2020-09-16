using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    static float gravity = 20;
    public float frictionSmoothSpeed = 0.2f;
    
    public float moveSpeed = 20;
    public float jumpSpeed = 8;
    public float shotThrust = 18;
    public float moveSmoothSpeed = 0.05f;
    public RectTransform cursorPosition;
    public GameObject gun;
    public GameObject piePrefab;

    CharacterController m_Controller;
    Vector3 m_Movement;
    Vector3 m_ShotMovement;
    Transform m_BarrelTransform;
    bool m_Jumping = false;
    bool m_HasShot = true;
    bool m_Swinging = false;
    bool m_OnSideCollision = false;
    Vector3 m_SwingPosition;
    Vector3 m_SwingVelocity;

    private float maxSpeed;

    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_BarrelTransform = gun.transform.GetChild(0);
        maxSpeed = moveSpeed + shotThrust;
    }

    void Update()
    {
        //horizontal movement
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed;
        if (horizontal != 0)
        {
            bool movingRightAndFaster = m_Movement.x >= 0 && horizontal >= 0 && horizontal < m_Movement.x;
            bool movingLeftAndFaster = m_Movement.x <= 0 && horizontal <= 0 && horizontal > m_Movement.x;
            if(m_Controller.isGrounded)
            {
                m_Movement.x = horizontal;
            }
            else
            {
                //ensure that you can only accelerate if input is in direction of momentum
                if (!(movingRightAndFaster || movingLeftAndFaster))
                {
                    m_Movement.x = Mathf.Lerp(m_Movement.x, horizontal, moveSmoothSpeed);
                }
            }
        }
        else
        {
            if(m_Controller.isGrounded)
            {
                m_Movement.x = Mathf.Lerp(m_Movement.x, 0, frictionSmoothSpeed);
            }
        }

        //jump if grounded
        m_Jumping = false;
        if (m_Controller.isGrounded)
        {
            m_Movement.y = 0;
            m_HasShot = true;
            if(Input.GetAxisRaw("Jump") == 1)
            {
                m_Jumping = true;
                m_Movement.y = jumpSpeed;
                AudioManager.Instance.PlayJump();
            }
        }

        //check for raycast hit on background
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(cursorPosition.position), out hit))
        {
            //have gun look at point
            Vector3 hitXY = new Vector3(hit.point.x, hit.point.y, 0);
            gun.transform.LookAt(hitXY + new Vector3(0, 0, gun.transform.position.z));

            //on shot
            m_ShotMovement = Vector3.zero;
            if (m_HasShot && Input.GetMouseButtonDown(0))
            {
                m_Movement.y = 0;
                
                //create vector to update movement
                m_ShotMovement = hitXY - transform.position;
                m_ShotMovement.z = 0;
                m_ShotMovement.Normalize();
                m_ShotMovement *= -shotThrust;

                //instantiate orb
                GameObject newShot = Instantiate(piePrefab, m_BarrelTransform.position, Quaternion.identity);
                newShot.GetComponent<Rigidbody>().velocity = (-2 * m_ShotMovement) + (0.5f * m_Controller.velocity);
                Destroy(newShot, 3);
                AudioManager.Instance.PlayShot();
                m_HasShot = false;
            }
            
            //on swing
            //check for surface in front of gun
            RaycastHit beam;
            if(Physics.Raycast(m_BarrelTransform.position, gun.transform.forward, out beam))
            {
                //if right mouse and a swingable surface
                if(Input.GetMouseButton(1) && beam.collider.CompareTag("SwingThing"))
                {
                    //if cursor is hovering over swing
                    if (!m_Swinging && Physics.Raycast(Camera.main.ScreenPointToRay(cursorPosition.position), 100, LayerMask.GetMask("Interactable")))
                    {
                        if(!m_Controller.isGrounded)
                        {
                            m_SwingPosition = beam.collider.transform.position;
                            m_Swinging = true;             
                        }
                        MouseCursor.LockCursor(beam.collider.transform.position);
                    }
                }
                else
                {
                    m_Swinging = false;
                    MouseCursor.UnlockCursor();
                }
            }
        }
        
        //bounce wall or ceiling
        if((m_Controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            if(m_Movement.y > 0)
            {
                m_Movement.y *= -0.5f;
            }
        }
        if((m_Controller.collisionFlags & CollisionFlags.Sides) != 0)
        {
            if(!m_OnSideCollision)
            {
                m_Movement.x *= -0.2f;
            }
            m_OnSideCollision = true;
        }
        else
        {
            m_OnSideCollision = false;
        }
        
        //update velocity vectors
        if(m_ShotMovement != Vector3.zero)
        {
            m_Movement += m_ShotMovement;
        }
        m_Movement.z = 0;
        m_Movement = Vector3.ClampMagnitude(m_Movement, maxSpeed);

        //swing or move character normally
        if (m_Swinging)
        {
            //get starting values
            Vector3 startPosition = transform.position;
            float tetherLength = (m_SwingPosition - transform.position).magnitude;

            //move character normally
            m_Controller.Move(m_SwingVelocity * Time.deltaTime);

            //move character back onto cirlce
            Vector3 r = transform.position - m_SwingPosition;
            if (r.magnitude > tetherLength)
            {
                Vector3 movePoint = m_SwingPosition + (r.normalized * tetherLength);
                Vector3 movement = movePoint - transform.position;
                movement.z = 0;
                m_Controller.Move(movement);
                m_SwingVelocity = (transform.position - startPosition) * (1 / Time.deltaTime);
            }

            //get new valocity
            m_SwingVelocity.y -= gravity * Time.deltaTime;
            if(m_Controller.isGrounded)
            {
                m_Controller.Move(m_Movement * Time.deltaTime);
                if (m_Jumping)
                {
                    m_SwingVelocity = m_Movement;
                }
            }
            else
            {
                m_Movement = m_SwingVelocity;
            }
        }
        else
        {
            m_Movement.y -= gravity * Time.deltaTime;
            m_Controller.Move(m_Movement * Time.deltaTime);
            m_SwingVelocity = m_Controller.velocity;
        }
    }
}
