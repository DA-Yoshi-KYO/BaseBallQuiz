using Unity.VisualScripting;
using UnityEngine;

public class Easing : MonoBehaviour
{
    /// <summary>
    /// ���������^�����s���܂�
    /// </summary>
    /// <param name="time">timeDeltaTime�̒l</param>
    /// <param name="timeMax">�C�[�W���O�ɂ����鎞��(�b)</param>
    /// <returns>�C�[�W���O�̐i�s�x(0.0f ~ 1.0f)</returns>
    public static float Linear(float time, float timeMax = 1.0f)
    {
        if (timeMax <= 0.0f) return 0.0f;
        float t = time / timeMax;
        if (t >= 1.0f) t = 1.0f;
     
        return t;
    }

    /// <summary>
    /// �ɂ₩�ɕω�����C�[�Y�C�����s���܂�
    /// �Q�l�Fhttps://easings.net/ja#easeInSine
    /// </summary>
    /// <param name="time">timeDeltaTime�̒l</param>
    /// <param name="timeMax">�C�[�W���O�ɂ����鎞��(�b)</param>
    /// <returns>�C�[�W���O�̐i�s�x(0.0f ~ 1.0f)</returns>
    public static float EaseInSine(float time,float timeMax = 1.0f)
    {
        if (timeMax <= 0.0f)return 0.0f;
        float t = time / timeMax;
        if (t >= 1.0f) t = 1.0f;

        return 1 - Mathf.Cos(t * Mathf.PI / 2);
    }

    /// <summary>
    /// �}���ɕω�����C�[�Y�C�����s���܂�
    /// �Q�l�Fhttps://easings.net/ja#easeEaseInCubic
    /// </summary>
    /// <param name="time">timeDeltaTime�̒l</param>
    /// <param name="timeMax">�C�[�W���O�ɂ����鎞��(�b)</param>
    /// <returns>�C�[�W���O�̐i�s�x(0.0f ~ 1.0f)</returns>
    public static float EaseInCubic(float time, float timeMax = 1.0f)
    {
        if (timeMax <= 0.0f) return 0.0f;
        float t = time / timeMax;
        if (t >= 1.0f) t = 1.0f;

        return Mathf.Pow(t, 3);
    }

    /// <summary>
    /// �X�ɋ}���ɕω�����C�[�Y�C�����s���܂�
    /// �Q�l�Fhttps://easings.net/ja#easeEaseInQuint
    /// </summary>
    /// <param name="time">timeDeltaTime�̒l</param>
    /// <param name="timeMax">�C�[�W���O�ɂ����鎞��(�b)</param>
    /// <returns>�C�[�W���O�̐i�s�x(0.0f ~ 1.0f)</returns>
    public static float EaseInQuint(float time, float timeMax = 1.0f)
    {
        if (timeMax <= 0.0f) return 0.0f;
        float t = time / timeMax;
        if (t >= 1.0f) t = 1.0f;

        return Mathf.Pow(t, 5);
    }

    /// <summary>
    /// �ɂ₩�ɕω�����C�[�Y�C���A�E�g���s���܂�
    /// �Q�l�Fhttps://easings.net/ja#easeInOutSine
    /// </summary>
    /// <param name="time">timeDeltaTime�̒l</param>
    /// <param name="timeMax">�C�[�W���O�ɂ����鎞��(�b)</param>
    /// <returns>�C�[�W���O�̐i�s�x(0.0f ~ 1.0f)</returns>
    public static float EaseInOutSine(float time, float timeMax = 1.0f)
    {
        if (timeMax <= 0.0f) return 0.0f;
        float t = time / timeMax;
        if (t >= 1.0f) t = 1.0f;

        return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
    }
    
    /// <summary>
    /// �}���ɕω�����C�[�Y�C���A�E�g���s���܂�
    /// �Q�l�Fhttps://easings.net/ja#easeInOutCubic
    /// </summary>
    /// <param name="time">timeDeltaTime�̒l</param>
    /// <param name="timeMax">�C�[�W���O�ɂ����鎞��(�b)</param>
    /// <returns>�C�[�W���O�̐i�s�x(0.0f ~ 1.0f)</returns>
    public static float EaseInOutCubic(float time, float timeMax = 1.0f)
    {
        if (timeMax <= 0.0f) return 0.0f;
        float t = time / timeMax;
        if (t >= 1.0f) t = 1.0f;

        return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
    }

    /// <summary>
    /// �X�ɋ}���ɕω�����C�[�Y�A�E�g���s���܂�
    /// �Q�l�Fhttps://easings.net/ja#easeEaseOutQuint
    /// </summary>
    /// <param name="time">timeDeltaTime�̒l</param>
    /// <param name="timeMax">�C�[�W���O�ɂ����鎞��(�b)</param>
    /// <returns>�C�[�W���O�̐i�s�x(0.0f ~ 1.0f)</returns>
    public static float EaseOutQuint(float time, float timeMax = 1.0f)
    {
        if (timeMax <= 0.0f) return 0.0f;
        float t = time / timeMax;
        if (t >= 1.0f) t = 1.0f;

        return 1 - Mathf.Pow(1 - t, 5);
    }

    /// <summary>
    /// 1���z���Ă���߂��Ă���C�[�Y�A�E�g�o�b�N���s���܂�
    /// �Q�l�Fhttps://easings.net/ja#easeOutBack
    /// </summary>
    /// <param name="time">timeDeltaTime�̒l</param>
    /// <param name="timeMax">�C�[�W���O�ɂ����鎞��(�b)</param>
    /// <returns>�C�[�W���O�̐i�s�x(0.0f ~ 1.0f)</returns>
    public static float EaseOutBack(float time, float timeMax = 1.0f)
    {
        const float c1 = 5;
        const float c3 = c1 + 1;
        
        if (timeMax <= 0.0f) return 0.0f;
        float t = time / timeMax;
        if (t >= 1.0f) t = 1.0f;

        return 1 + c3 * Mathf.Pow(time - 1, 3) + c1 * Mathf.Pow(time - 1, 2);
    }

    /// <summary>
    /// 0��������Ă���߂��Ă���C�[�Y�C���o�b�N���s���܂�
    /// �Q�l�Fhttps://easings.net/ja#easeOutBack
    /// </summary>
    /// <param name="time">timeDeltaTime�̒l</param>
    /// <param name="timeMax">�C�[�W���O�ɂ����鎞��(�b)</param>
    /// <returns>�C�[�W���O�̐i�s�x(0.0f ~ 1.0f)</returns>
    public static float EaseInBack(float time, float timeMax = 1.0f)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        if (timeMax <= 0.0f) return 0.0f;
        float t = time / timeMax;
        if (t >= 1.0f) t = 1.0f;
        float outValue = c3 * t * t * t - c1 * t * t;

        return outValue;
    }

public static float Helmite(float time, float x0, float x1, float v0, float v1, float timeMax = 1.0f)
    {
        float t = time / timeMax;
        float outValue = Mathf.Pow(t - 1, 2) * (2 * t + 1) * x0 + Mathf.Pow(t, 2) * (3 - 2 * t) * x1 + Mathf.Pow(1 - t, 2) * t * v0 + (t - 1) * Mathf.Pow(t, 2) * v1;

        return outValue;
    }
}
