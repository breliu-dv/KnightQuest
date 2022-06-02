//  This class has been folded into the soundmanager in assets/scripts

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SoundManager : MonoBehaviour
// {
//     [SerializeField]
//     private SoundClip mJumpSound;
//     [SerializeField]
//     private SoundClip mHitSound;
//     [SerializeField]
//     private SoundClip mPickupSound;

//     [SerializeField]
//     private SoundClip mBGM;

//     [SerializeField]
//     private List<SoundClip> musicTracks;

//     private AudioSource _mAudioSource;

//     // Start is called before the first frame update
//     void Start()
//     {
//         _mAudioSource = gameObject.GetComponent<AudioSource>();
//         mBGM.audioSource = _mAudioSource;
//         mBGM.audioSource.play();

//     }

//     public void NotifyJump()
//     {
//         _mAudioSource.PlayOneShot(mJumpSound);
//     }

//     public void NotifyHit()
//     {
//         _mAudioSource.PlayOneShot(mHitSound);
//     }

//     public void NotifyPickup()
//     {
//         _mAudioSource.PlayOneShot(mPickupSound);
//     }
// }
