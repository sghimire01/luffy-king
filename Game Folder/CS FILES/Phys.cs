using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phys : MonoBehaviour
{

   public float minimumGroundNormalY = 0.65f;
   protected Vector2 velocity; // Child classes can access velocity but other classes can not 
   protected Vector2 realVelocity;
   public float gravityCoefficient = 1f;
   protected bool grounded;
   protected Vector2 groundNormal; 
   protected Rigidbody2D rigid2d;
   protected ContactFilter2D contFilter;
   protected const float minimumMomentumDistance = 0.001f;
   protected const float hitboxRadius = 0.01f;
   protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
   protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
   protected int featherUse; // to count how many times the player has jumped

    void OnEnable()
    {
        rigid2d = GetComponent<Rigidbody2D>();
    }
    
   
    // Start is called before the first frame update
    void Start()
    {
      contFilter.useTriggers = false;
      contFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); // Tells Unity to use the Physics2D system to dictate what layers collide 
      contFilter.useLayerMask = true; 
    }

    // Update is called once per frame
    void Update()
    {
        realVelocity = Vector2.zero;
        CalculateVelocity();
        if(grounded == true)
        {
            featherUse = 0;
        }

    }

    protected virtual void CalculateVelocity()
    {

    }

    void FixedUpdate()
   {
      velocity += gravityCoefficient * Physics2D.gravity * Time.deltaTime; // Constant velocity gravity because Unity's built in gravity is realistic and doesn't fit the platforming game genre
      velocity.x = realVelocity.x;
      grounded = false;
      Vector2 changePos = velocity * Time.deltaTime;
      Vector2 moveOnGround = new Vector2(groundNormal.y, -groundNormal.x); // Vector along the ground that is perpendicular to groundnormal
      Vector2 momentum = moveOnGround * changePos.x;
      Movement(momentum, false);
      momentum = changePos.y * Vector2.up;
      Movement(momentum, true); // Dictates how much the rigid body should move
   }
    
   void Movement(Vector2 momentum, bool yMovement)
   {
        float distance = momentum.magnitude;
        if(distance > minimumMomentumDistance)
         {
           int count = rigid2d.Cast(momentum, contFilter, hitBuffer, distance + hitboxRadius); 
           hitBufferList.Clear();
           for(int n = 0; n < count; n++)
            {
                hitBufferList.Add(hitBuffer[n]);  
            }
           for(int n = 0; n < hitBufferList.Count; n++)
            {
                Vector2 currentNormal = hitBufferList [n].normal;
                if(currentNormal.y > minimumGroundNormalY)
                 {
                    grounded = true;
                    if (yMovement == true)
                    {
                        groundNormal = currentNormal; // Vector that is perpendicular to the ground
                        currentNormal.x = 0;
                    }
                 }
                float projection = Vector2.Dot(velocity, currentNormal); 
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[n].distance - hitboxRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance; 
            }
         }
        rigid2d.position = rigid2d.position + momentum.normalized * distance; // Moves the rigid body based on momentum(constant velocity gravity) and the X/Y position 
   }


}

