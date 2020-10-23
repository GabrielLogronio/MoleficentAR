using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerFollow : MonoBehaviour
{
    static UIPlayerFollow instance = null;

    [SerializeField]
    GameObject Players;

    Transform ToFollow;

    public static UIPlayerFollow getInstance()
    {
        return instance;
    }

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (ToFollow)
        {
            // PROBLEMA CON SIR LOIN <----------------------------------------------------------------------------------
            transform.position = ToFollow.position + Vector3.up * 0.1f;
            transform.Rotate(Vector3.up * 100f * Time.deltaTime);
        }

        MoveShadow();

    }

    public void SetPlayer(int Player)
    {
        if (Player >= 0 && Player <= 3)
        {
            ToFollow = Players.transform.GetChild(Player);
            transform.GetChild(Player).gameObject.SetActive(true);
        } 
    }

    void MoveShadow()
    {
        RaycastHit CameraHit;
        Ray CameraRay = new Ray(transform.position, Vector3.down * 60f);
        if (Physics.Raycast(CameraRay, out CameraHit, 60f))
        {
            transform.GetChild(4).position = new Vector3(transform.position.x, CameraHit.point.y + 0.1f, transform.position.z);
        }
    }

}
