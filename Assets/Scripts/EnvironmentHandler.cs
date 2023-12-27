using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHandler: MonoBehaviour {
    
    [SerializeField] GameObject background;
    [SerializeField] float size;
    [SerializeField] Transform player;

    public GameObject[] objects;
    
    int viewDist;
    int curBGCoordY;

    Dictionary<float, GameObject> bgInView = new Dictionary<float, GameObject>();
    
    List<float> _temp = new List<float>();

    void Start(){
        viewDist = Mathf.RoundToInt(Camera.main.orthographicSize/4f);
    }

    void Update(){
        curBGCoordY = Mathf.RoundToInt(player.position.y / size);
        
        foreach(var i in bgInView) if(Mathf.Abs(i.Key - curBGCoordY) > viewDist) _temp.Add(i.Key);
        foreach(var i in _temp) {
            Destroy(bgInView[i]);
            bgInView.Remove(i);
        }
        _temp.Clear();
        
        Vector3 coord = Vector3.zero;
        for(int y = 0; y < viewDist; y++){
            float _y = y+curBGCoordY;
            if(!bgInView.ContainsKey(_y)){
                coord.Set(0f, _y * size, 1f);
                bgInView.Add(_y, Instantiate(background, coord, Quaternion.identity, transform));
            }
        }
    }
}
