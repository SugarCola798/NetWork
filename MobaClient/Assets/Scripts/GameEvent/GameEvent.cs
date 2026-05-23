using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Game
{
    public class GameEvent : GameSingleton<GameEvent>
    {
        public ConcurrentDictionary<Type, Delegate> eventHandlers = new ();
        
        public void Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            try
            {
                /*if (!eventHandlers.TryGetValue(type, out Delegate handlerDelegate))
            {
                eventHandlers.TryAdd(type, handler);
            }
            else
            {
                eventHandlers[type] = Delegate.Combine(handlerDelegate, handler);
            }*/  //不能这样写，有线程操作风险
                var type = typeof(T);
                eventHandlers.AddOrUpdate(type, handler, (_, existing) => 
                    Delegate.Combine(existing, handler));
            }
            catch (Exception e)
            {
                Debug.Log($"Subscribe Type: {typeof(T).FullName} error: {e.Message}");
            }
        }
        
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="handler"></param>
        /// <typeparam name="T"></typeparam>
        public void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
        {
            try
            {
                var type = typeof(T);
                eventHandlers.AddOrUpdate(type, _ => null, (_, existing) => Delegate.Remove(existing, handler));

                if (eventHandlers.TryGetValue(typeof(T), out var existing) && existing == null)
                {
                    eventHandlers.TryRemove(typeof(T), out var _);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Unsubscribe Type: {typeof(T).FullName} error: {e.Message}");
                throw;
            }
        }

        public void Publish<T>(T obj) where T : IGameEvent
        {
            var type = typeof(T);
            if (eventHandlers.TryGetValue(type, out var handler))
            {
                foreach (Action<T> action in handler.GetInvocationList())
                {
                    try
                    {
                        action(obj);
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"Publish Type: {typeof(T).FullName} error: {e.Message}");
                        throw;
                    }
                }   
            }
        }
    }
}
