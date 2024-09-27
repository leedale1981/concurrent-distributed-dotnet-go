## Example Distributed + Concurrent Client Server Application.

This scenario is useful when you want to call a distributed server application (listening on multiple hostname/port combinations) and have each instance concurrently handle connections with the client only waiting for the first connection to return.


- .NET 8 Client creates async tasks and waits for the first task to complete.
- Go server listens to calls on specified port and concurrently handles each connection.
