using System;
using System.Collections;
using Main;
using UnityEditor.Tilemaps;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerView : MonoBehaviour, IDisposable
    {
        private SignalBus _signalBus;
        private float _fallTime = 1.0f;
        private IEnumerator _actualMove;
        

        public AnimationCurve moveCurve;

        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;

            SubscribeToSignalBus();
            SetDefaults();

        }

        public void Dispose()
        {
            UnsubscribeFromSignalBus();
        }
        
        
        private void SetDefaults()
        {
            transform.position = new Vector2(-1.3f, -1.5f);
        }

        private void SubscribeToSignalBus()
        {
            _signalBus.Subscribe<PlayerMoveToBottomSignal>(OnPlayerMoveToBottom);
            _signalBus.Subscribe<PlayerMoveToTopSignal>(OnPlayerMoveToTop);
        }

        private void UnsubscribeFromSignalBus()
        {
            
            _signalBus.Unsubscribe<PlayerMoveToBottomSignal>(OnPlayerMoveToBottom);
            _signalBus.Unsubscribe<PlayerMoveToTopSignal>(OnPlayerMoveToTop);
        }

        private IEnumerator Move(Vector2 targetPosition)
        {
            Vector2 srcPosition = transform.position;
            float elapsedTime = 0;
            while ((Vector2)transform.position != 
                   targetPosition)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector2.Lerp(
                    srcPosition, 
                    targetPosition, 
                    moveCurve.Evaluate(elapsedTime / _fallTime));
                yield return new WaitForFixedUpdate();
            }
        }
        
        
        private void OnPlayerMoveToBottom()
        {
            Debug.Log("Player moving to bottom...");
            _actualMove = Move(new Vector2(transform.position.x, -5.0f));
            StartCoroutine(_actualMove);
        }
        
        private void OnPlayerMoveToTop()
        {
            Debug.Log("Player moving to top...");
            _actualMove = Move(new Vector2(transform.position.x, 5.0f));
            StartCoroutine(_actualMove);
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log($"Player hit {col.gameObject.name}");
            StopCoroutine(_actualMove);
        }
    }
}