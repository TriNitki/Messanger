# ProjectBase

Configuration example:
```
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
    "Npgsql": "host=localhost;database=test_db;username=postgres;password=postgres"
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
    ],
    "DefaultServiceRoles": [
      "Service"
    ]
  }
}
```
