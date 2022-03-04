using System;
using System.Collections;
using Config;
using Main;
using UnityEngine;
using Zenject;

namespace Level
{
    public class FloorView : MonoBehaviour
    {
        private SignalBus _signalBus;
        private float _movespeed;
        private IEnumerator _movingRoutine;
        private Vector2 _finishPosition;

        [Inject]
        public void Construct(ConstructOptions options, GameConfig config, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _movespeed = config.GameSpeed;
            transform.position = options.Position;
            transform.localScale = options.Size;
            _finishPosition = new Vector2(-5 - options.Size.x, options.Position.y);
            tag = options.Tag;
            _movingRoutine = Move();
        }

        public void Freeze()
        {
            StopCoroutine(_movingRoutine);
        }
        
        public void Unfreeze()
        {
            StartCoroutine(_movingRoutine);
        }

        private void Awake()
        {
            StartCoroutine(_movingRoutine);
        }


        IEnumerator Move()
        {
            bool isReachedVisible = false;
            while ((Vector2) transform.position != _finishPosition)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    _finishPosition, 
                    _movespeed * Time.deltaTime);
                
                if (transform.position.x < 3.0f && !isReachedVisible)
                {
                    isReachedVisible = true;
                    _signalBus.Fire(new FloorReachedPositionSignal(this.gameObject, FloorPositionTypes.Visible));
                };
                yield return new WaitForFixedUpdate();
            }

            _signalBus.Fire(new FloorReachedPositionSignal(this.gameObject, FloorPositionTypes.Finish));
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