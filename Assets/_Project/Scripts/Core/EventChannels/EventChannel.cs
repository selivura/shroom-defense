using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.EventChannels
{
    public abstract class EventChannel<T> : ScriptableObject
    {
        readonly HashSet<IEventListener<T>> _observers = new();

        public void Invoke(T value)
        {
            foreach (var observer in _observers)
            {
                observer.Raise(value);
            }
        }

        public void Register(IEventListener<T> observer) => _observers.Add(observer);
        public void Unegister(IEventListener<T> observer) => _observers.Remove(observer);
    }
    public readonly struct Empty { }
    [CreateAssetMenu(menuName ="Events/EventChannel")]
    public class EventChannel : EventChannel<Empty> { }

}
