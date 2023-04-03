namespace TelegramBots.Shared
{
    public class Result<T>
    {
        private Result()
        {
        }

        private Result(T value)
        {
            Value = value;
        }

        public T? Value { get; }

        public bool Success => Value is not null;

        public bool Failed => !Success;

        public static Result<T> Ok(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Fail()
        {
            return new Result<T>();
        }
    }
}
