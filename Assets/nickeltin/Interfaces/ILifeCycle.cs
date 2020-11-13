﻿namespace nickeltin.Interfaces
{
    public interface ILifeCycle : IDisable, IEnable
    {
        bool Disabled { get; }
    }
    
    public interface IDisable
    {
        void Disable();
    }
    
    public interface IEnable
    {
        void Enable();
    }
}