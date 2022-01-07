namespace grid_puzzle_dungeon_engine.Content;

public class GenericRequest<T>
{
    public readonly bool Success = false;
    public readonly T Result;
        
    public GenericRequest(T result, bool success)
    {
        Result = result;
        Success = success;
    }
}