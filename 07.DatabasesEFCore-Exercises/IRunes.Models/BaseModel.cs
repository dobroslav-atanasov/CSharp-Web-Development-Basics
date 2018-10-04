using System;

namespace IRunes.Models
{
    public abstract class BaseModel<T>
    {
        public T Id { get; set; }
    }
}
