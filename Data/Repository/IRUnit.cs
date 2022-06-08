using System;
using System.Collections.Generic;
using System.Text;
namespace api.Data{
    public interface IRUnit : IDisposable
    {
        
         IREmployee Employee { get; }
        void SaveChanges();
        void SaveChangesAsync();
        bool isSave(string data);
        string GetNewID();
    }
}