using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_ANDROID
using Google.Play.Review;
#endif

public class CustomReviewManager : MonoBehaviour
{
    public static CustomReviewManager instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;        
    }

    public void StoreReview()
    {
        StartCoroutine(ReviewDelayCo());
    }

    private IEnumerator ReviewDelayCo()
    {
        yield return new WaitForSeconds(2f);  // Util.WaitGet(2f);
#if UNITY_ANDROID
        StartCoroutine(requireRate());
#elif UNITY_IOS
            UnityEngine.iOS.Device.RequestStoreReview();
#endif
    }

#if UNITY_ANDROID
    private IEnumerator requireRate()
    {
        // Create instance of ReviewManager
        ReviewManager _reviewManager;
        // ...
        _reviewManager = new ReviewManager();
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        PlayReviewInfo _playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }
#endif


}