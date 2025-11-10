# Desafio Backend - API de Locação de Veículos

Esta é uma API RESTful para gerenciamento de locação de motos e entregadores, construída em .NET 8 seguindo os princípios da Clean Architecture.

A aplicação gerencia o cadastro de motos, entregadores (com upload de CNH), e um sistema completo de locação com cálculo de custos, multas e taxas, além de usar mensageria (RabbitMQ) para notificações.

## Stack Tecnológica

* **API:** .NET 8 (C#)
* **Arquitetura:** Clean Architecture
* **Banco de Dados:** PostgreSQL (via Docker)
* **Mensageria:** RabbitMQ (via Docker)
* **ORM:** Entity Framework Core 8
* **Containerização:** Docker & Docker Compose

---

## 🚀 Como Rodar o Projeto

O projeto foi configurado para uma inicialização "zero-atrito". Os serviços de infraestrutura (Postgres e RabbitMQ) são gerenciados pelo Docker, e a API .NET roda localmente, conectando-se a eles.

### Pré-requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (deve estar **em execução**)
* Um cliente Git

---

### Iniciar os Serviços de Infraestrutura

# Em um terminal na raiz do projeto, execute:
docker-compose up -d
# Aguarde um momento até que os containers estejam em execução.

```bash
