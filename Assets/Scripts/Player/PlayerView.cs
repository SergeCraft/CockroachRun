using System;
using System.Collections;
using Main;
using UnityEngine;
using UnityEngine.Animations;
using Zenject;

namespace Player
{
    public class PlayerView : MonoBehaviour, IDisposable
    {
        private SignalBus _signalBus;
        private float _fallTime = 1.0f;
        private IEnumerator _actualMove;
        private float _radius;
        private PositionConstraint _pc;
        

        public AnimationCurve moveCurve;

        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _radius = GetComponent<CircleCollider2D>().radius;
            ConstraintSource cs = new ConstraintSource();
            cs.sourceTransform = GameObject.Find("SceneContext").transform;
            cs.weight = 1;
            _pc = GetComponent<PositionConstraint>();
            _pc.translationAxis = Axis.Y;
            _pc.AddSource(cs);

            SubscribeToSignalBus();

        }

        public void Dispose()
        {
            UnsubscribeFromSignalBus();
        }
        
        
        private void SetDefaults()
        {
            transform.position = new Vector2(-1.3f, 0.0f);
            _actualMove = Move(new Vector2(transform.position.x, -7.0f));
            StartCoroutine(_actualMove);
        }

        private void SubscribeToSignalBus()
        {
            _signalBus.Subscribe<PlayerStateChangedSignal>(OnPlayerStateChanged);
        }

        private void UnsubscribeFromSignalBus()
        {
            
            _signalBus.Unsubscribe<PlayerStateChangedSignal>(OnPlayerStateChanged);
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
        
        
        private void OnPlayerStateChanged(PlayerStateChangedSignal args)
        {
            switch (args.State)
            {
                case PlayerStates.MovingToTop:
                    _actualMove = Move(new Vector2(transform.position.x, 7.0f));
                    StartCoroutine(_actualMove);
                    break;
                case PlayerStates.MovingToBottom:
                    _actualMove = Move(new Vector2(transform.position.x, -7.0f));
                    StartCoroutine(_actualMove);
                    break;
                case PlayerStates.Frozen:
                    _pc.translationOffset = transform.position;
                    _pc.constraintActive = true;
                    break;
                case PlayerStates.Unfrozen:
                    _pc.constraintActive = false;
                    break;
                case PlayerStates.Restarting:
                    SetDefaults();
                    break;
                default:
                    Debug.Log($"Player view received unhandled state {args.State}");
                    break;
            }
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log($"Player hit {col.gameObject.name}");
            StopCoroutine(_actualMove);
            transform.position = new Vector2(
                transform.position.x,
                col.gameObject.CompareTag("TopFloor")
                    ? (col.bounds.min.y - _radius * transform.localScale.y * 0.8f)
                    : (col.bounds.max.y + _radius * transform.localScale.y * 0.8f));
            _signalBus.Fire(new PlayerHitSignal(col.gameObject));
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            Debug.Log($"Player left {col.gameObject.name}");
            _signalBus.Fire(new PlayerLeftSignal(col.gameObject));
        }
    }
}