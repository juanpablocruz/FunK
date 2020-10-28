using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;

namespace FunK
{
  using static F;

  public static partial class F
  {
    public static Either.Left<L> Left<L>(L l) => new Either.Left<L>(l);
    public static Either.Right<R> Right<R>(R r) => new Either.Right<R>(r);
  }
  
  public struct Either<L, R>
  {
    internal L Left { get; }
    internal R Right { get; }

    private bool IsRight { get; }
    private bool IsLeft => !IsRight;

    internal Either(L left)
    {
      IsRight = false;
      Left = left;
      Right = default(R);
    }

    internal Either(R right)
    {
      IsRight = true;
      Right = right;
      Left = default(L);
    }

    public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);
    public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);

    public static implicit operator Either<L, R>(Either.Left<L> left) => new Either<L, R>(left.Value);
    public static implicit operator Either<L, R>(Either.Right<R> right) => new Either<L, R>(right.Value);

    public TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right)
      => IsLeft ? Left(this.Left) : Right(this.Right);

    public Unit Match(Action<L> Left, Action<R> Right)
      => Match(Left.ToFunc(), Right.ToFunc());

    public IEnumerator<R> AsEnumerable()
    {
      if (IsRight) yield return Right;
    }

    public override string ToString() => Match(l => $"Left({l})", r => $"Right({r})");
  }

  public static class Either
  {
    public struct Left<L>
    {
      internal L Value { get; }
      internal Left(L value) { Value = value; }

      public override string ToString() => $"Left({Value})";
    }

    public struct Right<R>
    {
      internal R Value { get; }
      internal Right(R value) { Value = value; }

      public override string ToString() => $"Right({Value})";

      public Right<RR> Map<L, RR>(Func<R, RR> f) => Right(f(Value));
      public Either<L, RR> Bind<L, RR>(Func<R, Either<L, RR>> f) => f(Value);
    }
  }
}
