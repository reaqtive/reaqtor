# `Nuqleon.Time`

Provides a set of abstractions over notions of time, including clocks and stopwatches.

> **Note:** These types are a generalization of time constructs in Rx, at the level of schedulers. They're generally usable in a variety of contexts, e.g. cache management.

## `IClock`

`IClock` provides an abstraction of a clock providing ticks in values of type `long` through a property called `Now`. A variety of clock sources can be implemented using this interface.

> **Note:** This simple interface generalizes the `Now` property on `IScheduler` in Rx. Conversions to other time representations can be layered on top.

## `IStopwatch[Factory]`

These interfaces provide an abstraction over `System.Diagnostics.Stopwatch` by providing a factory type and a stopwatch interface type. Implementations of these interfaces can wrap existing stopwatches or be backed by a virtual time source.

> **Note:** This provides a generalization over the corresponding types in the Rx scheduler infrastructure.
