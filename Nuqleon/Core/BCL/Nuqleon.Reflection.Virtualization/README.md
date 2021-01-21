# `Nuqleon.Reflection.Virtualization`

Provides interfaces for the .NET reflection APIs, enabling the construction of wrappers, alternative implementations, mocks, etc. One example of a wrapper over reflection is a memoizing cache to speed up expensive reflection operations.

> **Note:** Facilities in this assembly have been used to abstract over reflection in the context of conversion between slim "Bonsai" and regular "System.Linq.Expressions" expression trees, both to inject caching behavior, as well as to support assembly, type, and member rebinding policies.
