# Catalog API - Kubernetes Deployment

Este diretório contém os manifestos Kubernetes para deploy do microsserviço Catalog API e seu Worker.

## Recursos

- **Deployment API**: Define os Pods da API
- **Deployment Worker**: Define os Pods do Worker (consumer)
- **Service**: Expõe a API dentro do cluster
- **ConfigMap**: Configurações não sensíveis
- **Secret**: Credenciais e dados sensíveis
- **PersistentVolumeClaim**: Volume compartilhado entre API e Worker (ReadWriteMany)

## Deploy

### 1. Build das Imagens Docker

```bash
# API
docker build -t fcg-catalog-api:latest -f Dockerfile .

# Worker
docker build -t fcg-catalog-worker:latest -f Dockerfile.worker .
```

### 2. Aplicar Manifestos

> **Atenção:** crie o Secret (seção abaixo) **antes** de aplicar e não use `kubectl apply -f k8s/` na pasta inteira: isso aplicaria o `secret.yaml` modelo (valores `REPLACE_ME`) por cima do Secret real.

```bash
kubectl apply -f k8s/configmap.yaml -f k8s/deployment-api.yaml -f k8s/deployment-worker.yaml -f k8s/pvc.yaml -f k8s/service.yaml
```

### 3. Verificar Status

```bash
# API
kubectl get pods -n fcgames -l app=catalog-api
kubectl logs -n fcgames -l app=catalog-api -f

# Worker
kubectl get pods -n fcgames -l app=catalog-worker
kubectl logs -n fcgames -l app=catalog-worker -f
```

## Configurações

### ConfigMap (configmap.yaml)
- `jwt-issuer`: Issuer do token JWT
- `rabbitmq-host`: Host do RabbitMQ
- `aspnetcore-urls`: URLs que a API escuta

### Secret (secret.yaml)
- `jwt-key`: Chave secreta para assinar tokens JWT
- `db-connection`: String de conexão do banco de dados
- `rabbitmq-username`: Usuário do RabbitMQ
- `rabbitmq-password`: Senha do RabbitMQ

**IMPORTANTE**: Altere os valores dos Secrets em produção!

O `secret.yaml` versionado é só um **modelo** (valores `REPLACE_ME`). Crie o Secret real com as suas credenciais:

```bash
kubectl create secret generic catalog-api-secret -n fcgames \
  --from-literal=jwt-key="$JWT__KEY" \
  --from-literal=db-connection="Data Source=/data/catalog.db" \
  --from-literal=rabbitmq-username="$RABBITMQ__USERNAME" \
  --from-literal=rabbitmq-password="$RABBITMQ__PASSWORD" \
  --from-literal=aws-access-key-id="local" \
  --from-literal=aws-secret-access-key="local"
```

## Acesso

### Port Forward para acesso local

```bash
kubectl port-forward -n fcgames svc/catalog-api 5002:80
```

Acesse: http://localhost:5002

## Endpoints

- `GET /api/jogos` - Listar jogos
- `GET /api/jogos/{id}` - Buscar jogo por ID
- `POST /api/jogos` - Criar jogo
- `PUT /api/jogos/{id}` - Atualizar jogo
- `DELETE /api/jogos/{id}` - Deletar jogo
- `GET /api/biblioteca/{usuarioId}` - Listar biblioteca do usuário
- `POST /api/compras` - Iniciar fluxo de compra
- `GET /health` - Health check

## Dependências

- RabbitMQ (deve estar rodando no namespace fcgames)

## Eventos Publicados

- **OrderPlacedEvent**: Publicado quando uma compra é iniciada

## Eventos Consumidos (Worker)

- **PaymentProcessedEvent**: Consumido para adicionar jogo à biblioteca se pagamento aprovado

## Nota sobre o Volume

O PVC usa `ReadWriteMany` para permitir que API e Worker compartilhem o mesmo banco SQLite. Em produção, considere usar um banco de dados externo.
