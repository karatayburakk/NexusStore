# NexusStore Backend

### **Caching Implementation in DataService**

- In DataService, the caching strategy is applied using a memcached client.
- The service uses a cache key (`"countries"`) with a set expiration (600 seconds).
- The code attempts to retrieve country data from the cache using GetValueOrCreateAsync. If the data exists in the cache, it returns that immediately.
- If caching fails (for example, due to network issues or memcached unavailability), the service falls back to fetching the data directly from the database via the repository.
- The fetched data from the database is then mapped to a DTO using AutoMapper before being returned.

### **ExceptionMiddleware for Error Handling**

- The ExceptionMiddleware intercepts exceptions during the request pipeline.
- When an exception is caught, the middleware sets the response content type to JSON.
- It selects an HTTP status code based on the type of exception (for example, returning 401 for UnauthorizedAccessException).
- An ErrorDetails object is created containing the status code, a message from the exception, and any additional stack trace details (shown only in development).
- The ErrorDetails object is serialized using configured JsonSerializerOptions (as defined in JsonOptions.cs) and written to the response.

Countries endpoint:

1. Enforces security through JWT authentication.
2. Tries to serve data efficiently by using caching.
3. Gracefully falls back to the database if caching fails.
4. If any database-related error occurs (or any unhandled exception), the ExceptionMiddleware catches the error. It then maps the exception (like DbException) to an appropriate HTTP status code, creates a custom JSON payload with details, and sends a consistent error response.
5. Provides consistent error responses with custom JSON payloads through centralized exception handling in the middleware.
