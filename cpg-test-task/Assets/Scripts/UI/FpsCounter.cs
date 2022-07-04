using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField]
        private Text fpsCounterText;

        private void Update()
        {
            var fpsCount = (int) (1.0f / Time.unscaledDeltaTime);
            fpsCounterText.text = fpsCount.ToString();
        }
    }
}