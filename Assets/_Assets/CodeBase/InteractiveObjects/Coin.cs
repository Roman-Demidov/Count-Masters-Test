using DG.Tweening;
using UnityEngine;

namespace countMastersTest
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private float _duration;

        private void Start()
        {
            transform.DORotate(Vector3.up * 360, _duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }

        public void hit()
        {
            Destroy(gameObject);
        }
    }
}
