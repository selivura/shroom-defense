using UnityEngine;
using UnityEngine.Events;

namespace ShroomDefense.EventChannels
{
    public interface IEventListener<T>
    {
        public void Raise(T value);
    }
    public abstract class EventListener<T> : MonoBehaviour, IEventListener<T>
    {
        [SerializeField] EventChannel<T> _channel;
        public UnityEvent<T> UnityEvent;

        private void Awake()
        {
            _channel.Register(this);
        }
        private void OnDestroy()
        {
            _channel.Unegister(this);
        }
        public void Raise(T value)
        {
            UnityEvent?.Invoke(value);
        }
    }
    public class EventListener : EventListener<Empty> { }
}
