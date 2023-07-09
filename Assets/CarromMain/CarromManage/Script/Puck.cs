using System;
//using Photon.Pun;
using UnityEngine;

[Serializable]
public class Puck
{
    //public PhotonView photonView;

    public Rigidbody2D rigidbody2D;

    public PuckColor puckcolor;

    //public Puck(PhotonView photonView, Rigidbody2D rigidbody2D, PuckColor puckcolor)
    //{
    //    this.photonView = photonView;
    //    this.rigidbody2D = rigidbody2D;
    //    this.puckcolor = puckcolor;
    //}

    public Puck(Rigidbody2D rigidbody2D, PuckColor puckcolor)
    {
        this.rigidbody2D = rigidbody2D;
        this.puckcolor = puckcolor;
    }
}
