﻿namespace nickeltin.Interfaces
{
    public interface ILifeCycle : IDisable, IEnable
    {
        bool Disabled { get; }
    }
}