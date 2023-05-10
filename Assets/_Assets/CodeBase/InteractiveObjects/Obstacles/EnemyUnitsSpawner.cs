using System.Collections.Generic;
using countMastersTest.character;
using countMastersTest.infrastructure.constants;
using UnityEngine;
using UnityEngine.Pool;

namespace countMastersTest.interactiveObjects.obstacles
{
    public class EnemyUnitsSpawner : MonoBehaviour
    {
        [SerializeField] private int _unitCount;
        [SerializeField] private Unit _pfUnit;
        [SerializeField] private int _roadSize;

        private ObjectPool<Unit> _unitPool;
        private List<Unit> _units;

        private void Start()
        {
            _units = new List<Unit>();
            initPool();
            initUnits();
        }

        private void initPool()
        {
            _unitPool = new ObjectPool<Unit>(
                () => Instantiate(_pfUnit, transform),
                pf => pf.gameObject.SetActive(true),
                pf => pf.gameObject.SetActive(false),
                pf => Destroy(pf.gameObject),
                true, 100, 400);
        }

        private void initUnits()
        {
            for (int i = 0; i < _unitCount; i++)
            {
                var unit = _unitPool.Get();
                unit.onUnitHitObstacle += returnUnitToPool;
                _units.Add(unit);
            }

            //updateUnitPlacement();
            UpdateUnitPlacement();
        }

        private void returnUnitToPool(Unit unit)
        {
            unit.onUnitHitObstacle -= returnUnitToPool;
            _units.Remove(unit);
            _unitPool.Release(unit);
        }

        private void updateUnitPlacement()
        {
            float radius = GameConstants.UNIT_RADIUS;
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

                    if (position.magnitude <= _roadSize/2)
                    {
                        _units[index].transform.localPosition = position;
                        index++;
                    }
                }
                currentRing++;
            }
        }

        private void UpdateUnitPlacement()
        {
            float radius = GameConstants.UNIT_RADIUS;
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

                    if (position.magnitude <= _roadSize / 2)
                    {
                        _units[index].transform.localPosition = position;
                        index++;
                    }
                    else
                    {
                        // Adjust the angle step to fit all objects within the maximum radius
                        while (position.magnitude > _roadSize / 2)
                        {
                            angleStep *= 0.99f;
                            angle = i * angleStep;
                            x = (currentRing * radius * 2) * Mathf.Cos(angle * Mathf.Deg2Rad);
                            z = (currentRing * radius * 2) * Mathf.Sin(angle * Mathf.Deg2Rad);
                            position = new Vector3(x, _units[index].transform.localPosition.y, z);
                        }

                        _units[index].transform.localPosition = position;
                        index++;
                    }
                }
                currentRing++;
            }
        }

    }


}
