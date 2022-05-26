using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Core
{
    public class BoxOverlapTester : MonoBehaviour
    {
        [SerializeField] private Vector3 colliderSize = new Vector3(1, 1, 1);
        [Space] [SerializeField] private LayerMask collideWith;
        [SerializeField] private int overlapBufferSize = 8;
        [SerializeField] private bool autoTest = true;
        [SerializeField] private Color debugColor = Color.green;

        private Collider[] _overlapBuffer;
        private int _overlapCount;

        public event OverlapCallback Overlap;

        private void Awake()
        {
            _overlapBuffer = new Collider[overlapBufferSize];
        }

        private void FixedUpdate()
        {
            if (!autoTest)
            {
                return;
            }

            if (!TestForOverlap())
            {
                return;
            }

            var overlaps = GetLastSuccessfulTestOverlaps().ToArray();
            Overlap?.Invoke(overlaps);
        }

        private void OnDrawGizmos()
        {
            GizmosHelper.PushColor(debugColor);
            GizmosHelper.PushMatrix(transform.localToWorldMatrix);

            Gizmos.DrawWireCube(Vector3.zero, colliderSize);

            GizmosHelper.PopColor();
            GizmosHelper.PopMatrix();
        }

        public bool TestForOverlap()
        {
            var rotation = transform.rotation;
            var size = GetScaledSize();
            _overlapCount = Physics.OverlapBoxNonAlloc(transform.position,
                size, _overlapBuffer, rotation, collideWith);

            return _overlapCount > 0;
        }

        public IEnumerable<Collider> GetLastSuccessfulTestOverlaps()
        {
            return _overlapBuffer.Take(_overlapCount);
        }

        public Vector3 GetScaledSize()
        {
            var scale = transform.localScale;
            return new Vector3(colliderSize.x * scale.x, colliderSize.y * scale.y, colliderSize.z * scale.z);
        }
    }
}