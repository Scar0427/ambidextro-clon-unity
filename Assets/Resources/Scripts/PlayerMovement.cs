using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;//Rigidbody del jugador
    [SerializeField]
    Animator animator;//Animator del personaje
    bool canJump = false;//Indica si puede o no saltar el personaje
    public bool isSecondPlayer = false;//Indica si se trata del segundo personaje o no
    float movementDirection = 0;//Vector que indica la direccióm del movimieno, siendo -1 la izquierda y 1 la derecha.
    public int horizontalVelocity = 40, jumpForce = 50;//Variables que indican la velocidad del movimiento y la fuerza del salto.

    void Start()
    {
        //Esto es solamente para ejecutar de forma correcta las animaciones, ya que ambos jugadores usan un mismo objeto como base.
        animator.SetBool("IsPlayerSecond", isSecondPlayer);
    }

    //Método usado para conocer si el jugador entró en contacto con una colisión
    private void OnCollisionEnter2D(Collision2D other) {
        //Si el jugador entra en contacto con el suelo puede saltar.
        if (other.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    //Método utilizado para saber si el jugador salió de una colisión
    void OnCollisionExit2D(Collision2D collision)
    {
        //Si el jugador no está en contacto con el suelo, no podrá saltar.
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }        
    }


    /// <summary>
    /// Función utilizada para el salto del jugador.
    /// </summary>
    public void OnJump(){
        //Validando que se pueda saltar.
        if (canJump)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            canJump = false;
        }
    }

    //Función de movimiento del jugador usada por el InputSystem de Unity
    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        movementDirection = inputVector.x;    
    }

    //Se ejecuta cada cierta cantidad de frames fija, por lo que no es afectada por los FPS del juego.
    void FixedUpdate()
    {
        //Estos condicionales son para el InputManager de Unity, para saber la dirección del movimiento del jugador.
        //Además, ejecutan el salto para este sistema de entrada.
        if(!isSecondPlayer){
            movementDirection = Input.GetAxis("Player1-Movement");
            if(Input.GetKey(KeyCode.W))
                OnJump();
        }else{
            movementDirection = Input.GetAxis("Player2-Movement");
            if(Input.GetKey(KeyCode.UpArrow))
                OnJump();
        }
        
        //Mueve al jugador.
        transform.Translate(new Vector2(movementDirection, 0) * Time.deltaTime * horizontalVelocity);
    }
}
