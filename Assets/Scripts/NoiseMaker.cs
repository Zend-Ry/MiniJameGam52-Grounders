using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NoiseMaker : MonoBehaviour
{
    [SerializeField] AudioSource source;
    private void Awake()
    {
        if (!source)
            source = GetComponent<AudioSource>();
    }
    public void CreateNoise() 
    {
        source.Play();
        SignalManager.MakeNoise.Emit();
    }
}
