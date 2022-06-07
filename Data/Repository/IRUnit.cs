using System;
using System.Collections.Generic;
using System.Text;
namespace api.Data{
    public interface IRUnit : IDisposable
    {
        
         IREmployee Employee { get; }
        void SaveChanges();
        bool isSave(string data);
        string GetNewID();
        bool isSafe(object data);
    }
}