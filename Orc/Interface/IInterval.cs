﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInterval.cs" company="ORC">
//   MS-PL
// </copyright>
// <summary>
//   The Interval interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Orc.Interface
{
    using System;

    /// <summary>
    /// The Interval interface.
    /// </summary>
    /// <typeparam name="T">
    /// T must be comparable.
    /// </typeparam>
    public interface IInterval<T> : IEquatable<IInterval<T>>, IComparable<IInterval<T>>
        where T : IComparable<T>
    {
        /// <summary>
        /// Gets the interval start endPoint value.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        IEndPoint<T> Min { get; }

        /// <summary>
        /// Gets the interval end endPoint value.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        IEndPoint<T> Max { get; }

        /// <summary>
        /// Intersects() returns a boolean on whether the specified value intersect the interval.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Intersects(T value);

        /// <summary>
        /// Overlaps() returns a boolean on whether another interval overlaps the interval.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Overlaps(IInterval<T> other);
    }
}