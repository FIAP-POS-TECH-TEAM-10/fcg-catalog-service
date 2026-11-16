# Arquitetura do Catálogo: antes e agora


## Antes: persistência relacional

```mermaid
flowchart LR
    Cliente["Cliente"] -->|"HTTP"| API["API do Catálogo"]
    API --> EF["Entity Framework"]
    EF --> SQLite[("SQLite<br/>Jogos, Pedidos e Bibliotecas")]
```

Jogos, pedidos e bibliotecas eram persistidos no SQLite. A lista de desejos ainda não existia nessa implementação.

## Agora: persistência poliglota no EKS

```mermaid
flowchart LR
    Cliente["Cliente"] -->|"HTTP + JWT"| API

    subgraph EKS["EKS"]
        subgraph Pod["Pod do Catálogo"]
            API["API do Catálogo"]
            Worker["Worker"]
            App["Handlers e repositórios"]
            EF["Entity Framework"]
            SDK["SDK AWS / DynamoDBContext"]
            SQLite[("SQLite em emptyDir<br/>Pedidos e Bibliotecas")]
            SA["Service account catalog-api"]

            API --> App
            App -->|"Jogos e desejos"| SDK
            App -->|"Pedidos e bibliotecas"| EF
            EF --> SQLite
            Worker --> SQLite
        end
    end

    SA -. "Pod Identity" .-> Role
    Role -. "Autoriza acesso" .-> Dynamo
    subgraph AWS["AWS: fora do cluster"]
        Role["Role IAM do Catálogo"]
        Dynamo[("DynamoDB<br/>Jogos e Desejos")]
    end
    SDK -->|"Leitura e gravação"| Dynamo
```

O SQLite do manifesto EKS é efêmero; o DynamoDB é um serviço gerenciado fora do cluster. No ambiente local, a API usa o DynamoDB Local em vez do serviço da AWS. O diagrama destaca a persistência do Catálogo, não todos os componentes da plataforma.
