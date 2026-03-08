using UnityEngine;

public class Contestant : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] LayerMask groundLayer  = 0;
    [SerializeField] LayerMask environLayer = 0;
    LayerMask currentLayer = 0;
    void Start()
    {
        SignalManager.WhistleBlown.AddListener(Grounders);
        if (groundLayer == 0)
            groundLayer = LayerMask.NameToLayer("Ground");

        if (environLayer == 0)
            environLayer = LayerMask.NameToLayer("Environment");
    }

    void Grounders() 
    {
        if(currentLayer.value != groundLayer.value)
            return;

        // Play Freeze Animation
        // Freeze Animation disables gameObject at the end of animation
        // if the target is player, change to main menu scene

        var animationController = GetComponent<ActorAnimationController>();
        if (animationController == null)
            return;

        if (gameObject.tag == "Actor")
        {
            animationController.DieBitch();
        }
        else if (gameObject.tag == "Player") 
        {
            animationController.PlayerEgoDeath();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the layer bit values matches the environment layer bit values
        if (((1 << collision.gameObject.layer) & environLayer) == (1 << collision.gameObject.layer))
        {
            currentLayer = environLayer;
        }
        // if the layer bit values matches the ground layer bit values
        else if ( ( (1 << collision.gameObject.layer) & groundLayer) == (1 << collision.gameObject.layer) )
        {
            currentLayer = groundLayer;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (((1 << collision.gameObject.layer) & environLayer) == (1 << collision.gameObject.layer)) 
        {
            currentLayer = groundLayer;
        }
    }
}
