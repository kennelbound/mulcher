using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BasicRenderer : MonoBehaviour
{
    private ParticleSystem _particleSystem;

	// Use this for initialization
	void Start ()
	{
	    _particleSystem = GetComponent<ParticleSystem>();
	}

    public void handleEvent(string type)
    {
        ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
        switch (type)
        {
            case "error":
                emitOverride.startColor = Color.red;
                break;
            case "info":
                emitOverride.startColor = Color.blue;
                break;
            default:
                break;
        }
        _particleSystem.Emit(emitOverride, 1);
    }
}
