namespace BARQ.Core.Enums;

public enum IntegrationProtocol
{
    REST = 0,
    SOAP = 1,
    GraphQL = 2,
    WebSocket = 3,
    gRPC = 4,
    MessageQueue = 5
}

public enum MessagePriority
{
    Low = 0,
    Normal = 1,
    High = 2,
    Critical = 3
}

public enum MessageStatus
{
    Pending = 0,
    Processing = 1,
    Completed = 2,
    Failed = 3,
    Retrying = 4,
    DeadLetter = 5
}

public enum IntegrationEventLevel
{
    Debug = 0,
    Info = 1,
    Warning = 2,
    Error = 3,
    Critical = 4
}

public enum IntegrationAlertSeverity
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

public enum IntegrationAuthenticationType
{
    None = 0,
    ApiKey = 1,
    Bearer = 2,
    Basic = 3,
    OAuth2 = 4,
    Certificate = 5
}
