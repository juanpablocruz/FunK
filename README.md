# FunK
FunK is a library for functional programming in C#. It contains most of the most common helpers and constructs for this matter.

## Usage

Most of the type lifters or constructors are inside the static class F, so you need to include ```using static F``` in order to
freely acces them.

```csharp
using FunK;
using static F;

public static class Guard
{
    public static Maybe<T> NotNull<T>(object value)
    {
        if (value is null)
        {
            return Nothing;
        }
        return Just((T)value);
    }
}

```

## Functional Monads and Functors
This library provides some of the most common monads and functors as well as new ones designed to ease the functional paradigm in
the C# ecosystem.

- Maybe
- Either
- Result
- Identity
- Validation

- Excepcional: Designed as a more verbose either, holding either a value or an Exception. It can be casted from either of those values.
- Try: Designed to wrap a function that pottentially throws an Exception, allowing you to lazy execute it returning an Exceptional from it.


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)

