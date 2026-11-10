# fcg-catalog-service (CatalogAPI + Worker)

Jogos, lista de desejos, compras e biblioteca do usuário. A API inicia a compra publicando
`PedidoRealizadoEvento`; o worker consome os eventos de usuário criado e de pagamento processado.
Só valida o JWT emitido pelo users (o `UsuarioId` vem do token).

## Endpoints

| Endpoint | Acesso | O que faz |
|---|---|---|
| `GET /jogos` | autenticado | lista jogos |
| `POST /jogos` · `PUT /jogos/{id}` · `DELETE /jogos/{id}` | Admin | cadastra, atualiza, remove |
| `POST /compras` | autenticado | cria pedido `Pendente`, publica `PedidoRealizadoEvento`, responde 202 com `orderId` |
| `GET /compras/{orderId}` | autenticado | status do pedido: `Pendente`, `Aprovado` ou `Rejeitado` |
| `GET /biblioteca/{usuarioId}` | autenticado | jogos do usuário |
| `GET /desejos` · `POST /desejos` · `DELETE /desejos/{jogoId}` | autenticado | lista de desejos |
| `GET /health` · `GET /health/live` | anônimo | health checks |
| `GET /metrics` | anônimo | métricas Prometheus |

Na subida a API cria as tabelas do DynamoDB se faltarem e semeia 3 jogos (Hades R$ 49,90,
Hollow Knight R$ 29,90, Cyberpunk 2077 R$ 149,90 — este último é rejeitado no pagamento, por passar de R$ 100).

## Eventos

| Evento | Papel | Fila | O que acontece |
|---|---|---|---|
| `UsuarioCriadoEvento` | consome (worker) | `catalog-usuario-criado` | cria a biblioteca vazia (ignora se já existe) |
| `PagamentoProcessadoEvento` | consome (worker) | `catalog-pagamento-processado` | `Aprovado`: pedido aprovado + jogo na biblioteca; `Rejeitado`: pedido rejeitado (ignora pedido já finalizado) |
| `PedidoRealizadoEvento` | publica (API) | — | consumido pelo payments |

Contratos: [`fcg-integration-events`](https://github.com/FIAP-POS-TECH-TEAM-10/fcg-integration-events).

## Dados

- **DynamoDB**: tabelas `Jogos` e `Desejos` (fora do cluster; sobrevivem ao desligar). Local: `dynamodb-local` do compose.
- **SQLite**: `Bibliotecas`, `ItensBiblioteca`, `Pedidos` (API e worker usam o mesmo arquivo).
- **Redis**: cache das consultas de jogos; sem `ConnectionStrings__Redis` usa cache em memória.

## Variáveis de ambiente

| Variável | Obrigatória | O que é |
|---|---|---|
| `JWT__KEY` · `JWT__ISSUER` | sim (API) | validação do JWT (mesma chave do users) |
| `ConnectionStrings__DefaultConnection` | não (`Data Source=fcgames.db`) | SQLite |
| `ConnectionStrings__Redis` | não | ex.: `redis:6379` |
| `DynamoDb__ServiceUrl` | não | URL do DynamoDB local; vazio = DynamoDB da AWS |
| `DynamoDb__Region` | não | região do DynamoDB |
| `Messaging__Provider` | não | `Sqs` = SQS/SNS (AWS); vazio + `RabbitMQ__Host` = RabbitMQ; nenhum = em memória |
| `AWS__Region` | não | região do SQS/SNS |
| `RabbitMQ__Host` · `RabbitMQ__Username` · `RabbitMQ__Password` | com RabbitMQ | broker local |

Segredos não ficam no repositório: localmente vêm do `.env` do
[`fcg-orchestration`](https://github.com/FIAP-POS-TECH-TEAM-10/fcg-orchestration); no EKS, do Secret `fcg-jwt`.

## Rodar local

Jeito recomendado: `docker compose up` no `fcg-orchestration`. Só este serviço (precisa de
RabbitMQ e DynamoDB local, ou das variáveis acima):

```bash
export NUGET_AUTH_TOKEN=$(gh auth token)   # pacote FCGames.IntegrationEvents (read:packages)
dotnet run --project app/src/Fiap.FCGames.Catalogo.Api       # http://localhost:5002 (Swagger em Development)
dotnet run --project app/src/Fiap.FCGames.Catalogo.Worker    # outro terminal
```

Imagens: `docker build --secret id=GITHUB_TOKEN,env=NUGET_AUTH_TOKEN -t fcg-catalog-service .`
(API) e `-f Dockerfile.worker` (worker).

## Na AWS

- Pod `catalog-api` no EKS `fcg-eks` (namespace `fcgames`) com 2 containers: `api` e `worker`
  (mesmo SQLite em disco efêmero). ALB em `/jogos`, `/compras`, `/biblioteca`, `/desejos`.
- Imagens no ECR `fcg-catalog-service` (`:<sha7>`/`:latest` e `:worker-<sha7>`/`:worker-latest`).
- SQS/SNS e DynamoDB pela role `fcg-eks-catalog-api` (Pod Identity). Redis é um pod no cluster.
- Manifests: `fcg-orchestration/k8s/eks`.

## CI/CD

`.github/workflows/ci-cd.yml`: PR → build, testes, imagens e Trivy. Merge na `main` → também push no
ECR e rolling update do pod (API e worker juntos); com o cluster desligado o deploy é pulado e as
imagens entram no próximo "Ligar". Ainda não há projeto de testes.

Para Kind/Docker Desktop local, ver [`k8s/README.md`](k8s/README.md).
