using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Transform camShakeTransform;
    private Coroutine shakeCor;

    public enum Strength
    {
        weakShake,
        mediumShake,
        strongShake
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<CameraShake>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        camShakeTransform = transform;
    }

    public void Shake(Strength shakeStrength)
    {
        StopShake();

        Vector2 shakeParam = GetDurMag(shakeStrength);

        shakeCor = StartCoroutine(ShakeCor(shakeParam.x, shakeParam.y));
    }

    public void Shake(float duration, float magnitude)
    {
        StopShake();

        Vector2 shakeParam = new Vector2(duration, magnitude);

        shakeCor = StartCoroutine(ShakeCor(shakeParam.x, shakeParam.y));
    }

    public void StopShake()
    {
        if (shakeCor != null)
        {
            StopCoroutine(shakeCor);
        }

        //Reset position
        camShakeTransform.localPosition = Vector2.zero;
    }

    private Vector2 GetDurMag(Strength shakeStrength)
    {
        switch (shakeStrength)
        {
            case Strength.weakShake:
                return new Vector2(0.15f, 0.04f);    //Configure values here
            case Strength.mediumShake:
                return new Vector2(0.36f, 0.06f);    //Configure values here
            case Strength.strongShake:
                return new Vector2(0.72f, 0.08f);    //Configure values here
            default:
                return new Vector2(0.10f, 0.02f);
        }
    }

    private IEnumerator ShakeCor(float duration, float magnitude)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            //Get randomOffset
            Vector2 randomOffset = Random.insideUnitCircle * magnitude;

            //Offset transform by random offset
            camShakeTransform.localPosition = randomOffset;

            //Increment time
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        //Reset position
        camShakeTransform.localPosition = Vector2.zero;
    }
}
