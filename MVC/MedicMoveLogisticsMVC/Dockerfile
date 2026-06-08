# Step 1: Use the .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["MedicMoveLogisticsMVC.csproj", "./"]
RUN dotnet restore "./MedicMoveLogisticsMVC.csproj"

# Copy the rest of the code and build the release
COPY . .
RUN dotnet publish "MedicMoveLogisticsMVC.csproj" -c Release -o /app/publish /p:useAppHost=false

# Step 2: Use the lighter ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# .NET 8 defaults to port 8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "MedicMoveLogisticsMVC.dll"]