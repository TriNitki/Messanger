# Messenger

### MSG.Security configuration example
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",

  "ServiceName": "authorization",
  "ConsulUri": "http://localhost:8500",

  "ConnectionStrings": {
    "Npgsql": "host=localhost;database=msg_security;username=postgres;password=postgres"
  },

  "SecurityOptions": {
    "AccessTokenLifetimeInMinutes": 60,
    "RefreshTokenLifetimeInMinutes": 180,
    "ServiceAccessTokenLifeTimeInDays": 7,
    "SecretKey": "LDktKdoQak3Pk0cnXxCltA-LDktKdoQak3Pk0cnXxCltA",
    "PasswordOptions": {
      "Salt": "gnbkIzYEX1qG8UWrqmjB5S9jnsSeSHpf"
    }
  },

  "RoleOptions": {
    "DefaultUserRoles": [
      "DefaultRole"
    ]
  }
}
```

### MSG.Messenger configuration example

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",

  "ServiceName": "messenger",
  "ServiceSecret": "messenger",

  "ConnectionStrings": {
    "Npgsql": "host=localhost;database=msg_messenger;username=postgres;password=postgres"
  },

  "ConsulUri": "http://localhost:8500",
  "FabioUrl": "http://localhost:9998",

  "SecretKey": "LDktKdoQak3Pk0cnXxCltA-LDktKdoQak3Pk0cnXxCltA",
  "SecurityServiceUri": "https://localhost:7150"
}
```
