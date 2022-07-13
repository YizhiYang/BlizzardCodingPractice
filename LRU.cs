LruCache lruCache = new LruCache(5);
lruCache.set("1", "1");
lruCache.set("2", "2");
lruCache.set("3", "3");
lruCache.set("4", "4");
lruCache.set("5", "5");
lruCache.set("6", "6");

// should NOT get it as the size is 5
Console.WriteLine(lruCache.get("1"));

lruCache.set("7", "7");

// should NOT get it as 1 and 2 were evicted
Console.WriteLine(lruCache.get("1"));
Console.WriteLine(lruCache.get("2"));

// should get it as "7" is the most recently inserted one
Console.WriteLine(lruCache.get("7"));


public class Node
{
    public string key;
    public string val;

    public Node next;
    public Node prev;

    public Node(string keyParam, string valParam)
    {
        key = keyParam;
        val = valParam;
        next = null;
        prev = null;
    }
}

public class LruCache
{
    private int _maxElements;
    private Dictionary<string, Node> keyToNodeDictionary;
    private int size;
    private Node dummyHead;
    private Node dummyTail;

    /**
     * maxElements - sets the maximum number of elements that the cache can hold.
     */
    public LruCache(int maxElements)
    {
        _maxElements = maxElements;
        keyToNodeDictionary = new Dictionary<string, Node>();

        dummyHead = new Node("head", "head");
        dummyTail = new Node("tail", "tail");

        dummyHead.next = dummyTail;
        dummyTail.prev = dummyHead;
    }

    /**
     * Gets an element from the cache based on an unique key
     */
    public string get(string key)
    {
        if (!keyToNodeDictionary.ContainsKey(key))
        {
            return null;
        }
        Node node = keyToNodeDictionary[key];
        removeFromList(node);
        addNodeToHead(node);
        return node.val;
    }

    /** 
     * If the cache is full, the element that has been accessed least 
     * recently by either get(...) or set(...) is removed to make * room for the new element. 
     */
    public void set(string key, string value)
    {
        if (keyToNodeDictionary.ContainsKey(key))
        {
            Node node = keyToNodeDictionary[key];
            node.val = value;
            removeFromList(node);
            addNodeToHead(node);
            return;
        }
        if(size == _maxElements)
        {
            Node node = dummyTail.prev;
            keyToNodeDictionary.Remove(node.key);
            removeFromList(node);
        } else
        {
            size++;
        }
        Node newNode = new Node(key, value);
        keyToNodeDictionary.Add(key, newNode);
        addNodeToHead(newNode);
    }


    private void removeFromList(Node node)
    { 
        node.prev.next = node.next;
        node.next.prev = node.prev;
        node.prev = null;
        node.next = null;
    }

    private void addNodeToHead(Node node)
   {
        Node prevHead = dummyHead.next;
        dummyHead.next = node;
        node.next = prevHead;
        node.prev = dummyHead;
        prevHead.prev = node;
    }


}
