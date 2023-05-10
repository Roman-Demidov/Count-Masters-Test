using DG.Tweening;
using UnityEngine;

namespace countMastersTest.ui.tutorial
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private GameObject _tutorialPanel;
        [SerializeField] private Transform _from;
        [SerializeField] private Transform _to;
        [SerializeField] private Transform _hand;
        [SerializeField] private float _duration;

        public void show()
        {
            _tutorialPanel.SetActive(true);
            _hand.transform.position = _from.position;
            _hand.DOMoveX(_to.position.x, _duration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void hide()
        {
            _tutorialPanel.SetActive(false);
            _hand.DOKill();
        }
    }
}
