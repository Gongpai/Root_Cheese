using UnityEditor;

namespace GDD
{
    public interface IState<T>
    {
        public string StateName();
        
        public void OnStart(T contrller);
        
        public void Handle(T contrller);

        public void OnExit();
    }
}