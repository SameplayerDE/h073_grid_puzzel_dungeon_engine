namespace grid_puzzle_dungeon_engine.Content;

public abstract class GenericResourceLoader<TK, T>
{
    // ReSharper disable once HeapView.ObjectAllocation.Evident
    protected Dictionary<TK, T> Resources = new Dictionary<TK, T>();
    protected TK FallBackKey = default;

    protected bool FallBackSet = false;
    //protected Dictionary<TK, Delegate> LoadLater = new Dictionary<TK, Delegate>();

    public virtual bool Add(TK key, T resource)
    {
        if (resource == null || key == null)
        {
            throw new ArgumentNullException();
        }

        return Resources.TryAdd(key, resource);
    }

    public virtual bool Add(TK key, T resource, bool asFallback = false)
    {
        if (resource == null || key == null)
        {
            throw new ArgumentNullException();
        }

        if (asFallback)
        {
            FallBackSet = true;
            FallBackKey = key;
        }

        return Resources.TryAdd(key, resource);
    }

    public virtual bool Remove(TK key)
    {
        if (key == null)
        {
            throw new ArgumentNullException();
        }

        // ReSharper disable once HeapView.PossibleBoxingAllocation
        if (FallBackSet && FallBackKey.Equals(key))
        {
            FallBackSet = false;
        }

        return Resources.Remove(key);
    }

    public virtual bool Remove(TK key, out T resource)
    {
        if (key == null)
        {
            throw new ArgumentNullException();
        }

        // ReSharper disable once HeapView.PossibleBoxingAllocation
        if (FallBackSet && FallBackKey.Equals(key))
        {
            FallBackSet = false;
        }

        return Resources.Remove(key, out resource);
    }

    public virtual bool Has(TK key)
    {
        if (key == null)
        {
            throw new ArgumentNullException();
        }

        return Resources.ContainsKey(key);
    }

    public virtual bool Has(TK key, out T resource)
    {
        if (key == null)
        {
            throw new ArgumentNullException();
        }

        return Resources.TryGetValue(key, out resource);
    }

    public virtual T Find(TK key)
    {
        return Has(key, out var result) ? result : (FallBackSet) ? Resources[FallBackKey] : default;
    }

    public virtual GenericRequest<T> Request(TK key)
    {
        if (Has(key, out var result))
        {
            return new GenericRequest<T>(result, true);
        }

        return FallBackSet
            ? new GenericRequest<T>(Resources[FallBackKey], true)
            : new GenericRequest<T>(default, false);
    }

    public virtual void LoadResources()
    {
        /*foreach (var (key, @delegate) in LoadLater)
        {
            var result = @delegate.DynamicInvoke();
            if (result is T resource)
            {
                Add(key, resource);
            }
            throw new Exception();
            
        }*/
    }
}