using System.Collections.Generic;
using System.Linq;

namespace DiscordLogHook.Utilities {
    internal class RollingQueue {
        private int limit;
        private readonly Queue<string> _queue;

        public RollingQueue(int capacity) {
            limit = capacity;
            _queue = new Queue<string>(capacity);
        }

        public void Add(string item) {
            if (_queue.Count >= limit) {
                _queue.Dequeue(); // forget
            }
            _queue.Enqueue(item);
        }

        public List<string> GetLines() {
            var list = _queue.ToList();
            _queue.Clear();
            return list;
        }

        public void UpdateLimit(int newLimit) {
            limit = newLimit;
            while (_queue.Count >= limit) {
                _queue.Dequeue(); // forget
            }
        }
    }
}
