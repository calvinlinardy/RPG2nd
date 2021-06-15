using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    [SerializeField] string[] lines = null;

    private bool canActivate;
    public bool isPerson = true;

    public bool shouldActivateQuest;
    public string questToMark;
    public bool markComplete;
    public bool clickToActivate;
    public bool clickAfterTrigger;
    public bool destroyAfterLines;
    AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clickToActivate)
        {
            if (canActivate && Input.GetButtonDown("Fire1") && !DialogManager.instance.dialogBox.activeInHierarchy &&
            !GameMenu.instance.theMenu.activeInHierarchy && BattleManager.instance.battleActive == false)
            {
                DialogManager.instance.ShowDialog(lines, isPerson);
                DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                if (audioSrc)
                {
                    if (!audioSrc.isPlaying)
                    {
                        audioSrc.Play();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (clickToActivate)
        {
            if (other.tag == "Player")
            {
                canActivate = true;
            }
        }
        else
        {
            if (!DialogManager.instance.dialogBox.activeInHierarchy &&
                !GameMenu.instance.theMenu.activeInHierarchy && BattleManager.instance.battleActive == false)
            {
                if (other.tag == "Player")
                {
                    DialogManager.instance.ShowDialog(lines, isPerson);
                    DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                    if (audioSrc)
                    {
                        if (!audioSrc.isPlaying)
                        {
                            audioSrc.Play();
                        }
                    }
                    if (clickAfterTrigger)
                    {
                        clickToActivate = true;
                        clickAfterTrigger = false;
                    }
                    if (destroyAfterLines)
                    {
                        this.gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (clickToActivate)
        {
            if (other.tag == "Player")
            {
                canActivate = false;
            }
        }
    }
}
