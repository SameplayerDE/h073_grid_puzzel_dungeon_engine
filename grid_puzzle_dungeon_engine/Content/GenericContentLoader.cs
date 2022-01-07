using Microsoft.Xna.Framework.Content;

namespace grid_puzzle_dungeon_engine.Content;

public abstract class GenericContentLoader<TK, T>
{
    // ReSharper disable once HeapView.ObjectAllocation.Evident
    protected Dictionary<TK, T> Content = new Dictionary<TK, T>();
    protected TK FallBackKey = default;

    protected bool FallBackSet = false;
    //protected Dictionary<TK, Delegate> LoadLater = new Dictionary<TK, Delegate>();

    public virtual bool Add(TK key, T resource)
    {
        if (resource == null || key == null)
        {
            throw new ArgumentNullException();
        }

        return Content.TryAdd(key, resource);
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

        return Content.TryAdd(key, resource);
    }

    public virtual bool Add(TK key, ContentManager contentManager, string resourcePath, bool asFallback = false)
    {
        var resource = contentManager.Load<T>(resourcePath);
        if (resource == null || key == null)
        {
            throw new ArgumentNullException();
        }

        if (asFallback)
        {
            FallBackSet = true;
            FallBackKey = key;
        }

        return Content.TryAdd(key, resource);
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

        return Content.Remove(key);
    }

    public virtual bool Remove(TK key, out T? resource)
    {
        if (key == null)
        {
            throw new ArgumentNullException();
        }

        // ReSharper disable once HeapView.PossibleBoxingAllocation
        if (FallBackKey != null && FallBackSet && FallBackKey.Equals(key))
        {
            FallBackSet = false;
        }

        return Content.Remove(key, out resource);
    }

    public virtual bool Has(TK key)
    {
        if (key == null)
        {
            throw new ArgumentNullException();
        }

        return Content.ContainsKey(key);
    }

    public virtual bool Has(TK key, out T? resource)
    {
        if (key == null)
        {
            throw new ArgumentNullException();
        }

        return Content.TryGetValue(key, out resource);
    }

    public virtual T? Find(TK key)
    {
        return Has(key, out var result) ? result : (FallBackSet) ? Content[FallBackKey] : default;
    }

    public virtual GenericRequest<T?> Request(TK key)
    {
        if (Has(key, out var result))
        {
            return new GenericRequest<T?>(result, true);
        }

        return FallBackSet ? new GenericRequest<T?>(Content[FallBackKey], true) : new GenericRequest<T?>(default, false);
    }

    public virtual void LoadContent(ContentManager contentManager)
    {
    }
}