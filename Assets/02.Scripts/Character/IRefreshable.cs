using System;

public interface IRefreshable
{
    event Action OnChanged;
}
