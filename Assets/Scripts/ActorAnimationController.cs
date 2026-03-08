using System.Collections.Generic;
using UnityEngine;

public class ActorAnimationController : MonoBehaviour
{
    [SerializeField] private List<Sprite> upwardsSprites = new List<Sprite>(3);
    [SerializeField] private List<Sprite> downwardsSprites = new List<Sprite>(3);
    [SerializeField] private List<Sprite> sidewaySprites = new List<Sprite>(2);
    
}
