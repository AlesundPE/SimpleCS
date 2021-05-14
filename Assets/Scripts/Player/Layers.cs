using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Layers : MonoBehaviourPunCallbacks
{
    public GameObject model;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine){
            changeLayer(model.transform, 8);
        }
    }

    private void changeLayer(Transform trans, int layer){
        trans.gameObject.layer = layer;
        foreach(Transform t in trans) changeLayer(t, layer);
    }
}
