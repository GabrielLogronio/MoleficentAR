using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    virtual protected void Start()
    {
        
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        
    }

    virtual public void Initiate() 
    { 
    
    }

    public void Deactivate(float Timer)
    {
        Invoke("Reactivate", Timer);
        gameObject.SetActive(false);

    }

    void Reactivate()
    {
        gameObject.SetActive(true);

    }
}
