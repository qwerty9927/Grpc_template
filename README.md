# Grpc_template

Grpc_template is a gRPC-based project built using .NET 8.0. It provides a template for creating gRPC services with features like interceptors, custom helpers, and structured responses.

## Features

- **gRPC Services**: Includes a sample `GreeterService` with a `SayHello` method.
- **Protobuf Definitions**: Protobuf files for defining gRPC services and messages.
- **Interceptors**: Custom interceptor for handling exceptions (`CallProcessingInterceptor`).
- **Helpers**: Utility methods for converting and handling gRPC messages (`GrpcHelper`).
- **Structured Responses**: Base response classes for consistent API responses.
- **Reflection**: gRPC reflection enabled for easier debugging and client generation.

## Project Structure

- **Protos/**: Contains `.proto` files for defining gRPC services and messages.
- **Services/**: Contains gRPC service implementations.
- **Common/**: Contains shared classes like `BaseResponse` and `BaseException`.
- **Helpers/**: Contains utility classes like `GrpcHelper`.
- **Interceptors/**: Contains gRPC interceptors like `CallProcessingInterceptor`.
- **Properties/**: Contains project configuration files like `launchSettings.json`.

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- A gRPC client for testing (e.g., [BloomRPC](https://github.com/bloomrpc/bloomrpc) or [Postman](https://www.postman.com/)).

### Running the Project

1. Clone the repository:

   ```bash
   git clone <repository-url>
   cd Grpc_template
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Run the project:

   ```bash
   dotnet run
   ```

4. The gRPC server will be available at:
   - HTTP: `http://localhost:5254`
   - HTTPS: `https://localhost:7230`

### Testing the gRPC Service

Use a gRPC client to test the `GreeterService`:

- Service: `Greeter`
- Method: `SayHello`
- Request:
  ```json
  {
    "name": "YourName"
  }
  ```

### Configuration

- **Logging**: Configure logging levels in `appsettings.json` or `appsettings.Development.json`.
- **Kestrel**: Configure HTTP/2 settings in `appsettings.json`.

## Development

### Adding a New gRPC Service

1. Define the service and messages in a `.proto` file under `Protos/`.
2. Add the `.proto` file to the `<Protobuf>` section in `Grpc_template.csproj`.
3. Implement the service in the `Services/` folder.

### Interceptors

Custom interceptors can be added to handle cross-cutting concerns like logging, authentication, or exception handling. Example: `CallProcessingInterceptor`.

### Helpers

Use `GrpcHelper` for converting objects to gRPC messages and vice versa.

## Dependencies

- [Grpc.AspNetCore](https://www.nuget.org/packages/Grpc.AspNetCore)
- [Mapster](https://www.nuget.org/packages/Mapster)
- [Grpc.AspNetCore.Server.Reflection](https://www.nuget.org/packages/Grpc.AspNetCore.Server.Reflection)

## License

This project is licensed under the MIT License. See the LICENSE file for details.

## Acknowledgments

- [gRPC](https://grpc.io/)
- [Protobuf](https://developers.google.com/protocol-buffers)
