using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace countMastersTest
{
    public class InfoHub : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private Transform _pfCoinUi;

        public ObjectPool<Transform> _coinPool;

        private void Start()
        {
            initCoinPool();
        }

        private void initCoinPool()
        {
            _coinPool = new ObjectPool<Transform>(
                () => Instantiate(_pfCoinUi),
                pf => pf.gameObject.SetActive(true),
                pf => pf.gameObject.SetActive(false),
                pf => Destroy(pf.gameObject),
                true, 10, 100);
        }

        public void addCoint(int coin, Transform coinPlaseOnScene)
        {
            Transform coinUi = _coinPool.Get();
            coinUi.SetParent(transform);
            coinUi.position = Camera.main.WorldToScreenPoint(coinPlaseOnScene.position);
            coinUi.DOMove(_coinText.transform.position, 1f).OnComplete(() => {
                _coinPool.Release(coinUi);
                _coinText.text = coin.ToString();
            });
        }

        public void show(string level, int coin)
        {
            gameObject.SetActive(true);
            _levelText.text = level;
            _coinText.text = coin.ToString();
        }

        public void hide()
        {
            gameObject.SetActive(false);
        }
    }
}
