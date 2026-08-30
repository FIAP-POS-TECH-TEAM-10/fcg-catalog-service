# ------------------------------------------------------------------------------
# 1. REDE PADRÃO E SECURITY GROUP
# ------------------------------------------------------------------------------
data "aws_vpc" "default" {
  default = true
}

data "aws_subnets" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

# Security Group liberando a porta da aplicação e SSH (opcional)
resource "aws_security_group" "ecs_sg" {
  name        = "${var.service_name}-ecs-sg"
  description = "Permite trafego de entrada para o container ECS"
  vpc_id      = data.aws_vpc.default.id

  # Libera a porta da aplicação para o mundo (Estudos)
  ingress {
    from_port   = var.app_port
    to_port     = var.app_port
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # Libera porta HTTP padrao
  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # Tráfego de saída ilimitado
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# ------------------------------------------------------------------------------
# 2. ROLES IAM DO ECS E DA INSTÂNCIA EC2
# ------------------------------------------------------------------------------

# Permite que a instância EC2 se comunique com o ECS Control Plane
resource "aws_iam_role" "ecs_instance_role" {
  name = "${var.service_name}-ecs-instance-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Action    = "sts:AssumeRole"
      Effect    = "Allow"
      Principal = { Service = "ec2.amazonaws.com" }
    }]
  })
}

resource "aws_iam_role_policy_attachment" "ecs_instance_role_policy" {
  role       = aws_iam_role.ecs_instance_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonEC2ContainerServiceforEC2Role"
}

resource "aws_iam_instance_profile" "ecs_instance_profile" {
  name = "${var.service_name}-ecs-instance-profile"
  role = aws_iam_role.ecs_instance_role.name
}

# ------------------------------------------------------------------------------
# 3. CLUSTER ECS + LAUNCH TEMPLATE + AUTO SCALING GROUP (FREE TIER)
# ------------------------------------------------------------------------------
resource "aws_ecs_cluster" "main" {
  name = var.cluster_name
}

# Template que inicializa a EC2 t2.micro configurada para o Cluster
resource "aws_launch_template" "ecs_ec2_template" {
  name_prefix   = "${var.service_name}-template-"
  image_id      = "ami-0c101bf811a80b660" # Amazon Linux 2023 ECS-Optimized (us-east-1)
  instance_type = "t2.micro"              # Elegível ao Free Tier

  iam_instance_profile {
    name = aws_iam_instance_profile.ecs_instance_profile.name
  }

  network_interfaces {
    associate_public_ip_address = true
    security_groups             = [aws_security_group.ecs_sg.id]
  }

  # Script de inicialização que vincula a instância ao Cluster recém-criado
  user_data = base64encode(<<-EOF
              #!/bin/bash
              echo "ECS_CLUSTER=${aws_ecs_cluster.main.name}" >> /etc/ecs/ecs.config
              EOF
  )
}

# Auto Scaling Group que mantém exatamente 1 instância micro rodando de graça
resource "aws_autoscaling_group" "ecs_asg" {
  name                = "${var.service_name}-asg"
  vpc_zone_identifier = data.aws_subnets.default.ids
  min_size            = 1
  max_size            = 1
  desired_capacity    = 1

  launch_template {
    id      = aws_launch_template.ecs_ec2_template.id
    version = "$Latest"
  }

  tag {
    key                 = "Name"
    value               = "${var.service_name}-ecs-host"
    propagate_at_launch = true
  }
}