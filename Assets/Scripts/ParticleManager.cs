using UnityEngine;

public class ParticleManager : MonoBehaviour {
    public static ParticleManager instance{get; private set;}
    
    [SerializeField] GameObject explosion;

    void Awake(){
        if(instance == null) instance = this;
        else{
            Destroy(gameObject);
            return;
        }
    }

    public void StartExplosion(Vector3 position){
        Destroy(Instantiate(explosion, position, Quaternion.identity), 0.5f);
    }
}
