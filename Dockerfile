# Official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the solution to the working directory in the container
COPY . .

# Restore the dependencies (NuGet packages) for the solution
RUN dotnet restore

# Build the application
RUN dotnet build

# Run tests (failing tests will not stop the build due to "|| true" allowing errors)
RUN dotnet test /app/CardValidation.Tests/ || true

# Expose port 80 to allow external traffic to access the container
EXPOSE 80

# Set the default command to run when the container starts (runs the web app)
CMD ["dotnet", "run", "--project", "/app/CardValidation.Web/CardValidation.Web.csproj"]