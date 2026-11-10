# catalog em Kubernetes local (Kind / Docker Desktop)

Manifests só para cluster local (API e worker em deployments separados, volume compartilhado).
Na AWS o deploy usa `fcg-orchestration/k8s/eks` (API e worker no mesmo pod).

0. Gere as imagens com as tags que os deployments esperam (no Kind, carregue no cluster):

```bash
docker build --secret id=GITHUB_TOKEN,env=NUGET_AUTH_TOKEN -t fcg-catalog-api:latest .
docker build --secret id=GITHUB_TOKEN,env=NUGET_AUTH_TOKEN -t fcg-catalog-worker:latest -f Dockerfile.worker .
kind load docker-image fcg-catalog-api:latest fcg-catalog-worker:latest    # só no Kind
```

1. Crie o Secret real (o `secret.yaml` daqui é só modelo; as chaves AWS `local` são as fictícias
   que o DynamoDB local aceita):

```bash
kubectl create secret generic catalog-api-secret -n fcgames \
  --from-literal=jwt-key="$JWT__KEY" \
  --from-literal=db-connection="Data Source=/data/catalog.db" \
  --from-literal=rabbitmq-username="$RABBITMQ__USERNAME" \
  --from-literal=rabbitmq-password="$RABBITMQ__PASSWORD" \
  --from-literal=aws-access-key-id="local" \
  --from-literal=aws-secret-access-key="local"
```

2. Aplique os demais manifests (não use `kubectl apply -f k8s/`: aplicaria o modelo por cima do Secret):

```bash
kubectl apply -f k8s/configmap.yaml -f k8s/deployment-api.yaml -f k8s/deployment-worker.yaml -f k8s/pvc.yaml -f k8s/service.yaml
```

3. Teste:

```bash
kubectl get pods -n fcgames -l 'app in (catalog-api,catalog-worker)'
kubectl port-forward -n fcgames svc/catalog-api 5002:80    # http://localhost:5002/health
```

Precisa do RabbitMQ e do DynamoDB local no namespace `fcgames` (manifests em `fcg-orchestration/k8s`).
