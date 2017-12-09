using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    private static bool created=false;

    void Awake()
    {
        if (created == false)
        {
            created = true;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
