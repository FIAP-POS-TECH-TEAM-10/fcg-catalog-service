output "ecr_repository_url" {
  value       = aws_ecr_repository.app_repo.repository_url
  description = "URI do repositório ECR criado para o microsserviço"
}
