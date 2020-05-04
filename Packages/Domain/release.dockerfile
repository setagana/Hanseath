FROM mcr.microsoft.com/dotnet/core/sdk:3.1
ARG API_KEY

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else, pack and push
COPY . ./
RUN dotnet pack --no-restore -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg -o out && \
    dotnet nuget push out/MdB.Hanseath.Domain.*.nupkg -k $API_KEY -s https://api.nuget.org/v3/index.json