# Reaqtor

Reaqtor provides a set of libraries that are essential to building distributed, reliable, stateful, and highly scalable reactive solutions. It roughly consists of the following core components:

* Client-side abstractions used to build client libraries used to create and delete subscriptions, manage streams, publish events, etc.
* A query engine that provides reliable execution of Reaqtive queries, including state persistence through checkpointing.
* Facilities for reliable event flows across services, using sequence numbers and with replay capabilities.
* Libraries to aid with hosting of query engines in services.
