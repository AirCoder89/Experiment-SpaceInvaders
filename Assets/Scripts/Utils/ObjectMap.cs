using System.Collections.Generic;
using Core;

namespace Utils
{
    public static class ObjectMap
    {
        private static Dictionary<int, GameView> _viewsMap;

        public static void Subscribe(int instanceId, GameView inView)
        {
            if (_viewsMap == null) _viewsMap = new Dictionary<int, GameView>();
            if(_viewsMap.ContainsKey(instanceId)) return;
            _viewsMap.Add(instanceId, inView);
        }

        public static void Unsubscribe(int instanceId)
        {
            if (_viewsMap == null) _viewsMap = new Dictionary<int, GameView>();
            if(_viewsMap.ContainsKey(instanceId)) return;
            _viewsMap.Remove(instanceId);
        }

        public static GameView GetView(int instanceId)
            => _viewsMap.ContainsKey(instanceId) ? _viewsMap[instanceId] : null;
    }
}