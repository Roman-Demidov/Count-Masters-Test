using System;
using System.Collections.Generic;
using countMastersTest.infrastructure;
using countMastersTest.infrastructure.constants;
using countMastersTest.infrastructure.input;
using countMastersTest.interactiveObjects.gate;
using DG.Tweening;
using MyBox;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

namespace countMastersTest.character
{
    public class UserCharacter : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private TextMeshProUGUI _unitCountText;
        [SerializeField] private Transform _unitContainer;
        [SerializeField] private Unit _pfUnit;
        [SerializeField] private ParticleSystem _pfHitParticle;
        [SerializeField] private float _moveSpeedForward = 2;
        [SerializeField] private float _moveSpeedSide = 5;
        [SerializeField] private float _maxUnitCircleRadius = 4;
        [SerializeField, Min(1)] private int _startUnitCount = 1;

        public ObjectPool<Unit> _unitPool;
        public ObjectPool<ParticleSystem> _hitParticlePool;
        private List<Unit> _units;
        private Vector3 _moveDirection;
        private IInputService _input;

        public event Action onFinish;
        public event Action onDeath;
        public event Action<Transform> onPickedCoin;
        private bool _canMove;

        public void init(IInputService inputService)
        {
            _input = inputService;
            _canMove = false;
        }

        private void OnValidate()
        {
            if(_characterController == null) _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            initUnitPool();
            initHitParticlePool();

            initUnitList();
            _input.onSwipe += movement;

            addUnit(_startUnitCount);
        }

        private void OnDestroy()
        {
            _input.onSwipe -= movement;
        }

        private void initUnitPool()
        {
            _unitPool = new ObjectPool<Unit>(
                () => Instantiate(_pfUnit),
                pf => pf.gameObject.SetActive(true),
                pf => pf.gameObject.SetActive(false),
                pf => Destroy(pf.gameObject),
                true, 200, 400);
        }

        private void initHitParticlePool()
        {
            _hitParticlePool = new ObjectPool<ParticleSystem>(
                () => Instantiate(_pfHitParticle),
                pf => pf.gameObject.SetActive(true),
                pf => pf.gameObject.SetActive(false),
                pf => Destroy(pf.gameObject),
                true, 200, 400);
        }

        private void Update()
        {
            if (_canMove == false) return;

            _moveDirection =  Vector3.forward * (_moveSpeedForward * Time.deltaTime);

            _characterController.Move(_moveDirection);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Gates gates))
            {
                gateAction(gates);
            }
            else if (other.TryGetComponent(out Coin coin))
            {
                coin.hit();
                onPickedCoin?.Invoke(coin.transform);
            }
            else if (other.TryGetComponent(out Finish finish))
            {
                stopMove();
                onFinish?.Invoke();
            }
        }

        public void startMove()
        {
            _canMove = true;
        }

        public void stopMove()
        {
            _canMove = false;
        }

        private void movement(SwipeData swipe)
        {
            Vector3 moveDirection = Vector3.zero;

            float size = (GameConstants.ROAD_WIDTH / 2) - _characterController.radius;

            if (transform.position.x <= -size && swipe.direction.x < 0) return;
            if (transform.position.x >= size && swipe.direction.x > 0) return;

            moveDirection.x = swipe.direction.x * _moveSpeedSide * Time.deltaTime;
            _characterController.Move(moveDirection);
        }

        private void addUnit(int value)
        {
            for (int i = 0; i < value; i++)
            {
                var unit = _unitPool.Get();
                unit.transform.parent = _unitContainer;
                unit.onUnitHitObstacle += returnUnitToPool;
                _units.Add(unit);
            }

            _unitCountText.text = _units.Count.ToString();
            updateUnitPlacement();
        }

        private void returnUnitToPool(Unit unit)
        {
            var particle = _hitParticlePool.Get();
            particle.transform.position = unit.transform.position;
            particle.Play();
            DOTween.Sequence().AppendInterval(2f).OnComplete(() => _hitParticlePool.Release(particle));


            unit.onUnitHitObstacle -= returnUnitToPool;
            _units.Remove(unit);
            _unitPool.Release(unit);

            _unitCountText.text = _units.Count.ToString();

            if(_units.Count == 0)
            {
                onDeath?.Invoke();
                stopMove();
            }
        }

        private void updateUnitPlacement()
        {
            float radius = GameConstants.UNIT_RADIUS;
            int index = 0;
            int currentRing = 0;
            float trigerRadius = 0;

            while (index < _units.Count)
            {
                if (currentRing == 0)
                {
                    _units[index].transform.localPosition = Vector3.zero;
                    index++;
                    currentRing++;
                    continue;
                }

                float circumference = 2 * Mathf.PI * (currentRing * radius * 2);
                int objectsInRing = Mathf.FloorToInt(circumference / (radius * 2));
                float angleStep = 360f / objectsInRing;

                for (int i = 0; i < objectsInRing && index < _units.Count; i++)
                {
                    float angle = i * angleStep;
                    float x = (currentRing * radius * 2) * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float z = (currentRing * radius * 2) * Mathf.Sin(angle * Mathf.Deg2Rad);
                    Vector3 position = new Vector3(x, _units[index].transform.localPosition.y, z);

                    if (position.magnitude <= _maxUnitCircleRadius)
                    {
                        _units[index].transform.localPosition = position;
                        index++;
                    }
                }

                trigerRadius = currentRing * (radius * 2);
                currentRing++;
            }

            _characterController.radius = trigerRadius;
            startMoveUnits();
        }

        private void initUnitList()
        {
            if (_units != null && _units.Count > 0)
            {
                foreach (var unit in _units)
                {
                    _unitPool.Release(unit);
                }
            }

            _units = new List<Unit>();
        }

        private void startMoveUnits()
        {
            foreach (var unit in _units)
            {
                unit.playAnimation(AnimationType.Run);
            }
        }

        private void gateAction(Gates gates)
        {
            int unitsCount;
            if (transform.position.x < gates.transform.position.x)
            {
                unitsCount = getUnitsCount(gates.getLeftAction());
            }
            else
            {
                unitsCount = getUnitsCount(gates.getRightAction());
            }
            addUnit(unitsCount);
        }

        public int getUnitsCount(GateSettings gateSettings)
        {
            switch (gateSettings.action)
            {
                case GateActions.Add:
                    return gateSettings.value;
                case GateActions.Multiply:
                    return (_units.Count * gateSettings.value) - _units.Count;
            }

            return 0;
        }
    }
}
