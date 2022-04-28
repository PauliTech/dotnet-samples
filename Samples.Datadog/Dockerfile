#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
ENV CORECLR_PROFILER_PATH="/app/bin/Debug/net6.0/datadog/linux-x64/Datadog.Trace.ClrProfiler.Native.so"
ENV DD_DOTNET_TRACER_HOME="/app/bin/Debug/net6.0/datadog"
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Samples.Datadog/Samples.Datadog.csproj", "Samples.Datadog/"]
RUN dotnet restore "Samples.Datadog/Samples.Datadog.csproj"
COPY . .
WORKDIR "/src/Samples.Datadog"
RUN dotnet build "Samples.Datadog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Samples.Datadog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Samples.Datadog.dll"]