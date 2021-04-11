using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] cosmetics;
    public static PlayerManager singleton;
    bool controllable = false;
    Vector3 turndir = Vector3.zero;
    [SerializeField] Animator animator;
    public float speed;
    private void Start() {
        singleton = this;
        int i = 0;
        foreach(string category in new string[] { "Mouth", "Eyes", "Nose" }) {
            AssignCosmetic(i, FirebaseDataHandler.singleton.GetCosmetic(category));
            i++;
        }
    }

    void AssignCosmetic(int num, string cosmeticName) {
        cosmetics[num].sprite = Resources.Load<Sprite>(cosmeticName);
    }

    public void SetControllable(bool newVal) {
        controllable = newVal;
    }

    private void Update() {
        if (controllable) {
            float moveHoriz = Input.GetAxisRaw("Horizontal");
            print(moveHoriz);
            if(moveHoriz < 0) {
                transform.forward = Vector3.left;
                animator.SetBool("moving", true);
            } else if (moveHoriz > 0) {
                transform.forward = Vector3.right;
                animator.SetBool("moving", true);
            } else {
                transform.forward = Vector3.back;
                animator.SetBool("moving", false);
            }
            transform.position += moveHoriz * speed * Vector3.right * Time.deltaTime;
        }
    }
}
