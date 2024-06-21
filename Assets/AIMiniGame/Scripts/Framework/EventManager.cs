using System;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager>
{
    private Dictionary<int, Delegate> eventDictionary = new Dictionary<int, Delegate>();

    private void Register(int eventId, Delegate listener)
    {
        if (eventDictionary.TryGetValue(eventId, out var del))
        {
            eventDictionary[eventId] = Delegate.Combine(del, listener);
        }
        else
        {
            eventDictionary[eventId] = listener;
        }
    }

    private void Unregister(int eventId, Delegate listener)
    {
        if (eventDictionary.TryGetValue(eventId, out var del))
        {
            var currentDel = Delegate.Remove(del, listener);
            if (currentDel == null)
            {
                eventDictionary.Remove(eventId);
            }
            else
            {
                eventDictionary[eventId] = currentDel;
            }
        }
    }

    private void Trigger(int eventId, params object[] parameters)
    {
        if (eventDictionary.TryGetValue(eventId, out var del))
        {
            del.DynamicInvoke(parameters);
        }
    }

    public void Register(int eventId, Action listener) => Register(eventId, (Delegate)listener);
    public void Register<T>(int eventId, Action<T> listener) => Register(eventId, (Delegate)listener);
    public void Register<T, U>(int eventId, Action<T, U> listener) => Register(eventId, (Delegate)listener);
    public void Register<T, U, V>(int eventId, Action<T, U, V> listener) => Register(eventId, (Delegate)listener);
    public void Register<T, U, V, W>(int eventId, Action<T, U, V, W> listener) => Register(eventId, (Delegate)listener);

    public void Unregister(int eventId, Action listener) => Unregister(eventId, (Delegate)listener);
    public void Unregister<T>(int eventId, Action<T> listener) => Unregister(eventId, (Delegate)listener);
    public void Unregister<T, U>(int eventId, Action<T, U> listener) => Unregister(eventId, (Delegate)listener);
    public void Unregister<T, U, V>(int eventId, Action<T, U, V> listener) => Unregister(eventId, (Delegate)listener);
    public void Unregister<T, U, V, W>(int eventId, Action<T, U, V, W> listener) => Unregister(eventId, (Delegate)listener);

    public void Trigger(int eventId) => Trigger(eventId, Array.Empty<object>());
    public void Trigger<T>(int eventId, T param) => Trigger(eventId, param);
    public void Trigger<T, U>(int eventId, T param1, U param2) => Trigger(eventId, param1, param2);
    public void Trigger<T, U, V>(int eventId, T param1, U param2, V param3) => Trigger(eventId, param1, param2, param3);
    public void Trigger<T, U, V, W>(int eventId, T param1, U param2, V param3, W param4) => Trigger(eventId, param1, param2, param3, param4);
}
