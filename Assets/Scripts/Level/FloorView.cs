using System;
using System.Collections;
using Config;
using UnityEngine;
using Zenject;

namespace Level
{
    public class FloorView : MonoBehaviour
    {
        private float _movespeed;
        private IEnumerator _movingRoutine;
        private Vector2 _finishPosition;

        [Inject]
        public void Construct(ConstructOptions options, GameConfig config)
        {
            _movespeed = config.GameSpeed;
            transform.position = options.Position;
            transform.localScale = options.Size;
            tag = options.Tag;
            _movingRoutine = Move();
            StartCoroutine(_movingRoutine);
        }


        IEnumerator Move()
        {
            while ((Vector2) transform.position != _finishPosition)
            {
                Vector2.MoveTowards(
                    transform.position,
                    _finishPosition, 
                    _movespeed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
        }
    

        public class Factory : PlaceholderFactory<ConstructOptions, FloorView>
        {
            
        }

        public class ConstructOptions
        {
            public Vector2 Position { get; private set; }
            public Vector2 Size { get; private set; }
            public string Tag { get; private set; }

            public ConstructOptions(Vector2 position, Vector2 size, string tag)
            {
                Position = position;
                Size = size;
                Tag = tag;  
            }
        }
    }
}