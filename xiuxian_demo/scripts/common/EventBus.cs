using Godot;
using System;
using System.Collections.Generic;

namespace XiuXianDemo.Common
{
    public class EventBus
    {
        private static EventBus _instance;
        private Dictionary<string, List<Action<object[]>>> _eventHandlers;

        private EventBus()
        {
            _eventHandlers = new Dictionary<string, List<Action<object[]>>>();
        }

        public static EventBus Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventBus();
                }
                return _instance;
            }
        }

        public void Subscribe(string eventName, Action<object[]> handler)
        {
            if (!_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers[eventName] = new List<Action<object[]>>();
            }
            _eventHandlers[eventName].Add(handler);
        }

        public void Unsubscribe(string eventName, Action<object[]> handler)
        {
            if (_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers[eventName].Remove(handler);
                if (_eventHandlers[eventName].Count == 0)
                {
                    _eventHandlers.Remove(eventName);
                }
            }
        }

        public void Publish(string eventName, params object[] args)
        {
            if (_eventHandlers.ContainsKey(eventName))
            {
                foreach (var handler in _eventHandlers[eventName])
                {
                    handler(args);
                }
            }
        }
    }
}