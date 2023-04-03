namespace TelegramBots.Shared
{
    public class Result<T>
    {
        private Result()
        {
        }

        private Result(T value)
        {
            this.Value = value;
        }

        public T? Value { get; }

        public bool Success => this.Value is not null;

        public bool Failed => !this.Success;

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
