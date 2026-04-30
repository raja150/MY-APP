namespace TranSmart.Core.Result
{
    public class Result<T> : BaseResult
    {
        private T _returnType;

        public Result()
        {
        }

        public Result(bool isSuccess) : base(isSuccess)
        {
        }

        public Result(T returnValue, bool isSuccess = false) : base(isSuccess)
        {
            _returnType = returnValue;
        }

        public T ReturnValue { get { return _returnType; } set { _returnType = value; } }
    }
}