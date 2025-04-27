graph TD
    A[ClientBenchmarking Tool\n(wrk, JMeter, k6)] -->|HTTP Requests| B[Load Balancer\n(AWS ALB)]
    
    subgraph Application Layer
        B -->|Route /aspnet| C1[ASP.NET Core\nInstance 1]
        B -->|Route /aspnet| C2[ASP.NET Core\nInstance 2]
        B -->|Route /nestjs| D1[NestJS\nInstance 1]
        B -->|Route /nestjs| D2[NestJS\nInstance 2]
        
        C1 --> E[UserService\n(ASP.NET Core)]
        C2 --> E
        D1 --> F[UserService\n(NestJS)]
        D2 --> F
    end
    
    subgraph Infrastructure Layer
        E -->|Cache| G[Redis\n(ElastiCache)]
        E -->|DB Write| H[SQL Server\n(RDS)]
        E -->|DB Read| I[SQL Server\nRead Replica]
        F -->|Cache| G
        F -->|DB Write| J[MongoDB\n(Atlas)]
        F -->|DB Read| K[MongoDB\nRead Replica]
        
        H -->|Replication| I
        J -->|Replication| K
    end
    
    subgraph Monitoring
        C1 --> L[CloudWatch/Monitor]
        C2 --> L
        D1 --> L
        D2 --> L
        L -->|Auto-Scale| M[ECS/App Service/Cloud Run]
    end
    
    classDef aspnet fill:#0078D4,stroke:#005BA1,stroke-width:2px;
    classDef nestjs fill:#E0234E,stroke:#000,stroke-width:2px;
    class C1,C2,E,H,I aspnet;
    class D1,D2,F,J,K nestjs;