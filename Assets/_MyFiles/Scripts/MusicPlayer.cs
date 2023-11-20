using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public TextMeshProUGUI CountDown;
    public GameObject CountDownOBJ;
    public TextMeshProUGUI CountDown2;
    public GameObject CountDown2OBJ;
    public AudioSource GetReady;
    public AudioSource GoAudio;
    public GameObject playerCanvas;
    public AudioClip[] clips;
    private AudioSource audioSource;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //audioSource.loop = false;
        //playerCanvas.SetActive(false);
        //StartCoroutine(CountStart());
    }

    private void Update()
    {
        //if (!audioSource.isPlaying)
        //{
        //    audioSource.clip = GetRandomClip();
        //    audioSource.Play();
        //}
    }

    IEnumerator CountStart()
    {
        yield return new WaitForSeconds(0.5f);
        CountDown.text = "3";
        CountDown2.text = "3";
        GetReady.Play();
        CountDownOBJ.SetActive(true);
        CountDown2OBJ.SetActive(true);

        yield return new WaitForSeconds(1);
        CountDownOBJ.SetActive(false);
        CountDown2OBJ.SetActive(false);
        CountDown.text = "2";
        CountDown2.text = "2";

        GetReady.Play();
        CountDownOBJ.SetActive(true);
        CountDown2OBJ.SetActive(true);


        yield return new WaitForSeconds(1);
        CountDownOBJ.SetActive(false);
        CountDown2OBJ.SetActive(false);

        CountDown.text = "1";
        CountDown2.text = "1";

        GetReady.Play();
        CountDownOBJ.SetActive(true);
        CountDown2OBJ.SetActive(true);


        yield return new WaitForSeconds(1);
        CountDownOBJ.SetActive(false);
        CountDown2OBJ.SetActive(false);

        CountDown.text = "GO";
        GoAudio.Play();
        CountDownOBJ.SetActive(true);
        playerCanvas.SetActive(true);


        yield return new WaitForSeconds(.5f);
        CountDownOBJ.SetActive(false);
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
