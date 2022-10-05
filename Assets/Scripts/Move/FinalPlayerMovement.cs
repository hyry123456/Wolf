using UnityEngine;

public class FinalPlayerMovement : MonoBehaviour
{   
    protected Rigidbody2D Rb;
    protected Collider2D coll;
    protected Animator anim;

    public float speed = 10;
    public float jumpForce = 1000;

    public Transform GroundCheck;
    public LayerMask ground;

    public bool isGround;
    public bool isJump = false;

    public bool jumpPressed = false;
    public int jumpCount = 0;

    public bool isHurt;//Ä¬ÈÏÖµÊÇfalse
    public void GroundMovement() {

        float horizontalMove = Input.GetAxis("Horizontal");
        float facedircetion = Input.GetAxisRaw("Horizontal");
        Rb.velocity = new Vector2(horizontalMove * speed, Rb.velocity.y);
        anim.SetFloat("Running", Mathf.Abs(facedircetion));

        if (facedircetion != 0) {
            transform.localScale = new Vector3(facedircetion, 1, 1);
        }
    }

    public void Jump() {
        if (isGround) {
            jumpCount = 2;
            isJump = false;
        }
        if(jumpPressed && isGround) {
            isJump = true;
            Rb.velocity = new Vector2(Rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount > 0 && isJump) {
            Rb.velocity = new Vector2(Rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }

    }

    public void Crouch() {
        if(Input.GetButtonDown("Crouch")) {
            anim.SetBool("Crouching", true);
        }else if(Input.GetButtonDown("Crouch")) {
            anim.SetBool("Crouching", false);
        }
    }
    public void SwitchAnim() {
        anim.SetBool("Idle", true);
        anim.SetFloat("Running", Mathf.Abs(Rb.velocity.x));


        if (isHurt == true) {
            anim.SetBool("Hurt", true);
            if(Mathf.Abs(Rb.velocity.x) < 0.1f) {
                anim.SetBool("Hurt", false);
                anim.SetBool("Idle", true);
                isHurt = false;
            }
        }
        else if(isGround) {
            anim.SetBool("Falling", false);
            anim.SetBool("Idle", true);
        }
        else if (!isGround && Rb.velocity.y > 0) {
            anim.SetBool("Jumping", true);
        }
        else if (Rb.velocity.y < 0) {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", true);

        }
       
    }

}
