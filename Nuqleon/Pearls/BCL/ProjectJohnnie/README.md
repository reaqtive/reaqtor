# `ProjectJohnnie`

Memory diagnostics and optimizations for managed heaps at runtime.

## History

The high density of reactive micro-computations in query engines has been one of the major selling points of Nuqleon, allowing for millions of standing queries processing thousands of events per seconds per physical process. In most deployments, the size of a single query engine instance is capped to an order of magnitude of tens of thousands standing queries, in order to allow for reasonable checkpoint and recovery times. As a result, we host tens to hundreds of query engines per physical process, as stateful service replicas where only the primary replica of a query evaluator service hydrates all of the query expressions and state. The only shared resource are:

* the physical scheduler (to cap the number of threads used);
* compiled delegate caches;
* some ingress and egress components.

However, there's often much more hidden shared state of immutable objects (e.g. expression trees nodes, say `Expression.Constant(1)`) whose instantiation is not always under our control, so caching isn't always possible. This is especially true if other frameworks are loaded by artifacts in query engines (e.g. ML libraries instantiating models as immutable runtime objects). As such, it can make sense to try to compact the heap, especially because state in query engines tends to be long-lived due to the nature of reactive standing queries.

This project introduced heap walking and minipulation as a managed library, and has been plugged into the query evaluator host processes for memory constrained environments that need ultra-high density, and where the runtime cost of compaction (by means of deduplication of shared immutable objects) is negligible compared to the memory gains to be had.

## Name

Because the utilities in this library *walk* the heap, sometimes in random patterns, the project was named after a famous *whiskey* brand.

## Mechanism

Heap walkers start from a given set of *root* objects and traverse the heap by finding references to other objects, for example through fields or elements in arrays. They also keep track of cycles while doing the walk, to avoid getting stuck in endless cycles. This mechanism of walking the heap is very similar to what the CLR's garbage collector does internally as well.

While walking the heap, walkers can yield information about the edges traversed (such as fields and array elements). A *fence* can be provided to limit the walk based on certain conditions (in addition to the built-in cycle detection check). For example, when we're walking a query engine, there are various references to host-level facilities, which in turn refer to dictionaries that refer to all the query engines in the process. If we were to walk all references starting from a query engine, we'd end up traversing into sibling engines as well. A fence can be used to prune the traversal at edges that escape an individual engine (e.g. references to a "host context").

Different specializations of heap walkers can perform different operations, for example:

* Gather statistics about objects and their types. This can include instance count, finding boxed values, aggregating sizes of objects, look for big arrays, etc.
* Edit the heap by reassigning fields or array elements. This has to be used very carefully because it can break reference equality that's depended upon.

Heap walkers can be implemented using late-bound accessors (i.e. using reflection to access fields or inspect arrays), or can use expression trees to generate fast walkers for types (see `FastHeapReferenceWalker`).

## Usage

Examples of heap walkers to compute statistics and to optimize object graphs can be found in the `ProjectJohnnie` application.
