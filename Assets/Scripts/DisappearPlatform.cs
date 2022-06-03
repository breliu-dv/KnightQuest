using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DisappearPlatform : MonoBehaviour
{
    [Header("Timing Parameters")]
    private float mTimePerCycle;
    private float mIdleTime;
    public float mThresholdRigid = 0.3f;
    public float mStartOffset = 1.0f; 
    public float minCycleTime;
    public float maxCycleTime;
    public float minIdleTime;
    public float maxIdleTime;
    public float mTimeBetweenCalls = 0.05f;

    private Tilemap _mTilemap;
    private TilemapCollider2D _mBoxCollider;
    private bool startedSequence;

    // Start is called before the first frame update
    void Start()
    {
        mTimePerCycle = Random.Range(minCycleTime, maxCycleTime);
        mIdleTime = Random.Range(minIdleTime, maxIdleTime);
        startedSequence = false;
        _mTilemap = gameObject.GetComponent<Tilemap>();
        _mBoxCollider = gameObject.GetComponent<TilemapCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject != null && !startedSequence)
        {
            startedSequence = true;
            Debug.Log("HingeJoint");
            StartCoroutine("StartStartOffset");
        }
    }

    IEnumerator StartStartOffset() 
    {
        yield return new WaitForSeconds(mStartOffset);
        yield return StartCoroutine("StartIdleBeforeDisappear");
    }


    IEnumerator StartDisappear() 
    {
        Color col = _mTilemap.color;

        for (float elapsedTime = 0f; elapsedTime < mTimePerCycle; elapsedTime += mTimeBetweenCalls) 
        {
            col.a = 1 - elapsedTime / mTimePerCycle;
            _mTilemap.color = col;
            
            if (_mTilemap.color.a < mThresholdRigid && _mBoxCollider.enabled) 
            {
                _mBoxCollider.enabled = false;
            }

            yield return new WaitForSeconds(mTimeBetweenCalls);
        }

        col.a = 0.0f;
        _mTilemap.color = col;

        yield return StartCoroutine("StartIdleBeforeAppear");
    }

    IEnumerator StartAppear() 
    {
        Color col = _mTilemap .color;

        for (float elapsedTime = 0f; elapsedTime < mTimePerCycle; elapsedTime += mTimeBetweenCalls) 
        {
            col.a = elapsedTime / mTimePerCycle;
            _mTilemap .color = col;

            if (_mTilemap.color.a < mThresholdRigid && !_mBoxCollider.enabled) 
            {
                _mBoxCollider.enabled = true;
            }
            
            yield return new WaitForSeconds(mTimeBetweenCalls);
        }

        col.a = 1.0f;
        _mTilemap.color = col;
        
        yield return StartCoroutine("StartIdleBeforeDisappear");
    }

    IEnumerator StartIdleBeforeDisappear() 
    {
        for (float elapsedTime = 0f; elapsedTime < mIdleTime; elapsedTime += mTimeBetweenCalls) 
        {
            yield return new WaitForSeconds(mTimeBetweenCalls);
        }

        yield return StartCoroutine("StartDisappear");
    }

    IEnumerator StartIdleBeforeAppear() 
    {
        for (float elapsedTime = 0f; elapsedTime < mIdleTime; elapsedTime += mTimeBetweenCalls) 
        {
            yield return new WaitForSeconds(mTimeBetweenCalls);
        }

        yield return StartCoroutine("StartAppear");
    }
}
