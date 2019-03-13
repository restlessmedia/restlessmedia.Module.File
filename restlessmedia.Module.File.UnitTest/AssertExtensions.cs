using Should.Core.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace restlessmedia.Module.File.UnitTest
{
  public static class AssertExtensions
  {
    public static void Fail(string message)
    {
      throw new Exception($"Assert failed with {message}");
    }

    public static void MustEqual<T>(this T actual, T expected)
    {
      Assert.Equal(expected, actual);
    }

    public static void MustNotBeNull(this object actual)
    {
      Assert.NotNull(actual);
    }

    public static void MustBeLike(this string actual, string expected)
    {
      Assert.False(string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase));
    }

    public static void MustNotBeLike(this string actual, string expected)
    {
      Assert.False(string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase));
    }

    public static void MustBeNull(this object actual)
    {
      Assert.Null(actual);
    }

    public static void MustContain<T>(this IEnumerable<T> actual, params T[] expected)
    {
      Assert.False(actual.Except(expected).Any());
    }

    public static void MustNotContain<T>(this IEnumerable<T> actual, params T[] expected)
    {
      Assert.False(actual.Intersect(expected).Any());
    }

    public static void MustBeTrue(this bool actual)
    {
      Assert.True(actual);
    }

    public static void MustBeFalse(this bool actual)
    {
      Assert.False(actual);
    }

    public static void IsA<T>(this Type type)
    {
      Assert.True(typeof(T).IsAssignableFrom(type));
    }

    public static void IsA<T>(this object obj)
    {
      IsA<T>(obj.GetType());
    }

    public static void MustNotThrow(this Action action)
    {
      Exception exception = null;

      try
      {
        action();
      }
      catch (Exception e)
      {
        exception = e;
      }

      exception.MustBeNull();
    }
  }
}