using System.Collections.Generic;

public static class ItemDictionaryConverter
{
    public static ItemDictionaryWrapper ToWrapper(Dictionary<int, Item> dictionary)
    {
        var wrapper = new ItemDictionaryWrapper();
        foreach (var kvp in dictionary)
        {
            wrapper.items.Add(new ItemKeyPair(kvp.Key, kvp.Value));
        }

        return wrapper;
    }

    public static Dictionary<int, Item> ToDictionary (ItemDictionaryWrapper wrapper)
    {
        var dictionary = new Dictionary<int, Item>();
        foreach (var pair in wrapper.items)
        {
            dictionary[pair.key] = pair.value;
        }

        return dictionary;
    }
}
