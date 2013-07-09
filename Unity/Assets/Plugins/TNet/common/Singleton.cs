/*
   Tunner Net -  Data interception use ProtoBuf in game Development. 
   e-mail : dongliang17@126.com
   project: https://github.com/dongliang/Tuner-Net
*/
namespace Tuner
{
    public class Singleton<T> where T : new()
    {
        protected Singleton() { }
        protected static T instance = new T();
        public static T Instance
        {
            get { return instance; }
        }
    }
}