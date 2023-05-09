using System;
using System.Collections.Generic;
using System.Xml.Linq;
using countMastersTest.constants;
using countMastersTest.infrastructure;
using countMastersTest.infrastructure.input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace countMastersTest.character
{
    public class UserCharacter : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _unitContainer;
        [SerializeField] private Unit _pfUnit;
        [SerializeField] private float _moveSpeedForward = 2;
        [SerializeField] private float _moveSpeedSide = 5;
        [SerializeField] private float _maxUnitCircleRadius = 4;
        [SerializeField, Min(1)] private int _startUnitCount = 1;

        private ObjectPool<Unit> _unitPool;
        private List<Unit> _units;
        private Game _game;
        private Vector3 _moveDirection;
        private IInputService _input;

        [Inject]
        private void constructor(Game game, IInputService inputService)
        {
            _game = game;
            _input = inputService;
        }

        private void OnValidate()
        {
            if(_characterController == null) _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            initPool();
            initUnitList();
            _input.onSwipe += movement;

            addUnit(_startUnitCount);
        }

        private void Update()
        {
            if (_game.isPause) return;

            _moveDirection =  Vector3.forward * (_moveSpeedForward * Time.deltaTime);

            _characterController.Move(_moveDirection);
        }

        private void initPool()
        {
            _unitPool = new ObjectPool<Unit>(
                () => Instantiate(_pfUnit, _unitContainer),
                pf => pf.gameObject.SetActive(true),
                pf => pf.gameObject.SetActive(false),
                pf => Destroy(pf.gameObject),
                true, 100, 400);
        }

        private void movement(SwipeData swipe)
        {
            Vector3 moveDirection = Vector3.zero;

            moveDirection.x = swipe.direction.x * _moveSpeedSide * Time.deltaTime;

            _characterController.Move(moveDirection);
        }

        private void addUnit(int value)
        {
            for (int i = 0; i < value; i++)
            {
                var unit = _unitPool.Get();
                _units.Add(unit);
            }

            updateUnitPlacement();
        }

        private void updateUnitPlacement()
        {
            float radius = _units[0].getRadius();
            int index = 0;
            int currentRing = 0;

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
                currentRing++;
            }

            startMove();
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

        private void startMove()
        {
            foreach (var unit in _units)
            {
                unit.playAnimation(AnimationType.Run);
            }
        }

        private void stopMove()
        {
            foreach (var unit in _units)
            {
                unit.playAnimation(AnimationType.Idle);
            }
        }
    }
}
