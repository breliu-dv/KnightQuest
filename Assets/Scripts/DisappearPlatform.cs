using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DisappearPlatform : MonoBehaviour
{
    [Header("Timing Parameters")]
    public float mTimePerCycle;
    public float mIdleTime;
    public float mThresholdRigid = 0.3f;
    public float mStartOffset = 1.0f; 

    [Header("Coroutine Timing Parameters")]
    public float mTimeBetweenCalls = 0.05f;

    // Just change the object. 
    // private SpriteRenderer _mTilemap;
    private Tilemap _mTilemap;
    // private BoxCollider2D _mBoxCollider;
    private TilemapCollider2D _mBoxCollider;
    // Start is called before the first frame update
    void Start()
    {
        _mTilemap = gameObject.GetComponent<Tilemap>();
        _mBoxCollider = gameObject.GetComponent<TilemapCollider2D>();
        StartCoroutine("StartStartOffset");
    }

    IEnumerator StartStartOffset() {
        yield return new WaitForSeconds(mStartOffset);
        yield return StartCoroutine("StartIdleBeforeDisappear");
    }


    IEnumerator StartDisappear() {
        Color col = _mTilemap.color;
        for (float elapsedTime = 0f; elapsedTime < mTimePerCycle; elapsedTime += mTimeBetweenCalls) {
            col.a = 1 - elapsedTime / mTimePerCycle;
            _mTilemap.color = col;
            if (_mTilemap.color.a < mThresholdRigid && _mBoxCollider.enabled) {
                _mBoxCollider.enabled = false;
            }

            yield return new WaitForSeconds(mTimeBetweenCalls);
        }

        col.a = 0.0f;
        _mTilemap.color = col;

        yield return StartCoroutine("StartIdleBeforeAppear");
    }

    IEnumerator StartAppear() {
        Color col = _mTilemap .color;
        for (float elapsedTime = 0f; elapsedTime < mTimePerCycle; elapsedTime += mTimeBetweenCalls) {
            col.a = elapsedTime / mTimePerCycle;
            _mTilemap .color = col;
            if (_mTilemap.color.a < mThresholdRigid && !_mBoxCollider.enabled) {
                _mBoxCollider.enabled = true;
            }
            

            yield return new WaitForSeconds(mTimeBetweenCalls);
        }

        col.a = 1.0f;
        _mTilemap.color = col;
        
        yield return StartCoroutine("StartIdleBeforeDisappear");
    }

    IEnumerator StartIdleBeforeDisappear() {
        for (float elapsedTime = 0f; elapsedTime < mIdleTime; elapsedTime += mTimeBetweenCalls) {
            yield return new WaitForSeconds(mTimeBetweenCalls);
        }

        yield return StartCoroutine("StartDisappear");
    }

    IEnumerator StartIdleBeforeAppear() {
        for (float elapsedTime = 0f; elapsedTime < mIdleTime; elapsedTime += mTimeBetweenCalls) {
            yield return new WaitForSeconds(mTimeBetweenCalls);
        }

        yield return StartCoroutine("StartAppear");
    }
}
