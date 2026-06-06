namespace OliveOilEshop.Pipelines;

public interface IUserScopeRequest
{
    long UserId { get; }
    void SetUserId(long userId);
}