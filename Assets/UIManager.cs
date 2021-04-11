using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager singleton;
    private Animation uiAnimationReference;
    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        uiAnimationReference = GameObject.Find("Canvas").GetComponent<Animation>();
    }

    public void ListAnims() {
        print("Listing");
        foreach(AnimationState an in uiAnimationReference) {
            print(an.name);
        }
    }

    public void PlayAnimation(string _clip) {
        uiAnimationReference.Play(_clip);
    }

    
}
